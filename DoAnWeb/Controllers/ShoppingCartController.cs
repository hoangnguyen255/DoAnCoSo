﻿using DoAnCoSo.Models;
using DoAnCoSo.Models.EF;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.Security.Provider;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Migrations.Sql;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace DoAnCoSo.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            ShoppingCart carttable = (ShoppingCart)Session["CartTable"];

            if (carttable != null && carttable.itemstable.Any())
            {
                ViewBag.CheckCartTable = carttable;
            }
            return View();
        }
        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.items.Any())
            {
                ViewBag.CheckCart = cart;
            }
          
            ShoppingCart carttable = (ShoppingCart)Session["CartTable"];
            if (carttable != null && carttable.itemstable.Any())
            {
                ViewBag.CheckCartTable = carttable;
            }
            return View();
        }
        public ActionResult Partial_CheckOut()
        {
            return PartialView();
        }
        public ActionResult Partial_Item_ThanhToan()
        {
           
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.items.Any())
            {
                return PartialView(cart.items);
            }

            return PartialView();
        }
        /*   public ActionResult Partial_Item_Table_ThanhToan()
           {

               ShoppingCart carttable = (ShoppingCart)Session["CartTable"];
               if (carttable != null && carttable.itemstable.Any())
               {
                   return PartialView(carttable.itemstable);
               }

               return PartialView();
           }*/
        /*        public ActionResult Partial_Item_Table_ThanhToan()
                {
                    ShoppingCart cart = (ShoppingCart)Session["Cart"];
                    decimal total = cart.GetTotalPrice();

                    ShoppingCart carttable = (ShoppingCart)Session["CartTable"];

                    decimal totalPrice = total * carttable.items.Sum(x => x.Quantity);
                    if (carttable != null)
                    {
                        return PartialView(new ShoppingCartNew { Tables = carttable.itemstable, Total = total });
                    }
                    return PartialView();

                }*/
        public ActionResult Partial_Item_Table_ThanhToan()
        {
            ShoppingCart carttable = (ShoppingCart)Session["CartTable"];
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            decimal total = carttable.itemstable.Sum(x => x.Quantity);
            decimal totalPrice = 0;

            if(cart != null)
            {
                totalPrice = total * cart.items.Sum(x => x.Quantity * x.Price);
            }
            //decimal totalPrice = total * cart.items.Sum(x => x.Quantity * x.Price);
            if (carttable != null)
            {
                return PartialView(new ShoppingCartNew { Tables = carttable.itemstable, Total = totalPrice });
            }
            return PartialView();

        }
        public ActionResult Partial_Item_cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.items.Any())
            {
                return PartialView(cart.items);
            }
            return PartialView();
        }
        public ActionResult Partial_Item_Table_cart()
        {
            ShoppingCart carttable = (ShoppingCart)Session["CartTable"];
            if (carttable != null && carttable.itemstable.Any())
            {
                return PartialView(carttable.itemstable);
            }
            return PartialView();
        }
        public ActionResult CheckOutSuccess()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
            var code = new { Success = false, Code = -1 };
            if (ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                ShoppingCart carttable = (ShoppingCart)Session["CartTable"];
                
                if (carttable !=null )
                {
                    Order order = new Order();
                    order.customername = req.customername;
                    order.phone = req.phone;
                    order.address = req.address;
                    order.email = req.email;
                    order.datetime = req.datetime;

                    if(cart != null)
                    {
                        cart.items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                        {
                            productid = x.ProductId,
                            quantity = x.Quantity,
                            price = x.Price
                        }));
                        order.total = cart.items.Sum(x => (x.Price * x.Quantity) *(carttable.itemstable.Sum(y=>y.Quantity)));
                        order.typepayment = req.typepayment;
                        order.createddate = DateTime.Now;
                        order.modifierdate = DateTime.Now;
                        order.createdby = req.phone;
                        Random rd = new Random();
                        order.code = "DDB" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    }

                    carttable.itemstable.ForEach(x => order.OrderDetails.Add(new OrderDetail
                    {
                        //productid = x.TableId,
                        tableid = x.TableId,
                        quantity = x.Quantity,
                    })); 

                   /* order.total = total;// cart.items.Sum(x => (x.Price * x.Quantity));
                    order.typepayment = req.typepayment;
                    order.createddate = DateTime.Now;
                    order.modifierdate = DateTime.Now;
                    order.createdby = req.phone;
                    Random rd = new Random();
                    order.code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);*/
                    //order.E = req.CustomerName;
                    db.Orders.Add(order);
                    db.SaveChanges();
                    //return Json("CheckOutSuccess");
                    //send mail cho khachs hang
                    var strSanPham = "";
                    var thanhtien = decimal.Zero;
                    var TongTien = decimal.Zero;
                    if(cart !=null)
                    {
                        foreach (var sp in cart.items)
                        {
                            strSanPham += "<tr>";
                            strSanPham += "<td>" + sp.ProductName + "</td>";
                            strSanPham += "<td>" + sp.Quantity + "</td>";
                            strSanPham += "<td>" + DoAnCoSo.Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
                            strSanPham += "</tr>";
                            thanhtien += sp.Price * sp.Quantity *carttable.itemstable.Sum(x=>x.Quantity);
                        }
                    }

                    var strTable = "";
                    foreach (var sp in carttable.itemstable)
                    {
                        strTable += "<tr>";
                        strTable += "<td>" + sp.TableName + "</td>";
                        strTable += "<td>" + sp.Quantity + "</td>";
                        strTable += "</tr>";
                    }

                    TongTien = thanhtien;
                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                    contentCustomer = contentCustomer.Replace("{{MaDon}}", order.code);
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{Table}}", strTable);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.customername);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.phone);
                    contentCustomer = contentCustomer.Replace("{{NgayDatBan}}", req.datetime.ToString("dd/MM/yyyy hh:mm"));
                    contentCustomer = contentCustomer.Replace("{{Email}}", req.email);
                    contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.address);
                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}", DoAnCoSo.Common.Common.FormatNumber(thanhtien, 0));
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", DoAnCoSo.Common.Common.FormatNumber(TongTien, 0));
                    DoAnCoSo.Common.Common.SendMail("GiaDungViet", "Đơn hàng #" + order.code, contentCustomer.ToString(), req.email); 
                    
                    string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                    contentAdmin = contentAdmin.Replace("{{MaDon}}", order.code);
                    contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                    contentAdmin = contentAdmin.Replace("{{Table}}", strTable);
                    contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order.customername);
                    contentAdmin = contentAdmin.Replace("{{Phone}}", order.phone);
                    contentAdmin = contentAdmin.Replace("{{Email}}", req.email);
                    contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", order.address);
                    contentAdmin = contentAdmin.Replace("{{ThanhTien}}", DoAnCoSo.Common.Common.FormatNumber(thanhtien, 0));
                    contentAdmin = contentAdmin.Replace("{{TongTien}}", DoAnCoSo.Common.Common.FormatNumber(TongTien, 0));
                    DoAnCoSo.Common.Common.SendMail("GiaDungViet", "Đơn hàng mới #" + order.code, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);
                    if(cart != null)
                    {
                        cart.clearCart();
                    }
                    carttable.clearCart();
                    return Json("CheckOutSuccess");
                }        
            }

            return Json(code);
        }
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return Json(new { Count = cart.items.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            var db = new ApplicationDbContext();
            var checkProduct = db.Products.FirstOrDefault(x => x.id == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.id,
                    ProductName = checkProduct.title,
                    CategoryName = checkProduct.ProductCategory.title,
                    Alias = checkProduct.alias,
                    Quantity = quantity
                };
                if (checkProduct.ProductImage.FirstOrDefault(x => x.isdefault) != null)
                {
                    item.ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.isdefault).image;
                }
                item.Price = checkProduct.price;
                if (checkProduct.pricesale > 0)
                {
                    item.Price = (decimal)checkProduct.pricesale;
                }
                item.TotalPrice = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code = new { Success = true, msg = "Thêm sản phẩm vào giỏ hàng thành công!", code = 1, Count = cart.items.Count };
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult AddTableToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1 };
            var db = new ApplicationDbContext();
            var checkTable = db.Tables.FirstOrDefault(x => x.id == id);
            if (checkTable != null)
            {
                ShoppingCart carttable = (ShoppingCart)Session["CartTable"];
                if (carttable == null)
                {
                    carttable = new ShoppingCart();
                }
                ShoppingCartTableItem item = new ShoppingCartTableItem
                {
                    TableId = checkTable.id,
                    TableName = checkTable.title,
                    SpaceName = checkTable.Space.title,
                    Alias = checkTable.alias,
                    Quantity = quantity
                };
                if (checkTable.TableImages.FirstOrDefault(x => x.isdefault) != null)
                {
                    item.TableImg = checkTable.TableImages.FirstOrDefault(x => x.isdefault).image;
                }

                carttable.AddTableToCart(item, quantity);
                Session["CartTable"] = carttable;
                code = new { Success = true, msg = "Thêm bàn vào giỏ hàng thành công!", code = 1 };
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                var checkProduct = cart.items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = 1, Count = cart.items.Count };
                }
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult DeleteTable(int id)
        {
            var code = new { Success = false, msg = "", code = -1 };
            ShoppingCart carttable = (ShoppingCart)Session["CartTable"];
            if (carttable != null)
            {
                var checkProduct = carttable.itemstable.FirstOrDefault(x => x.TableId == id);
                if (checkProduct != null)
                {
                    carttable.RemoveTable(id);
                    code = new { Success = true, msg = "", code = 1 };
                }
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { Success = true });
            }
            return Json(new { Success = false, msg = "Cập nhật số lượng bàn thành công!" });
        }    

        [HttpPost]
        public ActionResult UpdateTable(int id, int quantity)
        {
            ShoppingCart carttable = (ShoppingCart)Session["CartTable"];
            if (carttable != null)
            {
                carttable.UpdateQuantity(id, quantity);
                return Json(new { Success = true });
            }
            return Json(new { Success = false, msg = "Cập nhật số lượng bàn thành công!" });
        }
    }
}