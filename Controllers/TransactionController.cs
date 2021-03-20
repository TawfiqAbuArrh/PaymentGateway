using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentGateway_Task.Helpers;
using PaymentGateway_Task.Intrerfaces;
using PaymentGateway_Task.Models.API.Requests;
using PaymentGateway_Task.Models.API.Response;
using PaymentGateway_Task.Models.DB;
using System;
using System.Linq;

namespace PaymentGateway_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [PaymentGatewayAuthToken]
    public class TransactionController : ControllerBase
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

        private Response badRequest = new Response
        {
            ResponseCode = -2,
            ResponseMessage = "Bad Request",
            ResponseResults = false
        };

        private Response maximumAllowance = new Response
        {
            ResponseCode = 1,
            ResponseMessage = "User Exceed the maximum allowance",
            ResponseResults = false
        };

        private Response NotEnoughBalance = new Response
        {
            ResponseCode = 2,
            ResponseMessage = "Not enough balance",
            ResponseResults = false
        };

        public TransactionController(PaymentGatewayContext db, IUserTransaction userTransaction)
        {
            _db = db;
            this.userTransaction = userTransaction;
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddTransaction(Models.API.Requests.Transaction request)
        {
            try
            {
                if (request == null || !decimal.TryParse(request.TransactionAmount, out _) || (decimal.Parse(request.TransactionAmount) < 0) ||
                    string.IsNullOrEmpty(request.TransactionName))
                    throw new ArgumentException("Missing Parameter");

                var AccessToken = Request.Headers["Access-token"].ToString();

                var UserID = _db.LoginTokens.Where(s => s.Token == AccessToken).Select(p => p.UserId).SingleOrDefault();
                if (UserID != null)
                {
                    var user = _db.Users.Where(s => s.Id == UserID && s.UserTypeId != 1 && s.AdminApproval).SingleOrDefault();
                    if (user != null)
                    {
                        if (userTransaction.addTransaction(user, request)) {
                            response = new Response
                            {
                                ResponseCode = 0,
                                ResponseMessage = "Transaction added successfully",
                                ResponseResults = true
                            };
                            return Ok(response);
                        }
                        return Ok(NotEnoughBalance);
                    }
                }
                return Unauthorized(notAuthorized);
            }
            catch (ArgumentException e)
            {
                badRequest.ResponseMessage = e.Message;
                return BadRequest(badRequest);
            }
            catch
            {
                return BadRequest(badRequest);
            }
        }

        [HttpPost]
        [Route("AddBalance")]
        [PaymentGatewayAuthToken]
        public IActionResult AddBalance(Amount request)
        {
            try
            {
                if (request == null || !decimal.TryParse(request.amount, out _) || (decimal.Parse(request.amount) <= 0))
                    throw new ArgumentException("Missing Parameter");

                var AccessToken = Request.Headers["Access-token"].ToString();

                var UserID = _db.LoginTokens.Where(s => s.Token == AccessToken).Select(p => p.UserId).SingleOrDefault();
                if (UserID != null)
                {
                    var user = _db.Users.Where(s => s.Id == UserID && s.UserTypeId != 1 && s.AdminApproval).Include(s => s.UserType).SingleOrDefault();
                    if (user != null)
                    {
                        if (userTransaction.AddBalance(user, decimal.Parse(request.amount)))
                        {
                            return Ok(new Response
                            {
                                ResponseCode = 0,
                                ResponseMessage = "Balance Added: " + request.amount,
                                ResponseResults = "New Balance: " + user.CreditBalance
                            });
                        }
                    }
                }
                return Unauthorized(notAuthorized);
            }
            catch (ArgumentException e)
            {
                badRequest.ResponseMessage = e.Message;
                return BadRequest(badRequest);
            }
            catch
            {
                return BadRequest(badRequest);
            }
        }

        [HttpPost]
        [Route("Withdraw")]
        [PaymentGatewayAuthToken]
        public IActionResult WithdrawTransaction(Amount request)
        {
            try
            {
                if (request == null || !decimal.TryParse(request.amount, out _) || (decimal.Parse(request.amount) <= 0))
                    throw new ArgumentException("Missing Parameter");

                var AccessToken = Request.Headers["Access-token"].ToString();

                var UserID = _db.LoginTokens.Where(s => s.Token == AccessToken).Select(p => p.UserId).SingleOrDefault();
                if (UserID != null)
                {
                    var user = _db.Users.Where(s => s.Id == UserID && s.UserTypeId != 1 && s.AdminApproval).Include(s => s.UserType).SingleOrDefault();
                    if (user != null)
                    {
                        if (userTransaction.isUserExceedTheMaximumAllowance(user.Id, decimal.Parse(request.amount)))
                            return Ok(maximumAllowance);

                        if (userTransaction.withdrawTransaction(user, decimal.Parse(request.amount)))
                        {
                            return Ok(new Response
                            {
                                ResponseCode = 0,
                                ResponseMessage = "Withdraw Completed, Withdraw amount: " + request.amount,
                                ResponseResults = "New Balance: " + user.CreditBalance
                            });
                        }
                        return Ok(NotEnoughBalance);
                    }
                }
                return Unauthorized(notAuthorized);
            }
            catch (ArgumentException e)
            {
                badRequest.ResponseMessage = e.Message;
                return BadRequest(badRequest);
            }
            catch
            {
                return BadRequest(badRequest);
            }
        }

        [HttpPost]
        [Route("Refund")]
        public IActionResult RefundTransaction(Models.API.Requests.Transaction request)
        {
            try 
            {
                if (request == null || string.IsNullOrEmpty(request.TransactionId))
                    throw new ArgumentException("Missing Parameter");

                var AccessToken = Request.Headers["Access-token"].ToString();

                var UserID = _db.LoginTokens.Where(s => s.Token == AccessToken).Select(p => p.UserId).SingleOrDefault();
                if (UserID != null)
                {
                    var user = _db.Users.Where(s => s.Id == UserID && s.UserTypeId != 1 && s.AdminApproval).SingleOrDefault();
                    if (user != null)
                    {
                        var transaction = _db.Transaction.Where(s => s.UserId == user.Id && s.TransactionId == request.TransactionId
                                        && s.TransactionTypeId == 1).SingleOrDefault();
                        if (userTransaction.isUserExceedTheMaximumAllowance(user.Id, transaction.TransactionAmount))
                            return Ok(maximumAllowance);

                        if (userTransaction.refundTransaction(user, transaction))
                        {
                            response = new Response
                            {
                                ResponseCode = 0,
                                ResponseMessage = "Refund Process Completed",
                                ResponseResults = true
                            };
                            return Ok(response);
                        }
                        return BadRequest(badRequest);
                    }
                }
                return Unauthorized(notAuthorized);
            }
            catch (ArgumentException e)
            {
                badRequest.ResponseMessage = e.Message;
                return BadRequest(badRequest);
            }
            catch
            {
                return BadRequest(badRequest);
            }
        }
    }
}