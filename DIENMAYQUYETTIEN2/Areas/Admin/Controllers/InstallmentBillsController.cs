using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DIENMAYQUYETTIEN2.Models;
using System.Transactions;
using DIENMAYQUYETTIEN2.Areas.Admin.Models;

namespace DIENMAYQUYETTIEN2.Areas.Admin.Controllers
{
    public class InstallmentBillsController : Controller
    {
        private DIENMAYQUYETTIENEntities1 db = new DIENMAYQUYETTIENEntities1();

        // GET: Admin/InstallmentBills
        public ActionResult Index()
        {
            var inscashbill = db.InstallmentBills.Include(b => b.InstallmentBillDetails).ToList();

            //if (Session["Username"] != null)
            //{
            //    return View(inscashbill);
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}
                return View(inscashbill);
            }

        // GET: Admin/InstallmentBills/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "CustomerName");
            return View(Session["InsCashBill"]);
        }

        // POST: Admin/InstallmentBills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InstallmentBill model)
        {
            
            if (ModelState.IsValid)
            {
                
                Session["InsCashBill"] = model;

            }
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "CustomerName", model.CustomerID);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2()
        {
            
            using (var scope = new TransactionScope())
                try
                {
                    var inscashBill = Session["InsCashBill"] as InstallmentBill;
                    var ctinscashBill = Session["insctcashBill"] as List<InstallmentBillDetail>;

                    db.InstallmentBills.Add(inscashBill);
                    db.SaveChanges();

                    foreach (var chiTiet in ctinscashBill)
                    {
                        chiTiet.BillID = inscashBill.ID;
                        chiTiet.Product = null;
                        db.InstallmentBillDetails.Add(chiTiet);
                        inscashBill.GrandTotal += (chiTiet.Quantity * chiTiet.InstallmentPrice);
                        
                    }

                    db.SaveChanges();
                    scope.Complete();

                    Session["InsCashBill"] = null;
                    Session["insctcashBill"] = null;
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            
            return View("Create");
        }

        // GET: Admin/InstallmentBills/Edit/5
        public ActionResult Edit(int? id)
        {
            Session["idi"] = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstallmentBill installmentBill = db.InstallmentBills.Find(id);
            var query = db.CashBillDetails.Where(cbd => cbd.BillID == id).ToList();
            var b = query as List<CashBillDetail>;
            var sum = 0;
            foreach (var chiTiet in b)
            {
                sum += (chiTiet.Quantity * chiTiet.SalePrice);
            }
            installmentBill.GrandTotal = sum;

            Session["CashBilli"] = installmentBill;
            if (installmentBill == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "CustomerCode", installmentBill.CustomerID);
            return View(installmentBill);
        }

        // POST: Admin/InstallmentBills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( InstallmentBill model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "CustomerCode", model.CustomerID);
            return View(model);
        }

        // GET: Admin/InstallmentBills/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstallmentBill installmentBill = db.InstallmentBills.Find(id);
            if (installmentBill == null)
            {
                return HttpNotFound();
            }
            return View(installmentBill);
        }

        // POST: Admin/InstallmentBills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InstallmentBill installmentBill = db.InstallmentBills.Find(id);
            db.InstallmentBills.Remove(installmentBill);
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

        public ActionResult Print(int id)
        {
            var order = db.InstallmentBills.FirstOrDefault(o => o.ID == id);
            if (order != null)
            {
                ReceiptModel2 rp = new ReceiptModel2();
                rp.BillCode = order.BillCode;
                
                rp.CustomerID = order.Customer.CustomerName;
                rp.CustomerP = order.Customer.PhoneNumber;
                rp.Date = order.Date;
                rp.GrandTotal = order.GrandTotal;
                rp.Shipper = order.Shipper;
                rp.Method = order.Method;
                rp.Period = order.Period;
                rp.Note = order.Note;
                rp.Taken = order.Taken;
                rp.Remain = order.Remain;
                rp.InstallmentBillDetails = order.InstallmentBillDetails.ToList();
                return View(rp);
            }
            else
            {
                return View();
            }
        }
    }
}
