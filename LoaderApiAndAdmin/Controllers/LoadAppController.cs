using LoaderApiAndAdmin.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LoaderAppApi.Controllers
{
    [EnableCorsAttribute("*", "*", "*", SupportsCredentials = true)]
    public class LoadAppController : ApiController
    {
        [HttpPost]
        public dynamic UpdateProfile(User Input)
        {
            try {
                LoaderAppEntites dbContext = new LoaderAppEntites();

                var checkIfAlreayExist = dbContext.Users.FirstOrDefault(e => e.PhoneNo != Input.PhoneNo);
                if (checkIfAlreayExist == null)
                {
                    dbContext.Users.Add(Input);
                    dbContext.SaveChanges();

                    var userId = dbContext.Users.FirstOrDefault(e => e.PhoneNo == Input.PhoneNo);
                    return new
                    {
                        IsUserUpdated = true,
                        ErrorException = "null",
                        UserId = userId.Id

                    };


                }
                else
                {
                    return new
                    {
                        IsUserUpdated = false,
                        ErrorException = "Phone No Already Exists",
                        UserId = "-1"
                    };

                }
            }
            catch (Exception ex)
            {
                return new
                {
                    IsUserUpdated = false,
                    ErrorException = ex,
                    UserId = "-1"
                };
            }
        }

        [HttpPost]
        public dynamic SignIn(SignInInput input)
        {
            LoaderAppEntites dbContext = new LoaderAppEntites();
            var userEntity = dbContext.Users.FirstOrDefault(e => e.PhoneNo == input.PhoneNo && e.Password == input.Password);
            if (userEntity == null)
            {
                return new
                {
                    IsSignedIn = false,
                    Message = "Check Phone no or password",
                    UserData = "null"
                };

            }
            else {

                return new
                {
                    IsSignedIn = true,
                    Message = "SignIn sucessfull",
                    UserData = userEntity

                };

            }



        }
        [HttpPost]
        public dynamic UpdateVehicle(Vehicle Input)
        {
            try
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                dbContext.Vehicles.Add(Input);
                dbContext.SaveChanges();
                return new
                {
                    IsVehicleUpdated = true,
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    IsVehicleUpdated = false,
                    ErrorException = ex
                };
            }
        }
        [HttpPost]
        public dynamic PlaceAnOrder(Order Input)
        {
            try
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                dbContext.Orders.Add(Input);
                dbContext.SaveChanges();
                return new
                {
                    IsOrderUpdated = true,
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    IsOrderUpdated = false,
                    ErrorException = ex
                };
            }
        }
        [HttpPost]
        public dynamic ViewOrderDetailsOfClient(ViewOrderDetailsOfClientInput Input)
        {
            try
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                var orderist=  dbContext.Orders.Where(e=>e.ClientId==Input.ClientId).ToList();
                return orderist;
               
            }
            catch (Exception ex)
            {
                return new
                {
                    IsOrderUpdated = false,
                    ErrorException = ex
                };
            }
        }
        [HttpPost]
        public dynamic AcceptOrDeclineOffer(AcceptOrDeclineOfferInput Input)
        {
            try
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                var order= dbContext.Orders.Where(e => e.Id == Input.OrderId).Single();
                order.OrderStatus = Input.Status;
                dbContext.SaveChanges();
                return new
                {
                    IsOrderUpdated = true,
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    IsOrderUpdated = false,
                    ErrorException = ex
                };
            }
        }
        [HttpPost]
        public dynamic ViewOrderDetailsOfTransportOwner(ViewOrderDetailsOfTransportOwnerIput Input)
        {
            try
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                return dbContext.Orders.Where(e => e.TransportOwnerId == Input.TransportOwnerId).ToList();
               
            }
            catch (Exception ex)
            {
                return new
                {
                    IsOrderUpdated = false,
                    ErrorException = ex
                };
            }
        }
        [HttpPost]
        public dynamic ViewAvailableOrdersToBid()
        {
            try
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                return dbContext.Orders
                    .Where(e => e.OrderStatus=="Pending" || e.OrderStatus == "Rejected")
                    .ToList();

            }
            catch (Exception ex)
            {
                return new
                {
                    IsOrderUpdated = false,
                    ErrorException = ex
                };
            }
        }

    }


    // dto's
    public class ViewOrderDetailsOfClientInput
    {
        public int ClientId { get; set; }

    }

    public class SignInInput
    {
        public string PhoneNo { get; set; }
        public string Password { get; set; }

    }


    public class AcceptOrDeclineOfferInput
    {
        public int OrderId { get; set; }

        public string Status { get; set; }
    }

    public class ViewOrderDetailsOfTransportOwnerIput
    {
        public int TransportOwnerId { get; set; }
    }
}


