using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentGateway_Task.Helpers;
using PaymentGateway_Task.Intrerfaces;
using PaymentGateway_Task.Models.API.Requests;
using PaymentGateway_Task.Models.API.Response;
using PaymentGateway_Task.Models.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly PaymentGatewayContext _db;
        private readonly IUserTransaction userTransaction;
        private Response response;

        private Response notAuthorized = new Response
        {
            ResponseCode = -1,
            ResponseMessage = "Not Authorized",
            ResponseResults = false
        };

        private Response conflict = new Response
        {
            ResponseCode = -1,
            ResponseMessage = "Conflict user",
            ResponseResults = false
        };

        private Response badRequest = new Response
        {
            ResponseCode = -2,
            ResponseMessage = "Bad Request",
            ResponseResults = false
        };

        public UsersController(PaymentGatewayContext _db, IUserTransaction userTransaction)
        {
            this._db = _db;
            this.userTransaction = userTransaction;
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(User request)
        {
            try
            {
                if (request == null || !double.TryParse(request.CreditBalance, out _) || string.IsNullOrEmpty(request.Password) ||
                    string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.UserType) || !int.TryParse(request.UserType, out _))
                    throw new ArgumentException("Missing Parameter");

                if (int.Parse(request.UserType) == 2 && (string.IsNullOrEmpty(request.ContactName) || string.IsNullOrEmpty(request.ContactPhone)
                    || string.IsNullOrEmpty(request.BusinessType) || !int.TryParse(request.UserType, out _)))
                    throw new ArgumentException("Missing Parameter, User Type is Business, you must send Contact Name, Contact Phone");

                if (_db.Users.Any(s => s.UserName == request.UserName))
                    return Conflict(conflict);

                if (request.UserType.Equals("1"))
                    return Unauthorized(notAuthorized);

                var NewUser = new Users
                {
                    UserName = request.UserName,
                    Password = request.Password,
                    UserTypeId = int.Parse(request.UserType),
                    CreditBalance = decimal.Parse(request.CreditBalance)
                };

                _db.Users.Add(NewUser);
                if (NewUser.UserTypeId == 2)
                    _db.BusinessProfile.Add(new BusinessProfile
                    {
                        ContactName = request.ContactName,
                        ContactPhone = request.ContactPhone,
                        BusinessTypeId = int.Parse(request.BusinessType),
                        User = NewUser
                    });

                _db.SaveChanges();

                response = new Response
                {
                    ResponseCode = 0,
                    ResponseMessage = "User Created",
                    ResponseResults = "Id: " + NewUser.Id
                };
                return Ok(response);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new Response
                {
                    ResponseCode = -2,
                    ResponseMessage = e.Message,
                    ResponseResults = false
                });
            }
            catch
            {
                return BadRequest(new Response
                {
                    ResponseCode = -2,
                    ResponseMessage = "Bad Request",
                    ResponseResults = false
                });
            }
        }

        [HttpPost]
        [Route("UploadPDF")]
        [PaymentGatewayAuthToken]
        public async Task<IActionResult> uploadPDF(IFormFile pdf)
        {
            try
            {
                if (pdf == null || pdf.ContentType != "application/pdf")
                    return BadRequest(badRequest);

                var AccessToken = Request.Headers["Access-token"].ToString();

                var UserID = _db.LoginTokens.Where(s => s.Token == AccessToken).Select(p => p.UserId).SingleOrDefault();
                if (UserID != null)
                {
                    var User = _db.Users.Where(s => s.Id == UserID && s.UserTypeId == 2).SingleOrDefault();
                    if (User != null)
                    {
                        var businessProfile = _db.BusinessProfile.Where(s => s.UserId == User.Id).SingleOrDefault();
                        if (pdf.Length > 0)
                        {
                            using (var stream = new MemoryStream())
                            {
                                await pdf.CopyToAsync(stream);
                                var fileArray = stream.ToArray();

                                businessProfile.Pdf = Convert.ToBase64String(fileArray);
                                businessProfile.PdfName = User.UserName + "_BusinessCertificate";
                                _db.SaveChanges();

                                response = new Response
                                {
                                    ResponseCode = 0,
                                    ResponseMessage = "PDF Uploaded",
                                    ResponseResults = "Id: " + User.Id
                                };
                                return Ok(response);
                            }
                        }
                    }
                }
                return Unauthorized(notAuthorized);
            }
            catch
            {
                return BadRequest(badRequest);
            }
        }

        [HttpGet]
        [Route("GetBusniessPDF")]
        [PaymentGatewayAuthToken]
        public IActionResult getPDF()
        {
            var AccessToken = Request.Headers["Access-token"].ToString();

            var UserID = _db.LoginTokens.Where(s => s.Token == AccessToken).Select(p => p.UserId).SingleOrDefault();
            if (UserID != null)
            {
                var businessProfile = _db.BusinessProfile.Where(s => s.UserId == UserID).SingleOrDefault();
                if (businessProfile != null)
                {
                    byte[] bytes = Convert.FromBase64String(businessProfile.Pdf);
                    return File(bytes, "application/pdf");
                }
            }
            return Unauthorized(notAuthorized);
        }

        [HttpGet]
        [Route("Details")]
        [PaymentGatewayAuthToken]
        public IActionResult getUserDetails()
        {
            var AccessToken = Request.Headers["Access-token"].ToString();

            var UserID = _db.LoginTokens.Where(s => s.Token == AccessToken).Select(p => p.UserId).SingleOrDefault();
            if (UserID != null)
            {
                var user = _db.Users.Where(s => s.Id == UserID && s.UserTypeId != 1).Include(s => s.UserType).SingleOrDefault();
                if (user != null)
                {
                    BusinessProfile businessProfile = null;
                    if (user.UserTypeId == 2)
                        businessProfile = _db.BusinessProfile.Where(s => s.UserId == UserID).Include(s => s.BusinessType).SingleOrDefault();
                    var transactions = userTransaction.getUserTransaction((int)UserID);

                    var AllUserTransActions = new List<TransactionResponse>();
                    foreach (var item in transactions)
                    {
                        AllUserTransActions.Add(new TransactionResponse
                        {
                            TransactionAmount = item.TransactionAmount,
                            TransactionId = item.TransactionId,
                            TransactionName = item.TransactionName,
                            TransactionType = item.TransactionType.TransactionTypeName
                        });
                    }

                    var userDetails = new UserDetails
                    {
                        user = new UserResponse
                        {
                            UserName = user.UserName,
                            UserType = user.UserType.Name,
                            CreditBalance = user.CreditBalance,
                            BusinessType = user.UserTypeId == 2 ? businessProfile.BusinessType.BusinessName : null,
                            ContactName = user.UserTypeId == 2 ? businessProfile.ContactName : null,
                            ContactPhone = user.UserTypeId == 2 ? businessProfile.ContactPhone : null,
                            PDFName = user.UserTypeId == 2 ? businessProfile.PdfName : null,
                        },
                        transactions = AllUserTransActions
                    };
                    return Ok(new Response
                    {
                        ResponseCode = 0,
                        ResponseMessage = "Success",
                        ResponseResults = userDetails
                    });
                }
                return BadRequest(badRequest);
            }
            return Unauthorized(notAuthorized);
        }

        [HttpGet]
        [Route("Transactions")]
        [PaymentGatewayAuthToken]
        public IActionResult getUserTransactions()
        {
            var AccessToken = Request.Headers["Access-token"].ToString();

            var UserID = _db.LoginTokens.Where(s => s.Token == AccessToken).Select(p => p.UserId).SingleOrDefault();
            if (UserID != null)
            {
                var user = _db.Users.Where(s => s.Id == UserID && s.UserTypeId != 1).Include(s => s.UserType).SingleOrDefault();
                if (user != null)
                {
                    var transactions = userTransaction.getUserTransaction((int)UserID);

                    var AllUserTransActions = new List<TransactionResponse>();
                    foreach (var item in transactions)
                    {
                        AllUserTransActions.Add(new TransactionResponse
                        {
                            TransactionAmount = item.TransactionAmount,
                            TransactionId = item.TransactionId,
                            TransactionName = item.TransactionName,
                            TransactionType = item.TransactionType.TransactionTypeName
                        });
                    }
                    return Ok(new Response 
                    {
                        ResponseCode = 0,
                        ResponseMessage = "Success",
                        ResponseResults = AllUserTransActions
                    });
                }
                return BadRequest(badRequest);
            }
            return Unauthorized(notAuthorized);
        }
    }
}