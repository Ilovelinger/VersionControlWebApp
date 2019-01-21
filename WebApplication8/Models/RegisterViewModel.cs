﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    /// <summary>
    /// This is the application register view model.
    /// </summary>
    public class RegisterViewModel
    {
        [Required, EmailAddress, MaxLength(256), Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required, MinLength(6), MaxLength(50), DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }

        [Required, MinLength(6), MaxLength(50), DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password not match")]
        public string ConfirmPassword { get; set; }

        [Required,MaxLength(25)]
        public string Firstname { get; set; }

        [Required,MaxLength(25)]
        public string Surname { get; set; }

        [Required, MaxLength(25)]
        public string NickName { get; set; }

        [Required, MaxLength(25)]
        public int MobilePhoneNumber { get; set; }

        [MaxLength(25)]
        public int KitNumber { get; set; }

        [MaxLength(25)]
        public string Position { get; set; }

        public String UserRole { get; set; }
    }

    public enum UserRole
    {
        Player,
        CommonUser
    }
}
