using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    /// <summary>
    /// This is the application comment model with comment attributes.
    /// </summary>
    public class Comment
    {
        [Key]
        [ScaffoldColumn(false)]
        public int commentid { get; set; }

        [MinLength(5), MaxLength(200)]
        public String commentscontent { get; set; }

        //Create foreign key realated to post model.
        public virtual Post RelatedPost { get; set; }

    }
}
