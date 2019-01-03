using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    /// <summary>
    /// This is the application post model with post attributes.
    /// </summary>
    public class Post
    {
        [Key]
        [ScaffoldColumn(false)]
        public int postid { get; set; }

        [MinLength(2), MaxLength(20)]
        public String title { get; set; }
        public String author { get; set; }

        [MaxLength(500)]
        public String content { get; set; }

    }
}
