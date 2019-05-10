using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    /// <summary>
    /// Match result model with attributes.
    /// </summary>
    public class Match
    {
        [Key]
        [ScaffoldColumn(false)]
        public int matchid { get; set; }

        //[MinLength(2), MaxLength(20)]
        [Required]
        public String team1Name { get; set; }

        [Required]
        public String team2Name { get; set; }

        [Required]
        public String location { get; set; }

        [Required]
        public int team1Score { get; set; }

        [Required]
        public int team2Score { get; set; }

        [Required]
        public DateTime dateTime { get; set; }

        [Required]
        public String penalty { get; set; }

        [Required]
        public String overtime {get;set;}

        [Required]
        public int team1PenaltyScore { get; set; }

        [Required]
        public int team2PenaltyScore { get; set; }

        public virtual Team RelatedTeam1 { get; set; }
        public virtual Team RelatedTeam2 { get; set; }
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
