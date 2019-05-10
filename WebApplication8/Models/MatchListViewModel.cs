using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    /// <summary>
    /// Application post list view model
    /// </summary>
    public class MatchListViewModel
    {
        public int NumberOfMatches { get; set; }

        public List<Match> Matches { get; set; }

    }
}
