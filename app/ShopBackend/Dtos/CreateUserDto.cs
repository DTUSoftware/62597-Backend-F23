﻿
using ShopBackend.Models;

namespace ShopBackend.Dtos
{
    public class CreateUserDto
    {
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
