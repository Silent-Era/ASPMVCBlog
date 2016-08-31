using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASPMVCBlog.Models;
using Microsoft.AspNet.Identity;

namespace ASPMVCBlog.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            var posts = db.Posts.Where(p => p.IsDeleted == false)
               .Include(p => p.Author).OrderByDescending(p => p.PostDate).ToList();
            return View(posts);
        }

        //// GET: Posts/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Post post = db.Posts.Find(id);
        //    if (post.IsDeleted)
        //        post = null;
        //    if (post == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(post);
        //}
        //public ActionResult RedirectFromComments(Mediator med)
        //{
        //    return RedirectToAction("ViewPost", med.PostId);
        //}
        public ActionResult ViewPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Post post = db.Posts.Include(p=>p.Author).Include(p=>p.Comments)
            //    .FirstOrDefault(p=>p.PostId==id);
           var post = db.Posts.Where(p => p.IsDeleted == false).Include(p=>p.Author)
                .FirstOrDefault(p => p.PostId == id);
            var comments = db.Comments.Where(c => c.post.PostId == id && c.IsDeleted==false).Include(c => c.Author).ToList();
            if (post == null)
            {
                return HttpNotFound();
            }
            post.Comments = comments;
            return View(post);
        }
        // GET: Posts/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,PostTitle,PostBody,PostDate")] Post post)
        {
             post.Author = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            
            var UserId = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Where(p=>p.IsDeleted!=true)
                .Include(p=>p.Author).FirstOrDefault(p=>p.PostId==(int)id);

            if (post == null)
            {
                return HttpNotFound();
            }
            if (!User.IsInRole("Administrators"))
            {
                if (post.Author.Id != UserId)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,PostTitle,PostBody,PostDate,IsDeleted")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        
        public ActionResult Delete(int? id)
        {
            var UserId = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Where(p=>p.IsDeleted != true)
                .Include(p => p.Author).FirstOrDefault(p => p.PostId == (int)id);
            if (post == null)
            {
                return HttpNotFound();
            }
            if (!User.IsInRole("Administrators"))
            {
                if (post.Author.Id != UserId)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            post.IsDeleted = true;
            //db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
