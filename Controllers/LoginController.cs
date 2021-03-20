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

        public LoginController(PaymentGatewayContext _db)
        {
            this._db = _db;
        }

        [HttpPost]
        public IActionResult post(Login request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.UserName))
                {
                    throw new ArgumentException("Missing parametter");
                }

                var returnedUser = _db.Users.Where(s => s.UserName.Equals(request.UserName)).SingleOrDefault();

                if (returnedUser.UserTypeId != 1 && !returnedUser.AdminApproval)
                    return Unauthorized(notAuthorized);

                if (returnedUser.UserName.Equals(request.UserName) && returnedUser.Password.Equals(request.Password))
                {
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
                        ResponseResults = token
                    };
                    return Ok(response);
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