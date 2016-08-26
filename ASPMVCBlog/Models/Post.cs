using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASPMVCBlog.Models
{
    public class Post
    {
        public Post()
        {
            this.IsDeleted = false;
            this.PostDate = DateTime.Now;
        }
        [Key]
        public int PostId { get; set; }

        [Required]
        [StringLength(200,ErrorMessage ="The title of the post can be no longer than 200 symbols. :(")]
        public string PostTitle{ get; set; }

        [Required]
        public string PostBody { get; set; }

        [Required]
        public ApplicationUser Author{ get; set; }

        [Required]
        public DateTime PostDate { get; set; }

        public bool IsDeleted { get; set; }

    }
}