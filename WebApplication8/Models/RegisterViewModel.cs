﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    /// <summary>
    /// Application register view model.
    /// </summary>
    public class RegisterViewModel
    {
        [Required, EmailAddress, Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string NickName { get; set; }

        [Required]
        public string FullName { get; set; }


        [Required]
        public int MobilePhoneNumber { get; set; }


        public int KitNumber { get; set; }

        public string Position { get; set; }

        [Required]
        public String UserRole { get; set; }

        public IFormFile AvatarImage { get; set; }
    }


    public enum UserRole
    {
        Player,
        CommonUser
    }

}

