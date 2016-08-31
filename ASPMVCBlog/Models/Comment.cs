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
            this.AuthorStr = "Anonymous";
        }
        [Key]
        public int CommentId { get; set; }

       
        public int PostId { get; set; }

        public string AuthorId { get; set; }
        [Required]
        public string CommentBody { get; set; }

        [Required]
        public DateTime CommentDate { get; set; }

        public ApplicationUser Author { get; set; }

        [StringLength(20,ErrorMessage ="Name too long. Sorry :(")]
        public string AuthorStr { get; set; }

       
        public Post post { get; set; }

        public bool IsDeleted { get; set; }
    }
}