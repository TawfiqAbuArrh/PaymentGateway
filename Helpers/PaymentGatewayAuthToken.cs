using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PaymentGateway_Task.Models.DB;
using System;
using System.Linq;

namespace PaymentGateway_Task.Helpers
{
    public class PaymentGatewayAuthToken : ActionFilterAttribute
    {
        private readonly PaymentGatewayContext _db;

        public PaymentGatewayAuthToken()
        {
            this._db = new PaymentGatewayContext();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                if (context == null) throw new ArgumentException("Invalid HttpContext");

                string token = context.HttpContext.Request.Headers["Access-Token"].ToString();

                if (token != null)
                {
                    var record = _db.LoginTokens.Where(a => a.Token == token).SingleOrDefault();

                    if (record != null)
                    {
                        DateTime userExpiryTime = record.TokenExpiration.Value;
                        string recordToken = record.Token;

                        if (DateTime.Compare(userExpiryTime, DateTime.Now) < 0)
                            context.Result = new ContentResult()
                            {
                                Content = "Invalid Token",
                                StatusCode = 401
                            };
                    }
                    else
                        context.Result = new ContentResult()
                        {
                            Content = "Invalid Token",
                            StatusCode = 401
                        };
                }
                else
                {
                    context.Result = new ContentResult()
                    {
                        Content = "Invalid Token",
                        StatusCode = 401
                    };
                }
            }
            catch 
            {
            }
        }
    }
}