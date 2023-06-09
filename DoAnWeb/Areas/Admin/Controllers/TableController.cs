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
    [Authorize(Roles = "Admin,Employee")]

    public class TableController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Product
        public ActionResult Index(string SearchText,int? page)
        {
            var pageSize = 10;
            IEnumerable<Table> items = db.Tables.OrderByDescending(x => x.id);
            if (!string.IsNullOrEmpty(SearchText))
            {
                items = items.Where(x => x.alias.Contains(SearchText) || x.title.Contains(SearchText));
            }
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
        public ActionResult Add() 
        {
            ViewBag.Space=new SelectList(db.Spaces.ToList(),"id","title");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Add(Table model,List<String> Images,List<int> rDefault)
        {
            if(ModelState.IsValid)
            {
                if(Images!=null && Images.Count > 0)
                {
                    for(int i = 0;i<Images.Count;i++)
                    {
                        if (i + 1 == rDefault[0])
                        {
                            model.image = Images[i];
                            model.TableImages.Add(new TableImage()
                            {
                                tableid = model.id,
                                image = Images[i],
                                isdefault = true
                            });                          
                        }
                        else
                        {
                            model.TableImages.Add(new TableImage()
                            {
                                tableid = model.id,
                                image = Images[i],
                                isdefault = true
                            });
                        }
                    }
                }
                model.createddate = DateTime.Now;
                
                model.modifierdate = DateTime.Now;
                if (string.IsNullOrEmpty(model.alias))
                {
                    model.alias = DoAnCoSo.Models.Common.Filter.FilterChar(model.title);
                }
                //model.alias = DoAnCoSo.Models.Common.Filter.FilterChar(model.title);
                if (string.IsNullOrEmpty(model.seotitle))
                {
                    model.seotitle = model.title;
                }
                db.Tables.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Space = new SelectList(db.Spaces.ToList(), "id", "title");
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            ViewBag.Space = new SelectList(db.Spaces.ToList(), "id", "title");
            var item = db.Tables.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Table model)
        {
            if (ModelState.IsValid)
            {
                model.modifierdate = DateTime.Now;
                model.alias = DoAnCoSo.Models.Common.Filter.FilterChar(model.title);
                db.Tables.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Tables.Find(id);
            if (item != null)
            {
                db.Tables.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}