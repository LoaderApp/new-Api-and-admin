using LoaderApiAndAdmin.DataBase;
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
            //Session["SignIn"] = false;
            if (Session["SignIn"] != null)
            {
                return View();
            }
            return RedirectToAction("viewAvailableOrders");
        }


        public ActionResult viewAvailableOrders()
        {
           // Session["SignIn"] = true;
            var listOfOrders = new List<Order>();
            LoaderAppEntites dbContext = new LoaderAppEntites();
            // AllOrderS From Order Table
            ViewBag.OrderData = dbContext.Orders.ToList();
            return View();
        }

        public ActionResult viewAvailableQuotes()
        {
            var listOfOrders = new List<Order>();
            listOfOrders.Add(new Order()
            {
                Id = 1,
                ClientId = 2,
                TransportOwnerId = 3,
                OrderPickup = "Lahore",
                OrderDropOff = "Multan",
                OrderComodity = "",
                OrderWeight = "",
            });
            // AllOrderS From Order Table
            ViewBag.QuotesData = listOfOrders;
            return View();

        }

        public ActionResult viewQuoteDetails(int id)
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