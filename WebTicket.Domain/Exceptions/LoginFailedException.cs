﻿namespace WebTicket.Domain.Exceptions;

//primary constructor
public class LoginFailedException(string email) : Exception($"wrong email or password.");

//viết hoàn chỉnh

//public LoginFailedException(string email)
//    : base($"Invalid email: {email} or password.")
//{
//}
//gọi constructor con thì truyền message khởi tạo cha