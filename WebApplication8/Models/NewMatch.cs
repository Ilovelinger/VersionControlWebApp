using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    public class NewMatch
    {
        [Key]
        [ScaffoldColumn(false)]
        public int newMatchId { get; set; }

        //[MinLength(2), MaxLength(20)]
        public String matchType { get; set; }
        public DateTime dateTime { get; set; }
        public string location { get; set; }
        public String Penalty { get; set; }
        public String Overtime { get; set; }
        public String matchDescription {get; set; }
    }

    public enum Penalty
    {
        Yes,
        No
    }

    public enum Overtime
    {
        Yes,
        No
    }
}
