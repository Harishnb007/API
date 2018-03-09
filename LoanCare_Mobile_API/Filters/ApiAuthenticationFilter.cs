using Business_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Threading;
using System.Diagnostics;
using Business_Services.Models.Helpers;

namespace LoanCare_Mobile_API.Filters
{
    public class ApiAuthenticationFilter : GenericAuthenticationFilter
    {
        public ApiAuthenticationFilter() { }

        public ApiAuthenticationFilter(bool isActive) : base(isActive) { }

        protected override bool OnAuthorizeUser(string name, string password, HttpActionContext actionContext)
        {
            var provider = new TokenServices();

            if(provider != null)
            {
                Debug.WriteLine("Authenticating credentials - {0} {1}", name, password);
                ResponseWithToken token = provider.AuthenticateAsync(name, password).Result;
                Debug.WriteLine("Token - " + token);

                if(token != null)
                {
                    var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;

                    if (basicAuthenticationIdentity != null)
                        basicAuthenticationIdentity.UserName = name;

                    return true;
                }
            }

            return false;
        }
    }
}