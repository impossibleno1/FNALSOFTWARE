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
    public class CashBillDetailController : Controller
    {
        private DIENMAYQUYETTIENEntities db = new DIENMAYQUYETTIENEntities();

        // GET: /CashBillDetail/
        public ActionResult Index()
        {
            var cashbilldetails = db.CashBillDetails.Include(c => c.CashBill).Include(c => c.Product);
            return View(cashbilldetails.ToList());
        }

        // GET: /CashBillDetail/Details/5
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashBillDetail cashbilldetail = db.CashBillDetails.Find(id);
            if (cashbilldetail == null)
            {
                return HttpNotFound();
            }
            return View(cashbilldetail);
        }
        
        
        // GET: /CashBillDetail/Create
        public ActionResult Create()
        {
            var ids = (Int32)Session["id"];

            CashBillDetail cashbilldetail = new CashBillDetail(); 
            cashbilldetail.BillID= ids;
            
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductCode");
            return View(cashbilldetail);
        }

        // POST: /CashBillDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,BillID,ProductID,Quantity,SalePrice")] CashBillDetail cashbilldetail)
        {
            if (ModelState.IsValid)
            {
                var ids = (Int32)Session["id"];
                db.CashBillDetails.Add(cashbilldetail);
                db.SaveChanges();
                return RedirectToAction("Details", "CashBill", new { @id = ids });
            }

            ViewBag.BillID = new SelectList(db.CashBills, "ID", "BillCode", cashbilldetail.BillID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductCode", cashbilldetail.ProductID);
            return View(cashbilldetail);
        }

        // GET: /CashBillDetail/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashBillDetail cashbilldetail = db.CashBillDetails.Find(id);
            if (cashbilldetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.BillID = new SelectList(db.CashBills, "ID", "BillCode", cashbilldetail.BillID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductCode", cashbilldetail.ProductID);
            return View(cashbilldetail);
        }

        // POST: /CashBillDetail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,BillID,ProductID,Quantity,SalePrice")] CashBillDetail cashbilldetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cashbilldetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BillID = new SelectList(db.CashBills, "ID", "BillCode", cashbilldetail.BillID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductCode", cashbilldetail.ProductID);
            return View(cashbilldetail);
        }

        // GET: /CashBillDetail/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashBillDetail cashbilldetail = db.CashBillDetails.Find(id);
            if (cashbilldetail == null)
            {
                return HttpNotFound();
            }
            return View(cashbilldetail);
        }

        // POST: /CashBillDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CashBillDetail cashbilldetail = db.CashBillDetails.Find(id);
            db.CashBillDetails.Remove(cashbilldetail);
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
