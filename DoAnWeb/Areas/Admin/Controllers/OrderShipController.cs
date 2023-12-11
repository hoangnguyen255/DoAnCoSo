using DoAnCoSo.Models;
using DoAnCoSo.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCoSo.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderShipController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index(string SearchText, int? page)
        {
            IEnumerable<Order> items = db.Orders.Where(x => x.ship > 0).OrderByDescending(x => x.createddate).ToList();
            if (!string.IsNullOrEmpty(SearchText))
            {
                string searchTextLower = SearchText.ToLowerInvariant();  // Convert the search text to lowercase

                items = items.Where(x => x.code.ToLowerInvariant().Contains(searchTextLower) ||
                                          x.customername.ToLowerInvariant().Contains(searchTextLower) || x.phone.Contains(searchTextLower));
                //items = items.Where(x => x.code.Contains(SearchText) || x.customername.Contains(SearchText));
            }
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            return View(items.ToPagedList(pageNumber, pageSize));
        }  
        public ActionResult View(int id)
        {
            var item = db.Orders.Find(id);
            return View(item);
        }

        public ActionResult Partial_SanPham(int id)
        {
            var items = db.OrderDetails.Where(x => x.orderid == id).ToList();
            return PartialView(items);
        }

        [HttpPost]
        public ActionResult UpdateTT(int id, int trangthai)
        {
            var item = db.Orders.Find(id);
            if (item != null)
            {
                db.Orders.Attach(item);
                item.typepayment = trangthai;
                db.Entry(item).Property(x => x.typepayment).IsModified = true;
                db.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "Unsuccess", Success = false });
        }
    }
}