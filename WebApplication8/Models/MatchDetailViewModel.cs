using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    /// <summary>
    /// This is the application post detail view model,with comments' content in the designated post.
    /// </summary>
    public class MatchDetailViewModel
    {
        public Match Match { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public List<Comment> Comments { get; set; }

        public int MatchID { get; set; }
        public string CommentsContent { get; set; }

        public string commentUsername { get; set; }


        public MatchDetailViewModel()
        {
            Comments = new List<Comment>();
        }
    }
}
