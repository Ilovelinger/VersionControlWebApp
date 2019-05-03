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

        [Required]
        public String matchType { get; set; }

        [Required]
        public DateTime dateTime { get; set; }

        [Required]
        public string location { get; set; }

        [Required]
        public String Penalty { get; set; }

        [Required]
        public String Overtime { get; set; }

        [Required]
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
