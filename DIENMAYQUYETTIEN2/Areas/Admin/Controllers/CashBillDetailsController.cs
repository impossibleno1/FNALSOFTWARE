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
    public class CashBillDetailsController : Controller
    {
        private DIENMAYQUYETTIENEntities1 db = new DIENMAYQUYETTIENEntities1();

        // GET: Admin/CashBillDetails
        public ActionResult Index()
        {
            if (Session["ctcashBill"]==null)
            {
                Session["ctcashBill"] = new List<CashBillDetail>();   
            }
            return PartialView(Session["ctcashBill"]);
        }

        public PartialViewResult Editview()
        {

            var a = (Int32)Session["id"];
            ViewBag.id = a;
            var query = db.CashBillDetails.Where(cbd => cbd.BillID == a).ToList();
            var b = query as List<CashBillDetail>;
            var sum = 0;
            foreach (var chiTiet in b)
            {
                sum += (chiTiet.Quantity * chiTiet.SalePrice);
            }
            Session["Tonggia"] = sum;
            return PartialView(query);
        }

        // GET: /Admin/CashBillDetails/Details/5
        public int SalePrice(int ProductID)
        {
            return db.Products.Find(ProductID).SalePrice;
        }

        // GET: /Admin/CashBillDetails/Create
        public PartialViewResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName");
            var model = new CashBillDetail();
            model.ID = 0;
            model.Quantity = 1;
            return PartialView(model);
        }

        // POST: Admin/CashBillDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2(CashBillDetail model)
        {
            if (ModelState.IsValid)
            {
                model.ID = Environment.TickCount;
                model.Product = db.Products.Find(model.ProductID);
                var ctcashBill = Session["ctcashBill"] as List<CashBillDetail>;
                if (ctcashBill == null)
                    ctcashBill = new List<CashBillDetail>();
                ctcashBill.Add(model);
                Session["ctcashBill"] = ctcashBill;
                return RedirectToAction("Create", "CashBills");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductName", model.ProductID);
            return View("Create", model);
        }
        public ActionResult CreateEdit(int id)
        {
            var a = (Int32)Session["id"];
            ViewBag.id = a;
            CashBillDetail cashbilldetail = db.CashBillDetails.Find(id);

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

            var model = new CashBillDetail();
            model.BillID = id;
            model.Quantity = 1;
            return View(model);
        }

        // POST: /CashBillDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit( CashBillDetail cashbilldetail)
        {

            
            var a = (Int32)Session["id"];

            if (ModelState.IsValid)
            {

                db.CashBillDetails.Add(cashbilldetail);
                db.SaveChanges();
                return RedirectToAction("Edit", "CashBills", new { id = a });
                
            }

            ViewBag.BillID = new SelectList(db.CashBills, "ID", "BillCode", cashbilldetail.BillID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductCode", cashbilldetail.ProductID);
            return View(cashbilldetail);
        }
        // GET: Admin/CashBillDetails/Edit/5
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

        // POST: Admin/CashBillDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( CashBillDetail cashbilldetail)
        {
            var a = (Int32)Session["id"];
            if (ModelState.IsValid)
            {
                db.Entry(cashbilldetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "CashBills", new { id = a });
            }
            ViewBag.BillID = new SelectList(db.CashBills, "ID", "BillCode", cashbilldetail.BillID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "ProductCode", cashbilldetail.ProductID);
            return View(cashbilldetail);
        }

        // GET: Admin/CashBillDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashBillDetail cashBillDetail = db.CashBillDetails.Find(id);
            if (cashBillDetail == null)
            {
                return HttpNotFound();
            }
            return View(cashBillDetail);
        }

        

        // POST: Admin/CashBillDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var a = (Int32)Session["id"];
            CashBillDetail cashBillDetail = db.CashBillDetails.Find(id);
            db.CashBillDetails.Remove(cashBillDetail);
            db.SaveChanges();
            return RedirectToAction("Edit", "CashBills", new { id = a });
        }

        
    }
}
