using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Business_Services;

namespace LoanCare_Mobile_API.Action_Filters
{
    public class AuthorizationRequiredAttribute : ActionFilterAttribute
    {
        private const string Token = "AuthorizationToken";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            IEnumerable<string> AuthTokenValues;

            if(actionContext.Request.Headers.TryGetValues(Token, out AuthTokenValues))
            {
                // To do - Use dependency injection
                TokenServices tokenService = new TokenServices();

                // Validate Token
                if(!tokenService.ValidateToken(AuthTokenValues.FirstOrDefault()))
                {
                    var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
                    {
                        ReasonPhrase = "Invalid Request"
                    };

                    Debug.Write("Request rejected since the token cookie is invalid!");
                    actionContext.Response = responseMessage;
                }
            }
            else
            {
                Debug.Write("Request rejected since the token cookie is missing!");
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }


            base.OnActionExecuting(actionContext);
        }
    }
}