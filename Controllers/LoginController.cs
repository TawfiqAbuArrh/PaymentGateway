using Microsoft.AspNetCore.Mvc;
using PaymentGateway_Task.Models.API.Requests;
using PaymentGateway_Task.Models.API.Response;
using PaymentGateway_Task.Models.DB;
using System;
using System.Linq;

namespace PaymentGateway_Task.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly PaymentGatewayContext _db;
        private Response response;

        public LoginController(PaymentGatewayContext _db)
        {
            this._db = _db;
        }

        [HttpPost]
        public IActionResult post(LoginRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.UserName))
                {
                    throw new ArgumentException("Missing parametter");
                }

                var returnedUser = _db.Users.Where(s => s.UserName.Equals(request.UserName)).SingleOrDefault();

                if (returnedUser.UserTypeId != 1 && !returnedUser.AdminApproval)
                    return new UnauthorizedObjectResult(new Response
                    {
                        ResponseCode = -1,
                        ResponseMessage = "Not Authorized",
                        ResponseResults = false
                    });

                if (returnedUser.UserName.Equals(request.UserName) && returnedUser.Password.Equals(request.Password))
                {
                    if (returnedUser.UserTypeId == 2)
                    {
                        var query = from user in _db.Users
                                    join business in _db.BusinessProfile on user.Id equals business.UserId
                                    where user.Id == business.UserId
                                    select new { profile = business };              

                        if (string.IsNullOrEmpty(query.SingleOrDefault().profile.Pdf))
                            return new UnauthorizedObjectResult(new Response
                            {
                                ResponseCode = -1,
                                ResponseMessage = "Please submit your business certificate",
                                ResponseResults = false
                            });
                    }

                    var token = Guid.NewGuid();
                    _db.LoginTokens.Add(new LoginTokens
                    {
                        Token = token.ToString(),
                        TokenExpiration = DateTime.Now.AddMonths(1),
                        UserId = returnedUser.Id
                    });
                    _db.SaveChanges();

                    response = new Response
                    {
                        ResponseCode = 0,
                        ResponseMessage = "Success Login",
                        ResponseResults = true
                    };
                    return new OkObjectResult(response);
                }

                return new UnauthorizedObjectResult(new Response
                {
                    ResponseCode = -1,
                    ResponseMessage = "Not Authorized",
                    ResponseResults = false
                });
            }
            catch (ArgumentException e)
            {
                return new BadRequestObjectResult(new Response
                {
                    ResponseCode = -2,
                    ResponseMessage = e.Message,
                    ResponseResults = false
                });
            }
            catch
            {
                return new BadRequestObjectResult(new Response
                {
                    ResponseCode = -2,
                    ResponseMessage = "Bad Request",
                    ResponseResults = false
                });
            }
        }
    }
}