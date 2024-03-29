﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DIENMAYQUYETTIEN2.Areas.Admin.Controllers;
using System.Collections.Generic;
using DIENMAYQUYETTIEN2.Models;
using System.Web.Mvc;
using System.Linq;
using Moq;
using System.Web;
using System.Web.Routing;
using DIENMAYQUYETTIEN2.Controllers;
using DIENMAYQUYETTIEN2.Areas;

namespace UnitTestProject1

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
            var db = new DIENMAYQUYETTIENEntities();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(List<Product>));
            Assert.AreEqual(db.Products.Count(), ((List<Product>)result.Model).Count);
            session.Setup(s => s["Username"]).Returns("null");

        }

        [TestMethod]
        public void CreateTest()
        {
            var controller = new ProductAdminController();
            var db = new DIENMAYQUYETTIENEntities();
            var result = controller.Create() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData["product.ID"], typeof(SelectList));
        }

    }
}