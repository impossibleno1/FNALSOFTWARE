using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DIENMAYQUYETTIEN2.Models;
using System.Transactions;
using System.Net;
using System.Data.Entity;
using System.Data;
using System.Web.Security;
using System.Web.Configuration;

namespace DIENMAYQUYETTIEN2.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        DIENMAYQUYETTIENEntities1 db = new DIENMAYQUYETTIENEntities1();
        //
        // GET: /Admin/ProductAdmin/
        public ActionResult Index()
        {
            var product = db.Products.OrderByDescending(x => x.ID).ToList();
            return View(product);

        }
        //create
        [HttpGet]
        public ActionResult Create()
        {
            //ViewBag.ProductType = db.ProductTypes.OrderByDescending(x => x.ID).ToList();
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ID", "ProductTypeName");
            if (Session["Username"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product p)
        {
            
            CheckBangSanPham(p);
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    var pro = new Product();

                pro.ProductCode = p.ProductCode;
                pro.ProductName = p.ProductName;
                pro.ProductType = p.ProductType;
                pro.ProductTypeID = p.ProductTypeID;
                pro.OriginPrice = p.OriginPrice;
                pro.SalePrice = p.SalePrice;
                pro.Status = p.Status;
                pro.Quantity = p.Quantity;
                pro.InstallmentPrice = p.InstallmentPrice;
                pro.Avatar = p.Avatar;
                db.Products.Add(pro);
                db.SaveChanges();
                var path = Server.MapPath("~/App_Data");
                path = path + "/" + pro.ID;
                    if (Request.Files["Avatar"] != null && Request.Files["Avatar"].ContentLength > 0)
                    {
                        Request.Files["Avatar"].SaveAs(path);
                        scope.Complete();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Avatar", "Chưa có hình ảnh");
                    }
                }

            }
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ID", "ProductTypeName", p.ProductTypeID);
            return View();
        }
        public FileResult Details(int id)
        {
            var path = Server.MapPath("~/App_Data/" + id);
            return File(path, "images");
        }
        //Login
        public ActionResult Login()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult Login(Account acc)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using (DIENMAYQUYETTIENEntities db = new DIENMAYQUYETTIENEntities())
        //        {
        //            var obj = db.Accounts.Where(a => a.Username.Equals(acc.Username) && a.Password.Equals(acc.Password)).FirstOrDefault();

        //            if (obj != null)
        //            {
        //                Session["Username"] = obj.Username.ToString();
        //                Session["FullName"] = obj.FullName.ToString();
        //                Session["Usernameus"] = obj.Username.ToString();
        //                Session["FullNameus"] = obj.FullName.ToString();
        //                return RedirectToAction("ProductAdmin", "Admin");
        //            }
        //        }
        //    }
        //    return View(acc);
        //}
        ////Logout
        //[HttpPost]
        //public ActionResult Logout()
        //{   
        //    Session.Clear();
        //    Session.Abandon(); // it will clear the session at the end of request
        //    FormsAuthentication.SignOut();
        //    return RedirectToAction("Index");
        //}

        [HttpGet]
        public ActionResult Edit(int ID)
        {
                Product bangsanpham = db.Products.Find(ID);
                if (bangsanpham == null)
                {
                return HttpNotFound();
                }
            if (Session["Username"] != null)
            {
                //ViewBag.ProductType = db.ProductTypes.OrderByDescending(x => x.ID).ToList();
                ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ID", "ProductTypeName", bangsanpham.ProductTypeID);
                return View(bangsanpham);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            CheckBangSanPham(model);
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var path = Server.MapPath("~/App_Data");
                    path = path + "/" + model.ID;
                    if (Request.Files["HinhAnh"] != null && Request.Files["HinhAnh"].ContentLength > 0)
                    {
                        Request.Files["HinhAnh"].SaveAs(path);


                    }
                    scope.Complete();
                    return RedirectToAction("Index");

                }
            }
            //ViewBag.ProductType = db.ProductTypes.OrderByDescending(x => x.ID).ToList();
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ID", "ProductTypeName", model.ProductTypeID);
            return View(model);

        }

        private void CheckBangSanPham(Product model)
        {
            
            if (model.OriginPrice < 0)
            {
                ModelState.AddModelError("OriginPrice", "Giá bán phải lớn hơn 0");
            }
            if (model.SalePrice < 0)
            {
                ModelState.AddModelError("SalePrice", "Giá bán phải lớn hơn 0");
            }
            if (model.InstallmentPrice < 0)
            {
                ModelState.AddModelError("InstallmentPrice", "Giá bán phải lớn hơn 0");
            }
            if (model.Quantity < 0)
            {
                ModelState.AddModelError("Quantity", "Giá bán phải lớn hơn 0");
            }

            if (model.ProductName == null)
            {
                ModelState.AddModelError("ProductName", "Tên sản phẩm phải co ký tự");
            }
            
            if (model.ProductName.Length > 30)
            {
                ModelState.AddModelError("ProductName", "Tên sản phẩm phải lớn hơn 30 ký tự");
            }
            
            if (model.ProductName.Trim(' ')=="")
            {
                ModelState.AddModelError("ProductName", "Không được để trống");
            
            }
                
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product bangsanpham = db.Products.Find(id);
            if (bangsanpham == null)
            {
                return HttpNotFound();
            }
            if (Session["Username"] != null)
            {
                return View(bangsanpham);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product bangsanpham = db.Products.Find(id);
            db.Products.Remove(bangsanpham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

     
       
    }
}
