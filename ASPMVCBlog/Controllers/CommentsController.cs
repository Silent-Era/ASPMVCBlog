using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPMVCBlog.Models;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Data.Entity;

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
        public ActionResult DeleteComment(int id,int PostId)
        {
            
            Comment comment = db.Comments.Find(id);
            if (comment.IsDeleted)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!User.IsInRole("Administrators"))
            {
                if (User.Identity.GetUserId() != comment.AuthorId)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            comment.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("ViewPost","Posts", new { id = PostId });
        }
        public ActionResult EditComment(int id,int PostId)
        {
            Comment comment = db.Comments.Find(id);
            if (comment.IsDeleted)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(!User.IsInRole("Administrators"))
            {
                if (User.Identity.GetUserId() != comment.AuthorId)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            return PartialView("EditComment", new Comment() { CommentId = id
                                ,CommentBody=comment.CommentBody});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditComment([Bind(Include = "CommentBody,CommentId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                var com= db.Comments.Find(comment.CommentId);
                com.CommentBody = comment.CommentBody;
                db.SaveChanges();
                return RedirectToAction("ViewPost","Posts",new { id=comment.PostId});
            }
            return View();
        }
    }
}