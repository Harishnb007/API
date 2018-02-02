using LoanCare_Mobile_API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace LoanCare_Mobile_API.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class GenericAuthenticationFilter : AuthorizationFilterAttribute
    {
        /// <summary>
        /// Public default Constructor
        /// </summary>
        public GenericAuthenticationFilter()
        {

        }

        private readonly bool _isActive = true;

        /// <summary>
        /// parameter isActive explicitly enables/disables this filter
        /// </summary>
        /// <param name="isActive"></param>
        public GenericAuthenticationFilter(bool isActive)
        {
            _isActive = isActive;
        }

        /// <summary>
        /// Checks basic authentication request
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!_isActive) return;
            
            var identity = FetchAuthHeader(actionContext);
            if(identity == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, new ResponseModel
                {
                    status = new Status { CustomErrorCode = 1, Message = "Invalid username or password!" },
                    data = null
                });
                return;
            }

            var genericPrincipal = new GenericPrincipal(identity, null);
            Thread.CurrentPrincipal = genericPrincipal;

            if(!OnAuthorizeUser(identity.Name, identity.Password, actionContext))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, new ResponseModel {
                    status = new Status { CustomErrorCode = 1, Message = "Invalid username or password!"},
                    data = null
                });
                return;
            }

            base.OnAuthorization(actionContext);
        }

        /// <summary>
        /// Send the authentication challenge request
        /// </summary>
        /// <param name="actionContext"></param>
        private static void ChallengeAuthRequest(HttpActionContext actionContext)
        {
            var dnsHost = actionContext.Request.RequestUri.DnsSafeHost;
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", dnsHost));
        }

        protected virtual BasicAuthenticationIdentity FetchAuthHeader(HttpActionContext actionContext)
        {
            string authHeaderValue = null;
            Task<string> authRequest = actionContext.Request.Content.ReadAsStringAsync();
            Debug.WriteLine("Request received is " + authRequest.Result);

            if (authRequest != null)
                authHeaderValue = authRequest.Result;

            if (authHeaderValue == null)
                return null;

            //authHeaderValue = Encoding.Default.GetString(Convert.FromBase64String(authHeaderValue));

            try
            {
                var credentials = JsonConvert.DeserializeObject<UserCred>(authHeaderValue);
                return credentials == null ? null : new BasicAuthenticationIdentity(credentials.username, credentials.password);
            }
            catch (JsonException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Can be overriden to perform custom authorization
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected virtual bool OnAuthorizeUser(string name, string password, HttpActionContext actionContext)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
                return false;

            return true;
        }


    }
}