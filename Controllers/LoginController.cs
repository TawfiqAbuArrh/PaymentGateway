using Microsoft.AspNetCore.Mvc;
using PaymentGateway_Task.Models.API.Requests;
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

                if (_db.Users.Any(s => s.UserName.Equals(request.UserName) && s.Password.Equals(request.Password)))
                {
                    var token = Guid.NewGuid();
                    _db.LoginTokens.Add(new LoginTokens
                    {
                        Token = token.ToString(),
                        TokenExpiration = DateTime.Now.AddMonths(1),
                        UserId = _db.Users.Where(s => s.UserName == request.UserName).Select(s => s.Id).Single()
                    });
                    _db.SaveChanges();
                    return new OkObjectResult(new { Token = token, Message = "Success Login" });
                }

                return new UnauthorizedObjectResult(new string("Not Authorized"));
            }
            catch
            {
                return new BadRequestObjectResult(new string("Bad Request"));
            }
        }
    }
}