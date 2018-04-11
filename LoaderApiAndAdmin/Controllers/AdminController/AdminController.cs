﻿using LoaderApiAndAdmin.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoadProject.Controllers.AdminController
{
    public class AdminController : Controller
    {
        // GET: AdminMain
        public ActionResult AdminMain()
        {
            return View();
        }
        [HttpGet]
        public ActionResult viewAvailableOrders()
        {
            // AllOrderS From Order Table
            LoaderAppEntites dbContext = new LoaderAppEntites();

            var users = dbContext.Users.ToList();

            return View(users);
        }

        public ActionResult viewAvailableQuotes()
        {
            // All From Order Table
            return View();
        }

        public ActionResult viewQuoteDetails()
        {

            return View();
        }
        public ActionResult AcceptQuote()
        {

            return View();
        }

        public ActionResult updateOrderAsConfirmedToTransportOwner()
        {

            return View();
        }

        public ActionResult changeStatusToTransit()
        {

            return View();
        }

        public ActionResult changeStatusToCompleted()
        {

            return View();
        }



    }
}