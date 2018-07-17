using LoaderApiAndAdmin.DataBase;
using LoaderApiAndAdmin.Models;
using LoaderApiAndAdmin.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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
                ViewBag.OrderData = dbContext.Orders.Where(e => e.OrderStatus != "x").ToList();
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

                ViewBag.QuotesData = dbContext.Orders.Where(e => (e.OrderStatus == check1 || e.OrderStatus == check2)).ToList();
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

                //                Notifications.SendPushNotification("fUgPI4UUHHM:APA91bFE7tUHongWx4TphScu96RFZc1dieL2dqJ23lXE4K9qCiB3gcFnIsF7d4V-KIo0EAGzvb0__PCqmmHgfiGU6qhd1niEApFzt5bt1DtTsjeW_BNQCI_N_w1fEIs-pe0RYdJizSnU", "testing Push online");


                ViewBag.QuotesDetail = new LoaderAppEntites().Quotes.Where(e => e.OrderId == id).ToList();
                return View();



            }
            return RedirectToAction("SignIn");

            //return View();



        }
        public ActionResult AcceptQuote(int id = 0)
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

        public ActionResult SubmitQuote(SubmitQuoteInput model, int id)
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

                var client = dbContext.Users.Where(e => e.Id == orderToUpdate.ClientId).FirstOrDefault();

                var devId = client.DevId;
                var msg = "Hey " + client.FirstName + " " + client.LastName + " You Have an order pending for budget approval";
                Notifications.SendPushNotification(devId, msg);

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

                var transportOwner = dbContext.Users.Where(e => e.Id == order.TransportOwnerId).FirstOrDefault();

                var devId = transportOwner.DevId;
                var msg = "Hey " + transportOwner.FirstName + " " + transportOwner.LastName + " Your Quote Has been accepted";
                Notifications.SendPushNotification(devId, msg);





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
                var quote = dbContext.Quotes.Where(e => e.OrderId == Id).FirstOrDefault();
                quote.QuoteStatus = "Confirm Transit";

                var client = dbContext.Users.Where(e => e.Id == order.ClientId).FirstOrDefault();

                var devId = client.DevId;
                var msg = "Hey " + client.FirstName + " " + client.LastName + " Your order is In Transit";

                Notifications.SendPushNotification(devId, msg);


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
                ViewBag.CompletedOrders = dbContext.Quotes.Where(e => e.QuoteStatus == check)
                    .Select(e => new QuoteDto()
                    {
                        rating = 0,
                        Id = e.Id,
                        OrderId = e.OrderId,
                        TransportOwnerId = e.TransportOwnerId,
                        QuoteBudget = e.QuoteBudget,
                        QuoteStatus = e.QuoteStatus,
                        TransportOwnerName = e.TransportOwnerName,
                    })
                    .ToList();
                return View();
            }
            return RedirectToAction("SignIn");

        }

        public ActionResult ConfirmCompleted(int Id, string ToId)
        {
            var splitString = ToId.Split(' ');
            var toId = Convert.ToInt32(splitString[0]);
            var rating = Convert.ToInt32(splitString[1]);
            //Session["SignIn"] = false;
            if (Session["SignIn"] != null)
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();

                var order = dbContext.Orders.Where(e => e.Id == Id).SingleOrDefault();
                var quote = dbContext.Quotes.Where(e => e.OrderId == Id).SingleOrDefault();
                order.OrderStatus = "Completed";
                quote.QuoteStatus = "Job Completed";
                dbContext.Ratings.Add(new Rating()
                {
                    ToId= toId,
                    Rating1=rating

                });

                var client = dbContext.Users.Where(e => e.Id == order.ClientId).FirstOrDefault();
                var vehicle = dbContext.Vehicles.Where(e => e.UserId == quote.TransportOwnerId).FirstOrDefault();

                vehicle.VehicleIsBooked = false;



                var devId = client.DevId;
                var msg = "Hey " + client.FirstName + " " + client.LastName + " Your order is Completed";

                Notifications.SendPushNotification(devId, msg);



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



        public ActionResult DeleteSingleClient(int id)
        {

            //Session["SignIn"] = false;
            if (Session["SignIn"] != null)
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                var userToDelete = dbContext.Users.Single(e => e.Id == id);
                dbContext.Orders.Where(e => e.ClientId == id).ToList().ForEach(e =>
                {
                    dbContext.Orders.Remove(e);
                });
                dbContext.Users.Remove(userToDelete);
                dbContext.SaveChanges();

                return RedirectToAction("showClients");
            }
            return RedirectToAction("SignIn");



        }
        public ActionResult DeleteSingleOrder(int id)
        {

            //Session["SignIn"] = false;
            if (Session["SignIn"] != null)
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                var orderToDelete = dbContext.Orders.Single(e => e.Id == id);
                dbContext.Quotes.Where(e => e.OrderId == id).ToList().ForEach(e =>
                {
                    dbContext.Quotes.Remove(e);
                });
                dbContext.Orders.Remove(orderToDelete);
                dbContext.SaveChanges();

                return RedirectToAction("viewAvailableOrders");
            }
            return RedirectToAction("SignIn");



        }

        public ActionResult DeleteSingleTo(int id)
        {

            //Session["SignIn"] = false;
            if (Session["SignIn"] != null)
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                var userToDelete = dbContext.Users.Single(e => e.Id == id);


                dbContext.Quotes.Where(e => e.TransportOwnerId == id).ToList().ForEach(e =>
                {
                    dbContext.Quotes.Remove(e);
                });

                var vehicle = dbContext.Vehicles.Single(e => e.UserId == id);
                dbContext.Vehicles.Remove(vehicle);
                dbContext.Users.Remove(userToDelete);
                dbContext.SaveChanges();

                return RedirectToAction("showTOs");
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
                ViewBag.TosData = dbContext.Users.Where(e => e.Role == check1).Select(e =>
                new UserDto()
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    PhoneNo = e.PhoneNo,
                    CompanyName = e.CompanyName,
                    Rating = dbContext.Ratings.Where(r => r.ToId == e.Id).Sum(r => r.Rating1) / dbContext.Ratings.Where(r => r.ToId == e.Id).Count()
                }).ToList();
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

    public partial class QuoteDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int TransportOwnerId { get; set; }
        public string QuoteBudget { get; set; }
        public string QuoteStatus { get; set; }
        public string TransportOwnerName { get; set; }

        public int rating { get; set; }


    }


    public partial class UserDto
    {
      

        public int Id { get; set; }
        public string ImgId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNo { get; set; }
        public string Role { get; set; }
        public string DevId { get; set; }
     
       public int? Rating { get; set; }
     
    }
}