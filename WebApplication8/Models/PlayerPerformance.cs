using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    /// <summary>
    /// Player performance model with attributes
    /// </summary>
    public class PlayerPerformance
    {
        [Key]
        [ScaffoldColumn(false)]
        public int id { get; set; }

        //[MinLength(2), MaxLength(20)]
        [Required]
        public string linkedTeamname { get; set; }

        [Required]
        public string linkedplayername { get; set; }

        [Required]
        public String startup { get; set; }

        [Required]
        public String substitute { get; set; }

        [Required]
        public int goals { get; set; }

        [Required]
        public int assists { get; set; }

        [Required]
        public int keypasses { get; set; }

        [Required]
        public int keydribbles{ get; set; }

        [Required]
        public int clearances { get; set; }

        [Required]
        public int saves { get; set; }

        [Required]
        public int yellowcard { get; set; }

        [Required]
        public int redcard { get; set; }

        public virtual Match RelatedMatch { get; set; }

        public virtual ApplicationUser RelatedUser { get; set; }
    }
}
