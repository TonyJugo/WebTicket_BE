﻿
using Microsoft.AspNetCore.Identity;
using System.Net;
using System;
using System.Text.RegularExpressions;
using WebTicket.Domain.Entities;
using WebTicket.Domain.Exceptions;
using WebTicket.Domain.Requests;
using System.ComponentModel;
using WebTicket.Application.Abstracts;

public class CustomValidator
{

    public async Task<(List<string>, bool)> ValidateUserAsync(RegisterRequest register, List<string> universityNames)
    {
        //email, password ko cần check null
        var errors = new List<ErrorResponse>();
        //email
        if (!Regex.IsMatch(register.Email, @"^(?!.*\s).+@(gmail\.com|fpt\.edu\.vn)$"))
        {
            errors.Add(new ErrorResponse
            {
                Description = "\nmail must be @fpt.edu.vn or @gmail.vn"
            });
        }

        //password
        if (register.Password.Length < 8)
        {
            errors.Add(new ErrorResponse
            {
   
                Description = "\nPassword must be at minimum 8 characters long"
            });
        }
        if (register.Password.Length > 16)
        {
            errors.Add(new ErrorResponse
            {
              
                Description = "\nPassword must be at maximum 16 characters long"
            });
        }

        //FirstName
        if (string.IsNullOrWhiteSpace(register.FirstName))
        {
            errors.Add(new ErrorResponse
            {
     
                Description = "\nFirst name can't be null"
            });
        }
        //chỉ chứa chữ cái
        // \p{L} là ký tự chữ cái Unicode 
        if (!Regex.IsMatch(register.FirstName, @"^\p{L}+$"))
            errors.Add(new ErrorResponse
            {
         
                Description = "\nFirst name contains only characters"
            });

        //LastName
        if (string.IsNullOrWhiteSpace(register.LastName))
        {
            errors.Add(new ErrorResponse
            {
         
                Description = "\nLast name can't be null"
            });
        }
        if (!Regex.IsMatch(register.LastName, @"^\p{L}+$"))
            errors.Add(new ErrorResponse
            {

                Description = "\nLast name contains only characters"
            });

        //PhoneNumber
        if (string.IsNullOrWhiteSpace(register.PhoneNumber))
        {
            errors.Add(new ErrorResponse
            {
           
                Description = "\nPhone number can't be null"
            });
        }

        //phone number length
        if (register.PhoneNumber.Length < 10 || register.PhoneNumber.Length > 10)
        {
            errors.Add(new ErrorResponse
            {
           
                Description = "\nPhone number must be at 10 digits"
            });
        }

        //phone number format
        if (!Regex.IsMatch(register.PhoneNumber, @"^(0|\+84)(3|5|7|8|9)"))
        {
            errors.Add(new ErrorResponse
            {
               
                Description = "\nInvalid phone number format"
            });
        }

        //UniversityId
        if (string.IsNullOrWhiteSpace(register.UniversityName))
        {
            errors.Add(new ErrorResponse
            {
         
                Description = "\nUniversity ID can't be null"
            });
        }
        bool a = false;
        foreach (var university in universityNames)
        {
            if (university.Equals(register.UniversityName))
            {
                a = true;
                break;
            }
        }
        if(!a)
        {
            errors.Add(new ErrorResponse
            {
                Description = "\nUniversity available: \n1. Đại học FPT\n2. Đại học Bách Khoa\n3. Đại học Khoa Học Tự Nhiên\n4. Đại học Công Nghệ Thông Tin"
            });
        }
        return errors.Any() ? (errors.Select(e => e.Description).ToList(), false) : (new List<string>(), true);
    }
    public async Task<(List<string>, bool)> ValidateUniversityAsync(UniversityRequest request){
        var errors = new List<ErrorResponse>();
        if (!Regex.IsMatch(request.Name, @"^Đại học( [A-ZÀ-Ỵ][a-zà-ỹ]*)+$"))
        {
            errors.Add(new ErrorResponse
            {
                Description = "\nUniversity name must start with 'Đại học' each character at the start of university name must be uppercase"
            });
        }
        return errors.Any() ? (errors.Select(e => e.Description).ToList(), false) : (new List<string>(), true);
    }
}
