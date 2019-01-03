using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    /// <summary>
    /// This is the application post list view model
    /// </summary>
    public class PostListViewModel
    {
        public int NumberOfPosts { get; set; }
        //Create a list of posts.
        public List<Post> Posts { get; set; }

    }
}
