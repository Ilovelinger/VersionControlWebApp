using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    public class PlayerPerformance
    {
        [Key]
        [ScaffoldColumn(false)]
        public int id { get; set; }

        //[MinLength(2), MaxLength(20)]
        public string linkedTeamname { get; set; }
        public string linkedplayername { get; set; }
        public String startup { get; set; }
        public String substitute { get; set; }
        public int goals { get; set; }
        public int assists { get; set; }
        public int keypasses { get; set; }
        public int keydribbles{ get; set; }
        public int clearances { get; set; }
        public int saves { get; set; }
        public int yellowcard { get; set; }
        public int redcard { get; set; }

        public virtual Match RelatedMatch { get; set; }

        public virtual ApplicationUser RelatedUser { get; set; }
    }
}
