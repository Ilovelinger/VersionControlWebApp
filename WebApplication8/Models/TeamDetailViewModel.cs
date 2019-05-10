using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    /// <summary>
    /// Team detail view model
    /// </summary>
    public class TeamDetailViewModel
    {
        public Team Team { get; set; }

        public int TeamID { get; set; }

        public String TeamName { get; set; }

        public List<RegisteredUser> RegisteredUsers { get; set; }
    }
}
