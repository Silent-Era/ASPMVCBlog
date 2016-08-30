using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASPMVCBlog.Models
{
    public class Comment
    {
        public Comment()
        {
            this.CommentDate = DateTime.Now;
            this.IsDeleted = false;
        }
        [Key]
        public int CommentId { get; set; }

        [Required]
        public string CommentBody { get; set; }

        [Required]
        public DateTime CommentDate { get; set; }

        public ApplicationUser Author { get; set; }
        [Required]
        public Post post { get; set; }

        public bool IsDeleted { get; set; }
    }
}