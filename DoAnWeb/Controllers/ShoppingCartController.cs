using DoAnCoSo.Models;
using DoAnCoSo.Models.EF;
using DoAnWeb.Models;
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
using System.Xml.Linq;

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
        public ActionResult CheckOutShip()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        public ActionResult Partial_CheckOut()
        {
            return PartialView();
        }
        
        public ActionResult Partial_Map()
        {
            return PartialView();
        }

        public ActionResult Partial_CheckOutShip()
        {
            return PartialView();
        }
        public ActionResult Partial_Item_ThanhToan()
        {
            // chua xu ly 
            try {
                double distance = Convert.ToDouble(Session["Distance"]);
            }
            catch { }
            //================
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.items.Any())
            {
                  return PartialView(cart.items);
            }

            return PartialView();
            //if (distance != null && distance > 0)
            //{
            //    ShoppingCart cart = (ShoppingCart)Session["Cart"];
            //    double delivery = 0;
            //    if (distance <= 5)
            //    {
            //        delivery = 15000;

            //    }
            //    else
            //    {
            //        delivery = (delivery - 5) * 3000 + 15000;
            //    }
            //    if (cart != null && cart.items.Any())
            //    {
            //        return PartialView(cart.items);
            //    }

            //    return PartialView();
            //}
        }
        public ActionResult UpdateDistancePartial()
        {
            try
            {
                double distance = Convert.ToDouble(Session["Distance"]);
            }
            catch { }

            ShoppingCart cart = (ShoppingCart)Session["Cart"];

            if (cart != null && cart.items.Any())
            {
                return PartialView("Partial_Item_ThanhToan", cart.items);
            }

            return PartialView("Partial_Item_ThanhToan");
        }

        [HttpPost]
        public ActionResult SaveDistanceToSession(double distance)
        {
            Session["Distance"] = distance;
            return Json(new { success = true });
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
        public ActionResult Partial_Item_Table_ThanhToan(DateTime? date)
        {
            ShoppingCart carttable = (ShoppingCart)Session["CartTable"];
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            decimal total = carttable.itemstable.Sum(x => x.Quantity);
            decimal totalPrice = 0;

            if (cart != null)
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
        public ActionResult CheckOutShipSuccess()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOutShip(OrderViewModel req)
        {
            
            var code = new { Success = false, Code = -1, Url=""};
            double delivery = 0;
            try
            {
                var distance = Convert.ToDouble(Session["Distance"]);
                if(distance > 0 && distance<= 5)
                {
                    delivery = 15000;
                }
                else if(distance>5) 
                {
                    delivery = (distance - 5) * 3000 + 15000;
                }

            }
            catch { }
            if (req !=null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                {
                    Order order = new Order();
                    order.customername = req.customername;
                    order.phone = req.phone;
                    order.address = req.address;
                    order.email = req.email;
                    order.datetime = req.datetime;
                    order.status = 1;
                    cart.items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                    {
                        productid = x.ProductId,
                        quantity = x.Quantity,
                        price = x.Price
                    }));
                    order.total = cart.items.Sum(x => (x.Price * x.Quantity)) + decimal.Parse(delivery.ToString());
                    order.typepayment = req.typepayment;
                    order.createddate = req.datetime;
                    order.modifierdate = DateTime.Now;
                    order.createdby = req.phone;
                    Random rd = new Random();
                    order.code = "DDB" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    order.ship = decimal.Parse(delivery.ToString());

                    db.Orders.Add(order);
                    Session.Remove("Distance");
                    db.SaveChanges();
                    //return Json("CheckOutSuccess");
                    //send mail cho khachs hang
                    var strSanPham = "";
                    var thanhtien = decimal.Zero;
                    var TongTien = decimal.Zero;

                    foreach (var sp in cart.items)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>" + sp.ProductName + "</td>";
                        strSanPham += "<td>" + sp.Quantity + "</td>";
                        strSanPham += "<td>" + DoAnCoSo.Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
                        strSanPham += "</tr>";
                        thanhtien += sp.Price * sp.Quantity;
                    }
                    code = new { Success = true, Code = req.typepayment, Url = "" };
                    //var url = "";
                    if (req.typepayment == 2)
                    {
                        var url = UrlPayment(req.typepaymentvn, order.code);
                        code = new { Success = true, Code = req.typepayment, Url = url };
                    }
                    TongTien = thanhtien + decimal.Parse(delivery.ToString());
                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                    contentCustomer = contentCustomer.Replace("{{MaDon}}", order.code);
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy hh:mm"));
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.customername);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.phone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", req.email);
                    contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.address);
                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}", DoAnCoSo.Common.Common.FormatNumber(thanhtien, 0));
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", DoAnCoSo.Common.Common.FormatNumber(TongTien, 0));
                    DoAnCoSo.Common.Common.SendMail("VietKichen", "Đơn đặt hàng#" + order.code, contentCustomer.ToString(), req.email);

                    string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                    contentAdmin = contentAdmin.Replace("{{MaDon}}", order.code);
                    contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                    contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order.customername);
                    contentAdmin = contentAdmin.Replace("{{Phone}}", order.phone);
                    contentAdmin = contentAdmin.Replace("{{Email}}", req.email);
                    contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", order.address);
                    contentAdmin = contentAdmin.Replace("{{ThanhTien}}", DoAnCoSo.Common.Common.FormatNumber(thanhtien, 0));
                    contentAdmin = contentAdmin.Replace("{{TongTien}}", DoAnCoSo.Common.Common.FormatNumber(TongTien, 0));
                    DoAnCoSo.Common.Common.SendMail("VietKichen", "Đơn đặt hàng mới #" + order.code, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);
                    cart.clearCart();
                    Session.Remove("Distance");

                    /*code = new { Success = true, Code = req.typepayment, Url = "" };
                    //var url = "";
                    if (req.typepayment == 2)
                    {
                        var url = UrlPayment(req.typepaymentvn, order.code);
                        code = new { Success = true, Code = req.typepayment, Url = url };
                    }*/
                    /*                    return Json("CheckOutSuccess");
                    */
                }
            }
            return Json(code);
        }
        public ActionResult VnpayReturn()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        var itemOrder = db.Orders.FirstOrDefault(x => x.code == orderCode);
                        if (itemOrder != null)
                        {
                            itemOrder.status = 2;//đã thanh toán
                            db.Orders.Attach(itemOrder);
                            db.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        //Thanh toan thanh cong
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //ViewBag.MaOrder = "Mã giao dịch thanh toán:" + code.ToString();
                    ViewBag.MaVNPAY = "Mã giao dịch tại VNPAY: " + vnpayTranId.ToString();
                    ViewBag.ThanhToanThanhCong = "số tiền thanh toán (VND): " + vnp_Amount.ToString()+"đ";
                    ViewBag.NganHang = "Ngân hàng thanh toán: " + bankCode;
                }
            }
            //var a = UrlPayment(0, "DH3574");
            return View();
        }

        public string UrlPayment(int typepaymentvn, string orderCode)
        {
            var urlPayment = "";
            var order = db.Orders.FirstOrDefault(x => x.code == orderCode);
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            var Price = (long)order.total * 100;
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", Price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (typepaymentvn == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (typepaymentvn == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (typepaymentvn == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", order.createddate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng :" + order.code);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.code); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return urlPayment;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
            var code = new { Success = false, Code = -1 };
            if (!ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                ShoppingCart carttable = (ShoppingCart)Session["CartTable"];
                var time = HttpContext.Session["TimeTable"] as DateTime?;
                //if (carttable != null)
                //{
                    Order order = new Order();
                    order.customername = req.customername;
                    order.phone = req.phone;
                    order.address = req.address;
                    order.email = req.email;
                    order.datetime = time.Value;

                    if (cart != null)
                    {
                        cart.items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                        {
                            productid = x.ProductId,
                            quantity = x.Quantity,
                            price = x.Price
                        }));
                        order.total = cart.items.Sum(x => (x.Price * x.Quantity) * (carttable.itemstable.Sum(y => y.Quantity)));
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
                    if (cart != null)
                    {
                        foreach (var sp in cart.items)
                        {
                            strSanPham += "<tr>";
                            strSanPham += "<td>" + sp.ProductName + "</td>";
                            strSanPham += "<td>" + sp.Quantity + "</td>";
                            strSanPham += "<td>" + DoAnCoSo.Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
                            strSanPham += "</tr>";
                            thanhtien += sp.Price * sp.Quantity * carttable.itemstable.Sum(x => x.Quantity);
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
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy hh:mm"));
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.customername);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.phone);
                    contentCustomer = contentCustomer.Replace("{{NgayDatBan}}", time.Value.ToString("dd/MM/yyyy hh:mm"));
                    contentCustomer = contentCustomer.Replace("{{Email}}", req.email);
                    contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.address);
                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}", DoAnCoSo.Common.Common.FormatNumber(thanhtien, 0));
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", DoAnCoSo.Common.Common.FormatNumber(TongTien, 0));
                    DoAnCoSo.Common.Common.SendMail("VietKichen", "Đơn đặt bàn #" + order.code, contentCustomer.ToString(), req.email);

                    string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                    contentAdmin = contentAdmin.Replace("{{MaDon}}", order.code);
                    contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                    contentAdmin = contentAdmin.Replace("{{Table}}", strTable);
                    contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order.customername);
                    contentAdmin = contentAdmin.Replace("{{Phone}}", order.phone);
                    contentAdmin = contentAdmin.Replace("{{NgayDatBan}}", order.datetime.ToString("dd/MM/yyyy hh:mm"));
                    contentAdmin = contentAdmin.Replace("{{Email}}", req.email);
                    contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", order.address);
                    contentAdmin = contentAdmin.Replace("{{ThanhTien}}", DoAnCoSo.Common.Common.FormatNumber(thanhtien, 0));
                    contentAdmin = contentAdmin.Replace("{{TongTien}}", DoAnCoSo.Common.Common.FormatNumber(TongTien, 0));
                    DoAnCoSo.Common.Common.SendMail("VietKichen", "Đơn đặt bàn mới #" + order.code, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);
                    if (cart != null)
                    {
                        cart.clearCart();
                    }
                    carttable.clearCart();
                    return Json("CheckOutSuccess");
                //}
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
                code = new { Success = true, msg = "Thêm món ăn vào giỏ hàng thành công!", code = 1, Count = cart.items.Count };
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult AddTableToCart(int id, int quantity, DateTime time)
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
                //DateTime dateTime = DateTime.Now;
                carttable.AddTableToCart(item, quantity);
                Session["CartTable"] = carttable;
                Session["TimeTable"] = time;
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