﻿using LoaderApiAndAdmin.DataBase;
using LoaderApiAndAdmin.Models;
using LoaderApiAndAdmin.Notifications;
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

            if (Session["SignIn"] != null)
            {
                // Session["SignIn"] = true;
                var listOfOrders = new List<Order>();
                LoaderAppEntites dbContext = new LoaderAppEntites();
                // AllOrderS From Order Table
                ViewBag.OrderData = dbContext.Orders.Where(e => e.OrderStatus !="x").ToList();
                return View();
            }
            return RedirectToAction("SignIn");

        }

        public ActionResult viewAvailableQuotes()
        {

            //Session["SignIn"] = false;
            if (Session["SignIn"] != null)
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                var check1 = "Pending";
                var check2 = "Rejected";

                ViewBag.QuotesData = dbContext.Orders.Where(e => (e.OrderStatus == check1 || e.OrderStatus == check2) ).ToList();
                return View();
            }
            return RedirectToAction("SignIn");

            //return View();



        }

        public ActionResult viewQuoteDetails(int id)
        {

            //Session["SignIn"] = false;
            if (Session["SignIn"] != null)
            {

                ViewBag.QuotesDetail = new LoaderAppEntites().Quotes.Where(e => e.OrderId == id).ToList();
                return View();
            }
            return RedirectToAction("SignIn");

            //return View();



        }
        public ActionResult AcceptQuote(int id =0)
        {
            //Session["SignIn"] = false;
            if (Session["SignIn"] != null)
            {

                LoaderAppEntites dbContext = new LoaderAppEntites();
                var quote = dbContext.Quotes.Where(e => e.Id == id).FirstOrDefault();

                ViewBag.quoteData = quote;



                return View();
            }
            return RedirectToAction("SignIn");

            //return View();

        }

        public ActionResult SubmitQuote(SubmitQuoteInput model, int id )
        {


            //Session["SignIn"] = false;
            if (Session["SignIn"] != null)
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                var quote = dbContext.Quotes.Where(e => e.Id == id).FirstOrDefault();
                //hamza start

                Double BudgetForClient = Double.Parse(quote.QuoteBudget) + model.comission;

                var orderToUpdate = dbContext.Orders.Where(e => e.Id == quote.OrderId).FirstOrDefault();
                orderToUpdate.OrderStatus = "Waiting For Budget Approval";
                orderToUpdate.TransportOwnerId = quote.TransportOwnerId;
                orderToUpdate.OrderBudget = BudgetForClient.ToString();

                var vehicle = dbContext.Vehicles.Where(e => e.UserId == quote.TransportOwnerId).FirstOrDefault();
                vehicle.VehicleIsBooked = true;


                //hamza end



                quote.QuoteStatus = "Waiting For Budget Approval";
                var order = dbContext.Orders.Where(e => e.Id == quote.OrderId).FirstOrDefault();
                order.Quotes.ToList().Where(e => e.Id != id).ToList().ForEach(
                    e =>
                    {
                        dbContext.Quotes.Remove(e);
                    }
                    );
                dbContext.SaveChanges();


                return RedirectToAction("viewAvailableQuotes");
            }
            return RedirectToAction("SignIn");



        }

        public ActionResult updateOrderAsConfirmedToTransportOwner()
        {
            //Session["SignIn"] = false;
            if (Session["SignIn"] != null)
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                var check = "Accepted";
                ViewBag.AcceptedOrders = dbContext.Orders.Where(e => e.OrderStatus == check).ToList();
                return View();
            }
            return RedirectToAction("SignIn");



        }

        public ActionResult sendOrderConfirmationtoTO(int id)
        {


            //Session["SignIn"] = false;
            if (Session["SignIn"] != null)
            {

                LoaderAppEntites dbContext = new LoaderAppEntites();
                var quotes = dbContext.Quotes.Where(e => e.OrderId == id).FirstOrDefault();
                var order = dbContext.Orders.Where(e => e.Id == id).FirstOrDefault();
                order.OrderStatus = "Confirmed";
                quotes.QuoteStatus = "Confirmed";
                dbContext.SaveChanges();

                return RedirectToAction("updateOrderAsConfirmedToTransportOwner");

            }
            return RedirectToAction("SignIn");

        }

        


        public ActionResult changeStatusToTransit()
        {
            //Session["SignIn"] = false;
            if (Session["SignIn"] != null)
            {

                LoaderAppEntites dbContext = new LoaderAppEntites();
                ViewBag.TransitOrders = dbContext.Quotes.Where(e => e.QuoteStatus == "In Transit").ToList();
                return View();


            }
            return RedirectToAction("SignIn");


        }

        public ActionResult ConfirmTransit(int Id)
        {

            //Session["SignIn"] = false;
            if (Session["SignIn"] != null)
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                var order = dbContext.Orders.Where(e => e.Id == Id).FirstOrDefault();
                order.OrderStatus = "In Transit";
                dbContext.SaveChanges();
                return RedirectToAction("changeStatusToTransit");
            }
            return RedirectToAction("SignIn");
        }

        public ActionResult changeStatusToCompleted()
        {

            //Session["SignIn"] = false;
            if (Session["SignIn"] != null)
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                var check = "Completed";
                ViewBag.CompletedOrders = dbContext.Quotes.Where(e => e.QuoteStatus == check).ToList();
                return View();
            }
            return RedirectToAction("SignIn");

        }

        public ActionResult ConfirmCompleted(int Id)
        {

            //Session["SignIn"] = false;
            if (Session["SignIn"] != null)
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();

                var order = dbContext.Orders.Where(e => e.Id == Id).SingleOrDefault();
                var quote = dbContext.Quotes.Where(e => e.OrderId == Id).SingleOrDefault();
                order.OrderStatus = "Completed";
                quote.QuoteStatus = "Job Completed";
                dbContext.SaveChanges();

                return RedirectToAction("changeStatusToCompleted");
            }
            return RedirectToAction("SignIn");

        }

        public ActionResult showClients()
        {

            //Session["SignIn"] = false;
            if (Session["SignIn"] != null)
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                var check1 = "Customer";

                ViewBag.ClientsData = dbContext.Users.Where(e => e.Role == check1).ToList();
                return View();
            }
            return RedirectToAction("SignIn");



        }


        public ActionResult showTOs()
        {

            //Session["SignIn"] = false;
            if (Session["SignIn"] != null)
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                var check1 = "TransportOwner";
                ViewBag.TosData = dbContext.Users.Where(e => e.Role == check1).ToList();
                return View();
            }
            return RedirectToAction("SignIn");

        }


    }

    public class SignInInput
    {
        public string UserName { get; set; }

        public string Password { get; set; }
        
    }
}