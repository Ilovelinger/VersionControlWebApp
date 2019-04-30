using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    public class RegisteredUser
    {
        [Key]
        [ScaffoldColumn(false)]
        public int RegisteredUserId { get; set; }

        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }
        public int MobilePhoneNumber { get; set; }
        public int KitNumber { get; set; }
        public string Position { get; set; }

        public string FullName { get; set; }

        public byte[] AvatarImage { get; set; }

        public virtual Team RelatedTeam { get; set; }

        public string RelatedTeamName { get; set; }

        public string isRegistered { get; set; }
    }
}
