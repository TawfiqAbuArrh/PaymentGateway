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
            if (request == null || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.UserName))
            {
                return new UnauthorizedObjectResult(new string("Not Authorized"));
            }

            if (_db.Users.Any(s => s.UserName.Equals(request.UserName) && s.Password.Equals(request.Password)))
                return new OkObjectResult(new { Token = Guid.NewGuid(), Message = "Success Login" });

            return new UnauthorizedObjectResult(new string("Not Authorized"));
        }
    }
}