using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DIENMAYQUYETTIEN2.Models;

namespace DIENMAYQUYETTIEN2.Areas.Main.Controllers
{
    public class ContactController : Controller
    {
        DIENMAYQUYETTIENEntities1 db = new DIENMAYQUYETTIENEntities1();
        // GET: Main/Contact
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult Index2()
        //{



        //    return View(db.Contacts.ToList());

        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Contact ct)
        //{
        //    var product = db.Contacts.OrderByDescending(x => x.ID).ToList();
        //    var contact = new Contact();
        //    contact.ID = product[0].ID + 1;
        //    contact.TEN = ct.TEN;
        //    contact.EMAIL = ct.EMAIL;
        //    contact.PHONE = ct.PHONE;
        //    contact.MESSAGES = ct.MESSAGES;
        //    db.Contacts.Add(contact);
        //    db.SaveChanges();             
        //    return RedirectToAction("Index");
        //}
    }
}