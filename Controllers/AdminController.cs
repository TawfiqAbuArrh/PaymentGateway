using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway_Task.Helpers;
using PaymentGateway_Task.Models.API.Response;
using PaymentGateway_Task.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [PaymentGatewayAuthToken]
    public class AdminController : ControllerBase
    {
        private readonly PaymentGatewayContext _db;

        private Response NotAuthorized = new Response
        {
            ResponseCode = -1,
            ResponseMessage = "Not Authorized",
            ResponseResults = false
        };

        private Response BusniessUserNotComplete = new Response
        {
            ResponseCode = -1,
            ResponseMessage = "Failed, business profile not completed, missing business certificate",
            ResponseResults = false
        };

        public AdminController(PaymentGatewayContext _db)
        {
            this._db = _db;
        }

        [HttpGet]
        [Route("Approval")]
        public IActionResult approval(int UserId)
        {
            var AccessToken = Request.Headers["Access-token"];
            if (_db.LoginTokens.Join(_db.Users, L => L.UserId, U => U.Id, (L, U) => new { L, U }).Any(LU => LU.L.UserId == LU.U.Id && LU.U.UserTypeId == 1))
            {
                var user = _db.Users.FirstOrDefault(s => s.Id == UserId && s.UserTypeId != 1);
                if (user != null)
                {
                    if (user.UserTypeId == 2)
                    {
                        var query = from userTable in _db.Users
                                    join businessTable in _db.BusinessProfile on userTable.Id equals businessTable.UserId
                                    where userTable.Id == businessTable.UserId
                                    select new { profile = businessTable };

                        if (string.IsNullOrEmpty(query.SingleOrDefault().profile.Pdf))
                            return new UnauthorizedObjectResult(BusniessUserNotComplete);
                    }

                    user.AdminApproval = true;
                    _db.SaveChanges();

                    return new OkObjectResult(new Response
                    {
                        ResponseCode = 0,
                        ResponseMessage = "Approved",
                        ResponseResults = true
                    });
                }
            }

            return new UnauthorizedObjectResult(NotAuthorized);
        }
    }
}
