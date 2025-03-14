﻿using System.ComponentModel.DataAnnotations;

namespace FinanceMemos.API.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}