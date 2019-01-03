using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    /// <summary>
    /// This is the application user model with user attributes.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base() { }
       
        public string NickName { get; set; }

    }
}
