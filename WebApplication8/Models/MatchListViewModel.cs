using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    /// <summary>
    /// This is the application post list view model
    /// </summary>
    public class MatchListViewModel
    {
        public int NumberOfMatches { get; set; }
        //Create a list of posts.
        public List<Match> Matches { get; set; }

    }
}
