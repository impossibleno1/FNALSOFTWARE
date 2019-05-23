using DIENMAYQUYETTIEN2.Areas.Admin.Controllers;
using DIENMAYQUYETTIEN2.Controllers;
using DIENMAYQUYETTIEN2.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace UnitTestProject1.Tests.Controllers
{
    [TestClass]
    public class CashBillTest
    {
        [TestMethod]
        public void TestIndexCashBill()
        {
            var controller = new CashBillsController();
            //var context = new Mock<HttpContextBase>();
            //var session = new Mock<HttpSessionStateBase>();
            //context.Setup(c => c.Session).Returns(session.Object);
            //controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            //session.Setup(s => s["Username"]).Returns("abc");

            var result = controller.Index() as ViewResult;
            var db = new DIENMAYQUYETTIENEntities1();


            //Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(List<CashBill>));
            Assert.AreEqual(db.CashBills.Count(), ((List<CashBill>)result.Model).Count);

            //session.Setup(s => s["Username"]).Returns(null);
            //var redirect = controller.Index() as RedirectToRouteResult;
            ////Assert.AreEqual("Login", redirect.RouteValues["controller"]);
            //Assert.AreEqual("Login", redirect.RouteValues["action"]);

        }

        [TestMethod]
        public void TestIndexInstallmentBill()
        {
            var controller = new InstallmentBillsController();
            var result = controller.Index() as ViewResult;
            var db = new DIENMAYQUYETTIENEntities1();

            Assert.IsInstanceOfType(result.Model, typeof(List<InstallmentBill>));
            Assert.AreEqual(db.InstallmentBills.Count(), ((List<InstallmentBill>)result.Model).Count);

        }
    }
}
