using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    /// <summary>
    /// Team model with attributes
    /// </summary>
    public class Team
    {
        [Key]
        [ScaffoldColumn(false)]
        public int teamId { get; set; }

        [MinLength(2), MaxLength(20)]
        [Required]
        public string teamName { get; set; }


    }
}
