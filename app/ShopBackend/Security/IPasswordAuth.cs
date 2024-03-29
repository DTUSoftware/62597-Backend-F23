﻿using ShopBackend.Models;

namespace ShopBackend.Security
{
    public interface IPasswordAuth
    {
        bool VerifyPassword(string userPassword, string passwordToVerify);
        string GeneratePasswordHash(string password);
        bool IsPasswordStrong(string password);
        bool IsEmailValid(string password);
    }
}
