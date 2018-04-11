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

        public ActionResult SignIn()
        {
            return View();

        }

        [HttpPost]
        public ActionResult SignInMethod(SignInInput input)
        {
            if (input.UserName == "Admin" && input.Password == "Admin")
            {
                Session["SignIn"] = new { SignIn = "True" };
                return RedirectToAction("AdminMain");
            }
            return View("SignIn");

        }
        //SignInInput input
        // GET: AdminMain
        public ActionResult AdminMain()
        {
            //Session["SignIn"] = false;
            if (Session["SignIn"] != null)
            {
                return View();
            }
            return RedirectToAction("SignIn");

            //return View();
        }

        public ActionResult SignOut()
        {
            Session["SignIn"] = null;
            return RedirectToAction("AdminMain");
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
            LoaderAppEntites dbContext = new LoaderAppEntites();
            ViewBag.QuotesData = dbContext.Orders.ToList();
            return View();

        }

        public ActionResult viewQuoteDetails(int id)
        {
            ViewBag.QuotesDetail = new LoaderAppEntites().Quotes.Where(e => e.OrderId == id).ToList();
            return View();
        }
        public ActionResult AcceptQuote(int id)
        {
            LoaderAppEntites dbContext = new LoaderAppEntites();
            var quote= dbContext.Quotes.Where(e => e.Id == id).FirstOrDefault();
            quote.QuoteStatus = "Waiting For Budget Approval";
            //quote.a
             var order= dbContext.Orders.Where(e => e.Id == quote.OrderId).FirstOrDefault();
             order.Quotes.ToList().Where(e => e.Id != id).ToList().ForEach(
                e =>
                {
                    dbContext.Quotes.Remove(e);
                }
                );
            dbContext.SaveChanges();
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

    public class SignInInput
    {
        public string UserName { get; set; }

        public string Password { get; set; }
        
    }
}