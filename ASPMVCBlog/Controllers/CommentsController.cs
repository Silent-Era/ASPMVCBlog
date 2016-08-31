using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPMVCBlog.Models;
using Microsoft.AspNet.Identity;

namespace ASPMVCBlog.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult CreateComment(int PostId)
        {
            return PartialView("CreateComment",new Comment() { PostId=PostId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include ="PostId,CommentBody,AuthorStr")] Comment comment)
        {
            if (User.Identity.IsAuthenticated)
            {
                comment.AuthorId = User.Identity.GetUserId();
                comment.Author = db.Users.FirstOrDefault(u => u.Id == comment.AuthorId);
            }
            int id = comment.PostId;
            comment.post = db.Posts.Where(p => p.IsDeleted == false).FirstOrDefault(p => p.PostId == comment.PostId);
            
            //if(comment.Author==null && comment.AuthorStr == null)
            //{
            //    comment.AuthorStr = "Anonymous";
            //}
            if(ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("ViewPost","Posts", new { id = id });
            }
            return View();
        }
    }
}