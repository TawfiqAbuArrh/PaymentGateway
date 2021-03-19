using Microsoft.AspNetCore.Mvc;
using PaymentGateway_Task.Models.API.Requests;
using PaymentGateway_Task.Models.DB;
using System;
using System.Linq;

namespace PaymentGateway_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly PaymentGatewayContext _db;

        public UsersController(PaymentGatewayContext _db)
        {
            this._db = _db;
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(User request)
        {
            try
            {
                if (request == null || !double.TryParse(request.CreditBalance, out _)|| string.IsNullOrEmpty(request.Password) || 
                    string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.UserType) || !int.TryParse(request.UserType, out _))
                    throw new ArgumentException("Missing Parameter");

                if (int.Parse(request.UserType) == 2 && (string.IsNullOrEmpty(request.ContactName) || string.IsNullOrEmpty(request.ContactPhone)
                    || string.IsNullOrEmpty(request.BusinessType) || !int.TryParse(request.UserType, out _)))
                    throw new ArgumentException("Missing Parameter, User Type is Business, you must send Contact Name, Contact Phone");

                if (_db.Users.Any(s => s.UserName == request.UserName))
                    return new ObjectResult("Conflict")
                    {
                        StatusCode = 409,
                        Value = "Conflict user"
                    };

                if (request.UserType.Equals("1"))
                    return new UnauthorizedObjectResult("Not Authorized");


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
                return new OkObjectResult($"Created, UserId = {NewUser.Id}");
            }
            catch (ArgumentException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult("Bad Request");
            }
        }
    }
}