using DoAnCoSo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCoSo.Controllers
{
    public class TablesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Product
        public ActionResult Index()
        {
            var items = db.Tables.ToList();
            return View(items);
        }
        public ActionResult Detail(string alias, int id)
        {
            var item = db.Tables.Find(id);
            return View(item);
        }

        public ActionResult Space(string alias, int id)
        {
            var items = db.Tables.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.spaceid == id).ToList();
            }
            var cate = db.Spaces.Find(id);
            if(cate != null)
            {
                ViewBag.CateName = cate.title;
            }
            ViewBag.CateId = id;
            return View(items);
        }
      
    }

}