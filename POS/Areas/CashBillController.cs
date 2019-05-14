using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using POS;

namespace POS.Areas
{
    public class CashBillController : Controller
    {
        private DIENMAYQUYETTIENEntities db = new DIENMAYQUYETTIENEntities();

        // GET: /CashBill/
        public ActionResult Index()
        {
            return View(db.CashBills.ToList());
        }
        
        // GET: /CashBill/Details/5
        public ActionResult Details(int? id)
        {

            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashBill cashbill = db.CashBills.Find(id);
            Session["id"] = id;
            if (cashbill == null)
            {
                return HttpNotFound();
            }
            return View(cashbill);
        }
        public ActionResult ShowProduct()
        {
            var ids = (Int32)Session["id"];
            var query = db.CashBillDetails.Where(cbd => cbd.BillID == ids).ToList();
            var b = query as List<CashBillDetail>;
            return View(b);
        }
        // GET: /CashBill/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /CashBill/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,BillCode,CustomerName,PhoneNumber,Address,Date,Shipper,Note,GrandTotal")] CashBill cashbill)
        {
            if (ModelState.IsValid)
            {
                db.CashBills.Add(cashbill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cashbill);
        }

        // GET: /CashBill/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashBill cashbill = db.CashBills.Find(id);
            if (cashbill == null)
            {
                return HttpNotFound();
            }
            return View(cashbill);
        }

        // POST: /CashBill/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,BillCode,CustomerName,PhoneNumber,Address,Date,Shipper,Note,GrandTotal")] CashBill cashbill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cashbill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cashbill);
        }

        // GET: /CashBill/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashBill cashbill = db.CashBills.Find(id);
            if (cashbill == null)
            {
                return HttpNotFound();
            }
            return View(cashbill);
        }

        // POST: /CashBill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CashBill cashbill = db.CashBills.Find(id);
            db.CashBills.Remove(cashbill);
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
