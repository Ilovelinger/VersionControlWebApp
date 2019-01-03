using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    /// <summary>
    /// This is the application post detail view model,with comments' content in the designated post.
    /// </summary>
    public class PostDetailViewModel
    {
        public Post Post { get; set; }

        public List<Comment> Comments { get; set; }

        public int PostID { get; set; }
        public string CommentsContent { get; set; }

        public PostDetailViewModel()
        {
            Comments = new List<Comment>();
        }
    }
}
