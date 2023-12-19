using DoAnCoSo.Models;
using DoAnCoSo.Models.EF;
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
        public ActionResult Index2()
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

        public ActionResult Partial_View_Table(DateTime ?date, int ?id)
        {
            if (id != null)
            {
                Session["Table"] = id;
            }
            var _id = (int)Session["Table"];
            var cate = db.Spaces.Find(_id);
            var lstable = new List< TableViewModel>();
            var listtables = db.Tables.ToList();
            var listorderdetail = new List<OrderDetail>();
            if (_id != null)
            {
                listtables = listtables.Where(x => x.spaceid == _id).ToList();
            }
            if(date != null)
            {
                var dateMinus5Hours = date.Value.AddHours(-4);
                var datePlus5Hours = date.Value.AddHours(4);
                listorderdetail = db.OrderDetails.Where(x => x.Order.datetime >= dateMinus5Hours && x.Order.datetime <= datePlus5Hours).ToList();
            }       
            foreach (var table in listtables)
            {
                int t = 0;
                foreach (var orderdetail in listorderdetail)
                {
                    if (table.id == orderdetail.tableid)
                    {
                        t = 1;
                    }                 
                }
                if(t == 1)
                {
                    lstable.Add(new TableViewModel { table = table, isActive = false });
                }
                else
                {
                    lstable.Add(new TableViewModel { table = table, isActive = true });
                }
            }
            return PartialView(lstable);
        }
    }
}