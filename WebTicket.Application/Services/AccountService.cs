using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using WebTicket.Application.Abstracts;
using WebTicket.Application.Contracts;
using WebTicket.Domain.Constants;
using WebTicket.Domain.Entities;
using WebTicket.Domain.Enums;
using WebTicket.Domain.Exceptions;
using WebTicket.Domain.Requests;

namespace WebTicket.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAuthTokenProcessor _authTokenProcessor;
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly CustomValidator _validator;
    private readonly IUniversityService _universityService;
    private readonly IMemoryCache _memoryCache;
    public AccountService(IAuthTokenProcessor authTokenProcessor, UserManager<User> userManager, CustomValidator validator,
        IUserRepository userRepository, IUniversityService universityService, IMemoryCache memoryCache)
    {
        _authTokenProcessor = authTokenProcessor;
        _userManager = userManager;
        _userRepository = userRepository;
        _validator = validator;
        _universityService = universityService;
        _memoryCache = memoryCache;
    }

    public async Task RegisterAsync(RegisterRequest registerRequest)
    {
        //trim all
        StringTrimmerExtension.TrimAllString(registerRequest);
        //check xem user đã tồn tại chưa
        var userExists = await _userManager.FindByEmailAsync(registerRequest.Email) != null;

        if (userExists)
        {
            throw new UserAlreadyExistsException(email: registerRequest.Email);
        }
        //validate registerRequest
        List<string> universityNames = await _universityService.GetAllUniversityNames();
        var (erros, isValid) = _validator.ValidateUser(registerRequest, universityNames);
        // nếu không thành công do validate thì ném ra exception
        if (!isValid)
        {
            throw new RegistrationFailedException(erros);
        }
        //tìm university và tạo id user
       
        var universityId = await _userRepository.GetUniversityIdByNameAsync(registerRequest.UniversityName);
        string id = await GenerateUserId();
        //tạo user mới
        var user = User.Create(id, registerRequest.Mssv ,registerRequest.Email, registerRequest.FirstName, registerRequest.LastName, registerRequest.PhoneNumber, universityId);
        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, registerRequest.Password); //hash password trước khi lưu vào bảng AspNetUsers
        //gọi hàm CreateAsync để vừa check validate vừa lưu vào bảng AspNetUsers
         await _userManager.CreateAsync(user);
        //gán user với role vào bảng ASpNetUserRoles
        var addRoleResult = await _userManager.AddToRoleAsync(user, GetStringIdentityRoleName(Role.User));
        await _userManager.UpdateAsync(user); //cập nhật user sau khi thêm role
    }

    public async Task<string> LoginAsync(LoginRequest loginRequest)
    {
        //trim all
        StringTrimmerExtension.TrimAllString(loginRequest);
        //tìm user dựa trên login request
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        //không có user hoặc mật khẩu không đúng thì ném ra exception
        var result = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
      
        if (user == null || result == false)
        {
            throw new LoginFailedException(loginRequest.Email);
        }
        //gernater jwt token dựa trên role và user
        IList<string> roles = await _userManager.GetRolesAsync(user);

        var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user, roles);

        _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN",jwtToken, expirationDateInUtc);
        return jwtToken;
    }

    public async Task<string> LoginWithGoogleAsync(ClaimsPrincipal? principal)
    {
        var email = principal.FindFirstValue(ClaimTypes.Email);
        var user = await _userManager.FindByEmailAsync(email);
        //kiểm tra nếu user đã tồn tại chưa
        if (user == null) 
        {
            user = User.Create(await GenerateUserId(), string.Empty, email,
                principal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty,
                principal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty,
                string.Empty, null); // Assuming default university for Google users
            var result = await _userManager.CreateAsync(user);
            var addRoleResult = await _userManager.AddToRoleAsync(user, GetStringIdentityRoleName(Role.User));
            await _userManager.UpdateAsync(user); //cập nhật user sau khi thêm role
        }
        //tạo token đăng nhập
        IList<string> roles = await _userManager.GetRolesAsync(user);
        var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user, roles);
        _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN", jwtToken, expirationDateInUtc);
        return jwtToken;
    }
    public Task<SendEmailRequest> CreateOtp(string email)
    {
        var otp = new Random().Next(100000, 999999).ToString();
        _memoryCache.Set($"otp_{email}", otp, TimeSpan.FromMinutes(5));
        var emailRequest = new SendEmailRequest(email, "Your OTP", $"Your OTP is <strong style='font-size: 20px;'>{otp}</strong>", true);
        return Task.FromResult(emailRequest);
    }

    public async Task<(bool, string)> VerifyOtp(string email, string otp)
    {
        if (_memoryCache.TryGetValue($"otp_{email}", out string cachedOtp))
        {
            if (cachedOtp == otp)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if(user == null)
                {
                    throw new UserNotFoundException("User not found");
                }
                //gernater jwt token dựa trên role và user
                IList<string> roles = await _userManager.GetRolesAsync(user);

                var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user, roles);

                _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("RESET_TOKEN", jwtToken, expirationDateInUtc);
              

                _memoryCache.Remove($"otp_{email}"); // Xóa OTP sau khi xác thực thành công

                return (true, jwtToken);
            }
        }
        return (false, ""); // Trả về false nếu OTP không hợp lệ hoặc không tồn tại
    }

    public async Task ChangePassword(string userId, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if(user == null)
        {
            throw new UserNotFoundException($"User with ID {userId} not found.");
        }
        //validate password
        var (errors, isValid) = _validator.ValidatePassword(newPassword);
        if (!isValid)
        {
            throw new UpdateAddFailedException(errors);
        }
        var hashedPassword = _userManager.PasswordHasher.HashPassword(user, newPassword);
        user.PasswordHash = hashedPassword;
        await _userManager.UpdateAsync(user);
    }

    private string GetStringIdentityRoleName(Role role)
    {
        return role switch
        {
            Role.Moderator => IdentityRoleConstants.Moderator,
            Role.Staff => IdentityRoleConstants.Staff,
            Role.Organizer => IdentityRoleConstants.Organizer,
            Role.Admin => IdentityRoleConstants.Admin,
            Role.User => IdentityRoleConstants.User,
           _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Provided role is not supported.")
        };
    }
    private async Task<string> GenerateUserId()
    {
        string lastId = await _userRepository.GetLastId();
        if (lastId == null) return "User0001";
        int id = int.Parse(lastId.Substring(4)) + 1; // lấy id cuối cùng và cộng thêm 1
        string generatedId = "User" + id.ToString("D4");
        return generatedId;
    }



}