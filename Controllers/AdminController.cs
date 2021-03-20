using Microsoft.AspNetCore.Mvc;
using PaymentGateway_Task.Helpers;
using PaymentGateway_Task.Models.API.Response;
using PaymentGateway_Task.Models.DB;
using System.Linq;

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
            ResponseCode = -2,
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
            var AccessToken = Request.Headers["Access-token"].ToString();
            var AdminId = _db.LoginTokens.Where(s => s.Token == AccessToken).Select(p => p.UserId).SingleOrDefault();

            if (_db.Users.Any(s => s.Id == AdminId && s.UserTypeId == 1))
            {
                var user = _db.Users.SingleOrDefault(s => s.Id == UserId && s.UserTypeId == 2);
                if (user != null)
                {
                    var BusniessProfile = _db.BusinessProfile.Where(s => s.UserId == UserId).SingleOrDefault();
                    if (string.IsNullOrEmpty(BusniessProfile.Pdf))
                        return BadRequest(BusniessUserNotComplete);

                    user.AdminApproval = true;
                    _db.SaveChanges();

                    return Ok(new Response
                    {
                        ResponseCode = 0,
                        ResponseMessage = "Approved",
                        ResponseResults = true
                    });
                }
            }

            return Unauthorized(NotAuthorized);
        }
    }
}