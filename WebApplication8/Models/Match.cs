using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{

    public class Match
    {
        [Key]
        [ScaffoldColumn(false)]
        public int matchid { get; set; }

        //[MinLength(2), MaxLength(20)]
        public String team1Name { get; set; }
        public String team2Name { get; set; }
        public String location { get; set; }
        public int team1Score { get; set; }
        public int team2Score { get; set; }
        public DateTime dateTime { get; set; }
        public String penalty { get; set; }
        public String overtime {get;set;}
        public int team1PenaltyScore { get; set; }
        public int team2PenaltyScore { get; set; }

    }

    public enum penalty
    {
        Yes,
        No
    }

    public enum overtime
    {
        Yes,
        No
    }
}
