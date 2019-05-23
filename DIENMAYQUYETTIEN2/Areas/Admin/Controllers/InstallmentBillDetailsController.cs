using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DIENMAYQUYETTIEN2.Models;

namespace DIENMAYQUYETTIEN2.Areas.Admin.Controllers
{
    public class InstallmentBillDetailsController : Controller
    {
        private DIENMAYQUYETTIENEntities1 db = new DIENMAYQUYETTIENEntities1(); 

        // GET: Admin/InstallmentBillDetails
        public ActionResult Index()
        {
            if (Session["insctcashBill"] == null)
        {
                Session["insctcashBill"] = new List<InstallmentBillDetail>();
            }
            return PartialView(Session["insctcashBill"]);
        }
        // GET: Admin/InstallmentBillDetails/Create
        public ActionResult Create()
            {
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName");
            var model = new InstallmentBillDetail();
            model.ID = 0;
            model.Quantity = 1;
            return PartialView(model);
            }

        public int InstallmentPrice(int ProductID)
        {
            return db.Products.Find(ProductID).InstallmentPrice;
        }

        // POST: Admin/InstallmentBillDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2(InstallmentBillDetail model)
        {
            if (ModelState.IsValid)
            {
                model.ID = Environment.TickCount;
                model.Product = db.Products.Find(model.ProductID);
                var insctcashBill = Session["insctcashBill"] as List<InstallmentBillDetail>;
                if (insctcashBill == null)
                    insctcashBill = new List<InstallmentBillDetail>();
                insctcashBill.Add(model);
                Session["insctcashBill"] = insctcashBill;
                return RedirectToAction("Create", "InstallmentBills");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName", model.ProductID);
            return View("Create", model);
        }
        public PartialViewResult Editviewi()
        {

            var a = (Int32)Session["idi"];
            ViewBag.id = a;
            var query = db.InstallmentBillDetails.Where(cbd => cbd.BillID == a).ToList();
            var b = query as List<InstallmentBillDetail>;
            var sum = 0;
            foreach (var chiTiet in b)
            {
                sum += (chiTiet.Quantity * chiTiet.InstallmentPrice);
            }
            Session["Tonggia"] = sum;
            return PartialView(query);
        }
        public ActionResult CreateEditI(int id)
        {
            var a = (Int32)Session["idi"];
            ViewBag.id = a;
            InstallmentBillDetail inscashbilldetail = db.InstallmentBillDetails.Find(id);

            //var query = db.CashBillDetails.Where(cbd => cbd.ID == id).ToList();
            //var b = query as List<CashBillDetail>;
            //var idpd = 0 ;
            //foreach (var chiTiet in b)
            //{
            //     idpd = chiTiet.ProductID;
            //}
            //var query2 = db.Products.Where(cbd => cbd.ID == idpd).ToList();
            //var c = query2 as List<Product>;
            //var orgprice = 0;
            //foreach (var chiTiet in c)
            //{
            //    orgprice = chiTiet.OriginPrice;
            //}


            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName");

            var model = new InstallmentBillDetail();
            model.BillID = id;
            model.Quantity = 1;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEditI(InstallmentBillDetail ibilldetail)
        {


            var a = (Int32)Session["idi"];

            if (ModelState.IsValid)
            {

                db.InstallmentBillDetails.Add(ibilldetail);
                db.SaveChanges();
                return RedirectToAction("Edit", "InstallmentBills", new { id = a });

            }

            ViewBag.BillID = new SelectList(db.CashBills, "ID", "BillCode", ibilldetail.BillID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductCode", ibilldetail.ProductID);
            return View(ibilldetail);
        }

        // GET: Admin/InstallmentBillDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstallmentBillDetail installmentBillDetail = db.InstallmentBillDetails.Find(id);
            if (installmentBillDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.BillID = new SelectList(db.InstallmentBills, "ID", "BillCode", installmentBillDetail.BillID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductCode", installmentBillDetail.ProductID);
            return View(installmentBillDetail);
        }

        // POST: Admin/InstallmentBillDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BillID,ProductID,Quantity,InstallmentPrice")] InstallmentBillDetail installmentBillDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(installmentBillDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BillID = new SelectList(db.InstallmentBills, "ID", "BillCode", installmentBillDetail.BillID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductCode", installmentBillDetail.ProductID);
            return View(installmentBillDetail);
        }

        // GET: Admin/InstallmentBillDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstallmentBillDetail installmentBillDetail = db.InstallmentBillDetails.Find(id);
            if (installmentBillDetail == null)
            {
                return HttpNotFound();
            }
            return View(installmentBillDetail);
        }

        // POST: Admin/InstallmentBillDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InstallmentBillDetail installmentBillDetail = db.InstallmentBillDetails.Find(id);
            db.InstallmentBillDetails.Remove(installmentBillDetail);
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
