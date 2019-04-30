using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    public class TeamDetailViewModel
    {
        public Team Team { get; set; }

        public int TeamID { get; set; }

        public String TeamName { get; set; }

        public List<RegisteredUser> RegisteredUsers { get; set; }
    }
}
