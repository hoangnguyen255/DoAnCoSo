using DoAnCoSo.Models.EF;
using DoAnCoSo.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCoSo.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class CommentController : Controller
    {
        // GET: Admin/Comment
   
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/News
      /*  public ActionResult Index()
        {
            var items = db.Comments.ToList();
            return View(items);
        }*/
        public ActionResult Index(string SearchText, int? page)
        {
            IEnumerable<Comment> items = db.Comments.OrderByDescending(x => x.id);
            if (!string.IsNullOrEmpty(SearchText))
            {
                items = items.Where(x => x.message.Contains(SearchText) || x.name.Contains(SearchText));
            }
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            return View(items);
        }
        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.Comments.Find(id);
            if (item != null)
            {
                item.isactive = !item.isactive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isactive = item.isactive });
            }
            return Json(new { success = false });
        }
    }
}