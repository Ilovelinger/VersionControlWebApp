using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Models;

namespace WebApplication8.Data
{
    /// <summary>
    /// This the application database.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        //Create table "Matches".
        public DbSet<Match> Matches { get; set; }
        //Create table "Comments".
        public DbSet<Comment> Comments { get; set; }

        public DbSet<NewMatch> NewMatches { get; set; }
    }
}
