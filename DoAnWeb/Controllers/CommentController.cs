using DoAnCoSo.Models.EF;
using DoAnCoSo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCoSo.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Partial_Comment()
        {
            var items = db.Comments.Where(x => x.isactive).Take(20).ToList();
            
            return PartialView(items );
        }
        public ActionResult Partial_AddComment()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(Comment comment)
        {
            var code = new { Success = false, Code = -1 };
            if (ModelState.IsValid)
            {
                comment.createddate = DateTime.Now;
                comment.modifierdate = DateTime.Now;
                db.Comments.Add(comment);
                db.SaveChanges();
/*                return RedirectToAction("Index");
*/            }

            return Json(code);
        }

    }
}