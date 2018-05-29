using AutoMapper;
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

                var checkIfAlreayExist = dbContext.Users.FirstOrDefault(e => e.PhoneNo == Input.PhoneNo);
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
        public dynamic SignIn(User input)
        {

            try
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                var userEntity = dbContext.Users.FirstOrDefault(e => e.PhoneNo == input.PhoneNo && e.Password == input.Password);

               var userDto= Mapper.Map<User, UserDto>(userEntity);

                if (userDto == null)
                {
                    return new
                    {
                        IsSignedIn = false,
                        Message = "Check Phone no or password",
                        UserData = "null",
                        Error = "null"
                    };

                }
                else
                {

                    return new
                    {
                        IsSignedIn = true,
                        Message = "SignIn sucessfull",
                        UserData = userDto,
                        Error = "null"
                    };

                }

            }
            catch (Exception ex)
            {
                return new
                {
                    IsSignedIn = false,
                    Message = "Check Phone no or password",
                    UserData = "null",
                    Error = ex

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
        public dynamic ViewOrderDetailsOfClient(ViewOrderDetailsOfClientInput [] Input)
        {
            try
            {
                var inputClientId = Input[0].ClientId;

                LoaderAppEntites dbContext = new LoaderAppEntites();
                var orderist=  dbContext.Orders.Where(e=>e.ClientId==inputClientId).ToList();

                List<OrderDto> orderListDto = new List<OrderDto>();
                foreach (var order in orderist)
                {
                    orderListDto.Add(Mapper.Map<Order, OrderDto>(order));
                }

                //                var orderListDto = Mapper.Map<Order, OrderDto>(orderist);

                return orderListDto;


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
        public dynamic ViewOrderDetailsOfClientWaitingForBudgetApproval(ViewOrderDetailsOfClientInput[] Input)
        {
            try
            {
                var inputClientId = Input[0].ClientId;

                LoaderAppEntites dbContext = new LoaderAppEntites();
                var orderist = dbContext.Orders.Where(e => e.ClientId == inputClientId && e.OrderStatus == "Waiting For Budget Approval").ToList();

                List<OrderDto> orderListDto = new List<OrderDto>();
                foreach (var order in orderist)
                {
                    orderListDto.Add(Mapper.Map<Order, OrderDto>(order));
                }

                //                var orderListDto = Mapper.Map<Order, OrderDto>(orderist);

                return orderListDto;


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
        public dynamic ViewOrderStatusOfClient(ViewOrderDetailsOfClientInput[] Input)
        {
            try
            {
                var inputClientId = Input[0].ClientId;

                LoaderAppEntites dbContext = new LoaderAppEntites();
                var orderist = dbContext.Orders.Where(e => e.ClientId == inputClientId && (e.OrderStatus == "Accepted" || e.OrderStatus == "Confirmed" || e.OrderStatus == "In Transit" || e.OrderStatus == "Completed") ).ToList();

                List<OrderDto> orderListDto = new List<OrderDto>();
                foreach (var order in orderist)
                {
                    orderListDto.Add(Mapper.Map<Order, OrderDto>(order));
                }

                //                var orderListDto = Mapper.Map<Order, OrderDto>(orderist);

                return orderListDto;


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
                    IsOrderAccepted = true,
                    ErrorException = "null"
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    IsOrderAccepted = false,
                    ErrorException = ex
                };
            }
        }
        [HttpPost]
        public dynamic ViewOrderDetailsOfTransportOwner(ViewOrderDetailsOfTransportOwnerIput [] Input)
        {
            try
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();

                var inputToId = Input[0].TransportOwnerId;
                var check = "Completed";
                var orderist = dbContext.Orders.Where(e => e.TransportOwnerId == inputToId ).ToList();

                List<OrderDto> orderListDto = new List<OrderDto>();
                foreach (var order in orderist)
                {
                    orderListDto.Add(Mapper.Map<Order, OrderDto>(order));
                }

                //                var orderListDto = Mapper.Map<Order, OrderDto>(orderist);

                return orderListDto;


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
        // request parameter an array so that it could be accessed by volley in android as response is a json array
        public dynamic ViewOrderUpdatesOfTransportOwner(ViewOrderDetailsOfTransportOwnerIput[] Input)
        {
            try
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();

                var inputToId = Input[0].TransportOwnerId;
                var orderist = dbContext.Orders.Where(e => e.TransportOwnerId == inputToId && (e.OrderStatus == "Confirmed" || e.OrderStatus == "In Transit" )).ToList();

                List<OrderDto> orderListDto = new List<OrderDto>();
                foreach (var order in orderist)
                {
                    orderListDto.Add(Mapper.Map<Order, OrderDto>(order));
                }


                return orderListDto;


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
        public dynamic changeOrderStatus(AcceptOrDeclineOfferInput input)
        {
            try
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                var quote = dbContext.Quotes.Where(e => e.OrderId == input.OrderId).FirstOrDefault();
                quote.QuoteStatus = input.Status;
                dbContext.SaveChanges();
                return new
                {
                    Message ="Order Status changed Admin will review the changes" 
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Message = "Exception occured due to some reason"
                };
            }
        }

        [HttpPost]
        public dynamic ViewAvailableOrdersToBid(AcceptOrDeclineOfferInput [] Input)
        {
            try
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();
                 var ordersToBid = dbContext.Orders
                    .Where(e => e.OrderStatus=="Pending" || e.OrderStatus == "Rejected")
                    .ToList();
                List<OrderDto> orderListToBidDto = new List<OrderDto>();
                foreach (var order in ordersToBid)
                {
                    orderListToBidDto.Add(Mapper.Map<Order, OrderDto>(order));
                }
                return orderListToBidDto;
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
        public dynamic PostBidToAnOrder(PostBidToAnOrderInput input)
        {

            // comment
            try
            {
                LoaderAppEntites dbContext = new LoaderAppEntites();

                var isAlreadyQuoted = dbContext.Quotes.FirstOrDefault(e => e.OrderId == input.OrderId && e.TransportOwnerId == input.TransportOwnerId);
                var vehicle = dbContext.Vehicles.Where(e => e.UserId == input.TransportOwnerId).FirstOrDefault();



                if (isAlreadyQuoted == null  && vehicle.VehicleIsBooked != true)
                {
                    Quote quote = new Quote();

                    quote.OrderId = input.OrderId;
                    quote.QuoteStatus = "Pending";
                    quote.TransportOwnerId = input.TransportOwnerId;
                    quote.QuoteBudget = input.budget;
                    quote.TransportOwnerName = input.TransportOwnerName;
                    dbContext.Quotes.Add(quote);
                    dbContext.SaveChanges();

                    return new
                    {
                        isQuoteAdded = true,
                        exception = "null",
                        message = "Quote Posted Successfully"
                    };

                }
                else {
                    return new
                    {
                        isQuoteAdded = false,
                        exception = "null",
                        message = "Already Quoted This Order or Vehicle is booked"

                    };

                }

            }
            catch (Exception ex)
            {
                return new
                {
                    isQuoteAdded = false,
                    exception = ex,
                    message = "Server Error"


                };

            }




        }

    }


    // dto's
    public class OrderDto
    {
        public int Id { get; set; }
        public Nullable<int> ClientId { get; set; }
        public Nullable<int> TransportOwnerId { get; set; }
        public string OrderPickup { get; set; }
        public string OrderDropOff { get; set; }
        public string OrderComodity { get; set; }
        public string OrderWeight { get; set; }
        public Nullable<System.DateTime> OrderPickUpDate { get; set; }
        public string OrderLength { get; set; }
        public string OrderWidth { get; set; }
        public string OrderHeight { get; set; }
        public string OrderReceiverName { get; set; }
        public string OrderReceiverCompanyName { get; set; }
        public string OrderAdditionalDetails { get; set; }
        public string OrderStatus { get; set; }
        public string OrderBudget { get; set; }
    }

    public class ViewOrderDetailsOfClientInput
    {
        public int ClientId { get; set; }

    }

    public class PostBidToAnOrderInput
    {
        public int TransportOwnerId { get; set; }
        public int OrderId { get; set; }

        public string budget { get; set; }

        public string TransportOwnerName { get; set; }
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

    public class UserDto
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
    }
}


