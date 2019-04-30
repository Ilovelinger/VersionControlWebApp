using Microsoft.AspNetCore.Http;
using System;
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

        public String UserRole { get; set; }

        public IFormFile AvatarImage { get; set; }
    }


    public enum UserRole
    {
        Player,
        CommonUser
    }

    //public enum Position
    //{
    //    Centre_back,
    //    Sweeper,
    //    Full_back,
    //    Wing_back,
    //    Centremidfield,
    //    Defensive_midfield,
    //    Attacking_midfield,
    //    Wide_midfield,
    //    Centre_forward,
    //    Second_striker,
    //    Winger,
    //    Goalkeeper
    //}
}

