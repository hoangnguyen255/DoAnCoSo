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
    public class EmployeeController : Controller
    {
        // GET: Admin/Employee
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(string SearchText, int? page)

        {
            IEnumerable<Employee> items = db.Employees.OrderByDescending(x => x.id);
            if (!string.IsNullOrEmpty(SearchText))
            {
                items = items.Where(x => x.name.Contains(SearchText));
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
        public ActionResult add()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult add(Employee model)
        {
            if (ModelState.IsValid)
            {
                model.createddate = DateTime.Now;
                model.modifierdate = DateTime.Now;
                db.Employees.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Edit(int id)

        {
           /* ViewBag.gioitinh = new SelectList(db.Employees.ToList(), "Nam", "Nam", "Nữ", "Nữ") ;*/
            var item = db.Employees.Find(id);

            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee model)
        {
            if (ModelState.IsValid)
            {
                model.modifierdate = DateTime.Now;
                db.Employees.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Employees.Find(id);
            if (item != null)
            {
                db.Employees.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}