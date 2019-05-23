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
using System.Web.Security;
using System.Web.Configuration;
using DIENMAYQUYETTIEN2.Areas.Admin.Models;

namespace DIENMAYQUYETTIEN2.Areas.Admin.Controllers
{
    public class CashBillsController : Controller
    {
        private DIENMAYQUYETTIENEntities1 db = new DIENMAYQUYETTIENEntities1();

        // GET: Admin/CashBills
        public ActionResult Index()
        {
            var cashbill = db.CashBills.Include(x=>x.CashBillDetails).ToList();

            //if (Session["Username"] != null)
            //{
            //    return View(cashbill);
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}
            return View(cashbill);
        }


        // GET: Admin/CashBills/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View(Session["CashBill"]);
        }

        // POST: Admin/CashBills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CashBill model)
        {
            if (ModelState.IsValid)
            {
                Session["CashBill"] = model;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2()
        {
            using (var scope = new TransactionScope())
            try
            {
                var cashBill = Session["CashBill"] as CashBill;
                var ctcashBill = Session["ctcashBill"] as List<CashBillDetail>;

                db.CashBills.Add(cashBill);
                db.SaveChanges();

                foreach (var chiTiet in ctcashBill)
                {
                    chiTiet.BillID = cashBill.ID;
                    chiTiet.Product = null;
                    db.CashBillDetails.Add(chiTiet);
                    cashBill.GrandTotal += (chiTiet.Quantity * chiTiet.SalePrice);
                }

                db.SaveChanges();
                scope.Complete();

                Session["CashBill"] = null;
                Session["ctcashBill"] = null;
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View("Create");
        }

        //[HttpPost]
        //public ActionResult Login(Account acc)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using (DIENMAYQUYETTIENEntities db = new DIENMAYQUYETTIENEntities())
        //        {
        //            var admin = "admin";
        //            var obj = db.Accounts.Where(a => a.Username.Equals(acc.Username) && a.Password.Equals(acc.Password)).FirstOrDefault();

        //            if (obj != null)
        //            {
        //                Session["Username"] = obj.Username.ToString();
        //                Session["FullName"] = obj.FullName.ToString();
        //                return RedirectToAction("Index");
        //            }
                    
                    
        //        }
        //    }
        //    return View(acc);
        //}

        //[HttpPost]
        //public ActionResult Logout()
        //{
        //    Session.Clear();
        //    Session.Abandon(); // it will clear the session at the end of request
        //    FormsAuthentication.SignOut();
        //    return RedirectToAction("Login");
        //}

        // GET: Admin/CashBills/Edit/5
        public ActionResult Edit(int? id)
        {
            Session["id"] = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CashBill cashbill2 = db.CashBills.Find(id);


            var query = db.CashBillDetails.Where(cbd => cbd.BillID == id).ToList();
            var b = query as List<CashBillDetail>;
            var sum = 0;
            foreach (var chiTiet in b)
            {
                sum += (chiTiet.Quantity * chiTiet.SalePrice);
            }
            cashbill2.GrandTotal = sum;


            Session["CashBill2"] = cashbill2;
            if (cashbill2 == null)
            {
                return HttpNotFound();
            }

            return View(Session["CashBill2"]);
        }

        // POST: Admin/CashBills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CashBill model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Admin/CashBills/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashBill cashBill = db.CashBills.Find(id);
            if (cashBill == null)
            {
                return HttpNotFound();
            }
            return View(cashBill);
        }

        // POST: Admin/CashBills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CashBill cashBill = db.CashBills.Find(id);
            db.CashBills.Remove(cashBill);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Print(int id)
        {
            var order = db.CashBills.FirstOrDefault(o => o.ID == id);
            if (order != null)
            {
                ReceiptModel rp = new ReceiptModel();
                rp.Address = order.Address;
                rp.BillCode = order.BillCode;
                rp.CustomerName = order.CustomerName;
                rp.Date = order.Date;
                rp.GrandTotal = order.GrandTotal;
                rp.Note = order.Note;
                rp.PhoneNumber = order.PhoneNumber;
                rp.Shipper = order.Shipper;
                rp.CashBillDetail = order.CashBillDetails.ToList();
                return View(rp);
            }
            else
            {
                return View();
            }
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
