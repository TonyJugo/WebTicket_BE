using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using WebTicket.Domain.Exceptions;

namespace WebTicket.Domain.Entities;

public class User : IdentityUser<string>
{
    //phone, password, email, dob

    public int wallet { get; set; } = 0; // Wallet is initialized to 0, can be used for storing user balance or credits.
    public string Mssv { get; set; } 
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    //non-nullable fk
    public string? UniversityId { get; set; } // FK từ bảng university

    [ForeignKey("UniversityId")]
    public University? University { get; set; } // Navigation property
    //factory method to create a new user instance
    public static User Create(string password, string id, string mssv,string email, string firstName, string lastName, string phoneNumber, string universityId)
    {

        //chuyển university name thành university id
        return new User
        {
            PasswordHash = password,
            Id = id, // Id is the primary key, can be Guid or string
            Mssv = mssv,
            Email = email,
            UserName = email,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            UniversityId = universityId, // Assuming universityName is the ID of the university
        };
    }
    
    public override string ToString()
    {
        return FirstName + " " + LastName;
    }
}