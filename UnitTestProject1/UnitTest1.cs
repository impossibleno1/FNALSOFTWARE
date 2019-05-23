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
    public class ProductAdminTest
    {
        [TestMethod]
        public void TestIndex()
        {
            var controller = new ProductAdminController();
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(c => c.Session).Returns(session.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            session.Setup(s => s["Username"]).Returns("abc");

            var result = controller.Index() as ViewResult;
            var db = new DIENMAYQUYETTIENEntities1();


            //Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(List<Product>));
            Assert.AreEqual(db.Products.Count(), ((List<Product>)result.Model).Count);

            session.Setup(s => s["Username"]).Returns(null);
            var redirect = controller.Index() as RedirectToRouteResult;
            //Assert.AreEqual("Login", redirect.RouteValues["controller"]);
            Assert.AreEqual("Login", redirect.RouteValues["action"]);
            
        }

        [TestMethod]
        public void TestCreate1()
        {
            var controller = new ProductAdminController();
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(c => c.Session).Returns(session.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            session.Setup(s => s["Username"]).Returns("abc");

            var result = controller.Create() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData["ProductTypeID"], typeof(SelectList));
        }

        [TestMethod]
        public void TestDetails()
        {
            var controller = new ProductAdminController();
            var context = new Mock<HttpContextBase>();
            context.Setup(c => c.Server.MapPath("~/App_Data/0")).Returns("~/App_Data/0");
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            var result = controller.Details(0) as FilePathResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("images", result.ContentType);
            Assert.AreEqual("~/App_Data/0", result.FileName);
        }

        [TestMethod]
        public void TestCreate2()
        {
            var controller = new ProductAdminController();
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var files = new Mock<HttpFileCollectionBase>();
            var file = new Mock<HttpPostedFileBase>();
            context.Setup(c => c.Request).Returns(request.Object);
            request.Setup(r => r.Files).Returns(files.Object);
            files.Setup(f => f["Avatar"]).Returns(file.Object);
            file.Setup(f => f.ContentLength).Returns(1);
            context.Setup(c => c.Server.MapPath("~/App_Data")).Returns("~/App_Data");
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            var db = new 1();
            var model = new Product();
            model.ProductTypeID = db.ProductTypes.First().ID;
            model.ProductName = "TenSP";
            model.ProductCode = "TVI0001";
            model.OriginPrice = 123;
            model.SalePrice = 456;
            model.InstallmentPrice = 789;
            model.Quantity = 10;

            using (var scope = new TransactionScope())
            {
                var result0 = controller.Create(model) as RedirectToRouteResult;
                Assert.IsNotNull(result0);
                file.Verify(f => f.SaveAs(It.Is<string>(s => s.StartsWith("~/App_Data/"))));
                Assert.AreEqual("Index", result0.RouteValues["action"]);

                file.Setup(f => f.ContentLength).Returns(0);
                var result1 = controller.Create(model) as ViewResult;
                Assert.IsNotNull(result1);
                Assert.IsInstanceOfType(result1.ViewData["ProductTypeID"], typeof(SelectList));
            }
        }
        [TestMethod]
        public void TestEdit1()
        {
            // arrange
            var db = new DIENMAYQUYETTIENEntities();

            var expectedProduct = db.Products.First();

            var controller = new ProductAdminController();
            var session = new Mock<HttpSessionStateBase>();
            var context = new Mock<HttpContextBase>();
            //context.Setup(c => c.Session["Username"]).Returns("abc");
            context.Setup(c => c.Session).Returns(session.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            session.Setup(s => s["Username"]).Returns("abc");

            // act
            var result = controller.Edit(expectedProduct.ID) as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData["ProductTypeID"], typeof(SelectList));
            //Assert.IsInstanceOfType(result.ViewBag.ProductTypes, typeof(List<ProductType>))
        }

        [TestMethod]
        public void TestEdit2()
        {
            var db = new DIENMAYQUYETTIENEntities();
            var controller = new ProductAdminController();
            var expectedProduct = db.Products.AsNoTracking().First();
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var files = new Mock<HttpFileCollectionBase>();
            var file = new Mock<HttpPostedFileBase>();
            context.Setup(c => c.Request).Returns(request.Object);
            request.Setup(r => r.Files).Returns(files.Object);
            files.Setup(f => f["Avatar"]).Returns(file.Object);
            file.Setup(f => f.ContentLength).Returns(1);
            context.Setup(c => c.Server.MapPath("~/App_Data")).Returns("~/App_Data");
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            //var db = new DIENMAYQUYETTIENEntities();
            //var model = new Product();
            //model.ProductTypeID = db.ProductTypes.First().ID;
            //model.ProductName = "TenSP";
            //model.ProductCode = "TVI0001";
            //model.OriginPrice = 123;
            //model.SalePrice = 456;
            //model.InstallmentPrice = 789;
            //model.Quantity = 10;

            using (var scope = new TransactionScope())
            {
                var result0 = controller.Edit(expectedProduct) as RedirectToRouteResult;
                Assert.IsNotNull(result0);
                //file.Verify(f => f.SaveAs(It.Is<string>(s => s.StartsWith("~/App_Data/"))));
                Assert.AreEqual("Index", result0.RouteValues["action"]);

                file.Setup(f => f.ContentLength).Returns(0);
                var result1 = controller.Edit(expectedProduct) as RedirectToRouteResult;
                Assert.IsNotNull(result1);
                //Assert.IsInstanceOfType(result1.ViewData["ProductTypeID"], typeof(SelectList));

            }
        }

        [TestMethod]
        public void TestDelete()
        {
            var db = new DIENMAYQUYETTIENEntities();
            var product = new Product
            {
                ProductName = "ProductName",
                ProductTypeID = db.ProductTypes.First().ID,
                SalePrice = 123,
                OriginPrice = 123,
                InstallmentPrice = 123,
                Quantity = 123,
                Avatar = ""
            };

            var controller = new ProductAdminController();
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            session.Setup(s => s["Username"]).Returns("abc");
            context.Setup(c => c.Session).Returns(session.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            using (var scope = new TransactionScope())
            {
                db.Products.Add(product);
                db.SaveChanges();
                var count = db.Products.Count();
                var result2 = controller.DeleteConfirmed(product.ID) as RedirectToRouteResult;
                Assert.IsNotNull(result2);
                Assert.AreEqual(count - 1, db.Products.Count());
            }
        }

    }
}
