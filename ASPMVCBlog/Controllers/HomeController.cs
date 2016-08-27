using ASPMVCBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ASPMVCBlog.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var posts = new List<Post>();
            if (db.Posts.Count()> 0)
            {
               posts = db.Posts.Include(p => p.Author)
                    .Where(p=>p.IsDeleted != true).OrderByDescending(p => p.PostDate)
                   .ToList();
            }
            return View(posts);
        }

       //public List<Post> ShowPosts(int counter)
       //{
       //     ApplicationDbContext db = new ApplicationDbContext();
       //    var PostsList = new List<Post>();
       //    if (db.Posts.Count() > 0)
       //    {
       //         PostsList = db.Posts.Include(p => p.Author)
       //             .Where(p=>p.IsDeleted != true).OrderByDescending(p => p.PostDate)
       //            .ToList();
       //         var posts = new List<Post>();
       //         if (counter > PostsList.Count && PostsList.Count >=2)
       //             counter = PostsList.Count;
       //         for(int i = counter - 2; i <= counter; i++)
       //         {
       //             posts.Add(PostsList.ElementAt(i));
       //         }
       //         return posts;
       //    }

       //    return PostsList;
       //}
    }
}
