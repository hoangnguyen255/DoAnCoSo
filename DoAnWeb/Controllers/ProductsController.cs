﻿using DoAnCoSo.Models;
using DoAnCoSo.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCoSo.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Product
       /* public ActionResult Index()
        {
            var items = db.Products.ToList();
            return View(items);
        }*/
        public ActionResult Index(string SearchText)
        {
            IEnumerable<Product> items = db.Products.OrderByDescending(x => x.id).Where(x => x.isactive == true).ToList();
            if (!string.IsNullOrEmpty(SearchText))
            {
                items = items.Where(x => x.alias.Contains(SearchText) || x.title.Contains(SearchText));
            }
            return View(items);

        }
        public ActionResult Detail(string alias, int id)
        {
            var item = db.Products.Find(id);
            return View(item);
        }

        public ActionResult ProductCategory(string alias, int id)
        {
            var items = db.Products.Where(x => x.isactive == true).ToList();
            if (id > 0)
            {
                items = items.Where(x => x.productcategoryid == id).ToList();
            }
            var cate = db.ProductCategories.Find(id);
            if(cate != null)
            {
                ViewBag.CateName = cate.title;
            }
            ViewBag.CateId = id;
            return View(items);
        }
        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Products.Where(x => x.ishome && x.isactive).Take(20).ToList();
            return PartialView(items);
        }
        public ActionResult Partial_ProductIsBestSeller()
        {
            var items = db.Products.Where(x => x.issale && x.isactive).Take(12).ToList();
            return PartialView(items);
        }
    }

}