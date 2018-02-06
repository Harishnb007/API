using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Business_Services;
using LoanCare_Mobile_API.Filters;
using LoanCare_Mobile_API.Models;
using LoanCare_Mobile_API.Action_Filters;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using System.Data.Entity;
using Business_Services.Models;

namespace LoanCare_Mobile_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/authenticate")]
    public class AuthenticateController : ApiController
    {
        private readonly ITokenServices _tokenServices;
        private readonly IUserServices _userServices;

        public AuthenticateController()
        {
            _tokenServices = new TokenServices();
            _userServices = new UserServices();
        }


        /// <summary>
        /// Authenticates user and returns token with expiry
        /// </summary>
        /// <returns></returns>
        //[ApiAuthenticationFilter]
        //[HttpPost]
        //public HttpResponseMessage Authenticate(UserCred userCred)
        //{
        //    Debug.WriteLine("Authenticate method has been invoked..");
        //    if (System.Threading.Thread.CurrentPrincipal != null && System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
        //    {
        //        var basicAuthenticationIdentity = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
        //        if (basicAuthenticationIdentity != null)
        //        {
        //            var userId = basicAuthenticationIdentity.UserName;
        //            return GetAuthToken(userId);
        //        }
        //    }
        //    return null;
        //}
        [AcceptVerbs("Post")]
        public async Task<HttpResponseMessage> Authenticate([FromBody]UserCred userCred)
        {
            Debug.WriteLine("Authenticate method has been invoked..");

            //var lcAuthTokenValue = _tokenServices.AuthenticateAsync(userCred.username, userCred.password).Result;
            //return GetAuthToken(userCred.username, lcAuthTokenValue.data.ToString());

            var lcAuthTokenValueTask = _tokenServices.AuthenticateAsync(userCred.username, userCred.password);
            await Task.WhenAll(lcAuthTokenValueTask);

            var GetTokenTask = GetAuthTokenAsync(userCred.username, userCred.password, lcAuthTokenValueTask.Result, userCred.Is_New_MobileUser,userCred.username,userCred.resourcename,userCred.log);
            await Task.WhenAll(GetTokenTask);

            return GetTokenTask.Result;
        }

      

        [AcceptVerbs("Get")]
        [Route("testauth")]
        public async Task<IHttpActionResult> TestAuth()
        {
            Debug.WriteLine("TestAuth() invoked..");
            var response = _tokenServices.TestAuthAsync();
            await Task.WhenAll(response);

            var data = response.Result;

            return Ok(data);
        }
        //[Route("getAlert/{loan_number}")]
        //[HttpGet]
        //[AcceptVerbs("Get")]
        //[Route("getAlert/{loan_number}")]
        //public async Task<IHttpActionResult> GetAlertDetails(string loan_number)
        //{
        //   // LoanService loanService = new LoanService();
        //    var payment = await loanService.GetAlertDetailsAsync(loan_number);
        //    if (payment == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(payment);
        //}


        /// <summary>
        /// Returns auth token for the validated user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> GetAuthTokenAsync(string userId, string Password, string lcAuthToken, bool Is_New_MobileUser, string UserName,string resourcename,string log)
        {
            AuthTokenAndUserDetails Auth_data = new AuthTokenAndUserDetails();
            IUserServices userService = new UserServices();
            try
            {
                var details = userService.getUserDetailsAsync(lcAuthToken, userId);
                await Task.WhenAll(details);
                Business_Services.Models.User userDetails = (Business_Services.Models.User)details.Result.data;
                //Debug.WriteLine(userDetails.first_name);

                var token = _tokenServices.GenerateToken(userId, Password, userDetails.ClientId, lcAuthToken, userDetails.username,resourcename,log);
                List<LoanSummarys> loanS = new List<LoanSummarys>();
                //AuthTokenAndUserDetails Auth_data = new AuthTokenAndUserDetails
                //{
                using (var ctx = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
                {
                    var setpin = ctx.MobileUsers.Where(s => s.User_Id == userId).FirstOrDefault();

                    if (setpin == null)
                    {
                        if (Is_New_MobileUser == false)
                        {

                            Is_New_MobileUser = false;
                        }
                        else if (Is_New_MobileUser == true)
                        {

                            Is_New_MobileUser = true;
                        }
                        using (var context = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
                        {
                            Business_Services.Models.DAL.LoancareDBContext.MobileUser obj_Login = new Business_Services.Models.DAL.LoancareDBContext.MobileUser()
                            {
                                pin = "",
                                User_Id = userId,
                                mae_steps_completed = "0",
                                Mobile_Token_Id = "",
                                created_on = DateTime.Now,
                                Is_New_MobileUser = Is_New_MobileUser,
                                Legal_version = 0,
                                Privacy_version = 0,
                                Terms_version = 0
                            };
                            context.MobileUsers.Add(obj_Login);
                            context.Entry(obj_Login).State = EntityState.Added;
                            context.SaveChanges();
                            Auth_data.mae_steps_completed = "0";
                        }
                    }
                    else if (setpin != null)
                    {
                        Auth_data.mae_steps_completed = setpin.mae_steps_completed;
                    }
                    else if (setpin.mae_steps_completed == "")
                    {
                        setpin.mae_steps_completed = "0";
                    }
                }

                Auth_data.AuthorizationToken = token;
                Auth_data.Expires = DateTime.Now.AddSeconds(Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
                Auth_data.username = userDetails.username;
                Auth_data.setup_status = userDetails.setup_status;
                Auth_data.first_name = userDetails.first_name;
                Auth_data.last_name = userDetails.last_name;
                Auth_data.middle_name = userDetails.middle_name;
                Auth_data.email = userDetails.email;
                Auth_data.id = userDetails.id;
                Auth_data.ssn = userDetails.ssn;
                Auth_data.password = userDetails.password;
                Auth_data.loanNumber = userDetails.loanNumber;
                Auth_data.NotifyEmail = userDetails.NotifyEmail;
                Auth_data.discVer = userDetails.discVer;
                Auth_data.is_successful = userDetails.is_successful;
                Auth_data.address.isForeign = userDetails.addresss.isForeign;
                Auth_data.address.street = userDetails.addresss.street;
                Auth_data.address.city = userDetails.addresss.city;
                Auth_data.address.zipcode = userDetails.addresss.zipcode;
                Auth_data.address.phone = userDetails.addresss.phone;
                Auth_data.address.state = userDetails.addresss.state;
                Auth_data.address.country = userDetails.addresss.country;
                Auth_data.BorrowerName = userDetails.BorrowerName;
                Auth_data.id = userDetails.id;
                Auth_data.ClientId = userDetails.ClientId;
                Auth_data.ClientName = userDetails.ClientName;
                Auth_data.LoginId = userDetails.LoginId;
            
                foreach (var Add in userDetails.loanss)
                {
                    loanS.Add(new LoanSummarys
                    {

                        loan_number = Add.loan_number,
                        property_address = Add.property_address

                    }
                     );

                }
                Auth_data.loans = loanS;
                // To do - Pass the user ID instead of hardcoded value
                // loans = new Business_Services.Models.Loan { first_name = userDetails.first_name, loans = userDetails.loans, last_name = userDetails.last_name,username =userDetails.username }
                // };

                var responsedata = Request.CreateResponse(HttpStatusCode.OK, new ResponseModel
                {
                    status = new Status { CustomErrorCode = 0, Message = "success" },
                    data = Auth_data
                });
                return responsedata;
            }
            catch (Exception Ex)
            {
                //if (lcAuthToken != "Problem occurred trying to validate the user credentials.Please try again.")
                //{
                //    var details = userService.getUserDetailsAsync(lcAuthToken, userId);
                //    await Task.WhenAll(details);
                //    Business_Services.Models.User userDetails = (Business_Services.Models.User)details.Result.data;
                //    //Debug.WriteLine(userDetails.first_name);

                //    var token = _tokenServices.GenerateToken(userId, Password, userDetails.ClientId, lcAuthToken);
                //    List<LoanSummarys> loanS = new List<LoanSummarys>();

                //    using (var ctx = new Business_Services.Models.DAL.LoanCareContext.MDBServices())
                //    {

                //        if (Is_New_MobileUser == false)
                //        {

                //            Is_New_MobileUser = false;
                //        }
                //        else if (Is_New_MobileUser == true)
                //        {

                //            Is_New_MobileUser = true;
                //        }
                //        using (var context = new Business_Services.Models.DAL.LoanCareContext.MDBServices())
                //        {
                //            Business_Services.Models.DAL.LoanCareContext.Mobile_User obj_Login = new Business_Services.Models.DAL.LoanCareContext.Mobile_User()
                //            {
                //                pin = "",
                //                User_Id = userId,
                //                mae_steps_completed = "0",
                //                Mobile_Token_Id = "",
                //                created_on = DateTime.Now,
                //                Is_New_MobileUser = Is_New_MobileUser
                //            };
                //            context.Mobile_User.Add(obj_Login);
                //            context.Entry(obj_Login).State = EntityState.Added;
                //            context.SaveChanges();
                //            Auth_data.mae_steps_completed = "0";
                //        }
                //    }

                //    Auth_data.AuthorizationToken = token;
                //    Auth_data.Expires = DateTime.Now.AddSeconds(Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
                //    Auth_data.username = userDetails.username;
                //    Auth_data.setup_status = userDetails.setup_status;
                //    Auth_data.first_name = userDetails.first_name;
                //    Auth_data.last_name = userDetails.last_name;
                //    Auth_data.middle_name = userDetails.middle_name;
                //    Auth_data.email = userDetails.email;
                //    Auth_data.id = userDetails.id;
                //    Auth_data.ssn = userDetails.ssn;
                //    Auth_data.password = userDetails.password;
                //    Auth_data.loanNumber = userDetails.loanNumber;
                //    Auth_data.NotifyEmail = userDetails.NotifyEmail;
                //    Auth_data.discVer = userDetails.discVer;
                //    Auth_data.is_successful = userDetails.is_successful;
                //    Auth_data.address.isForeign = userDetails.addresss.isForeign;
                //    Auth_data.address.street = userDetails.addresss.street;
                //    Auth_data.address.city = userDetails.addresss.city;
                //    Auth_data.address.zipcode = userDetails.addresss.zipcode;
                //    Auth_data.address.phone = userDetails.addresss.phone;
                //    Auth_data.address.state = userDetails.addresss.state;
                //    Auth_data.address.country = userDetails.addresss.country;


                //    foreach (var Add in userDetails.loanss)
                //    {
                //        loanS.Add(new LoanSummarys
                //        {

                //            loan_number = Add.loan_number,
                //            property_address = Add.property_address

                //        }
                //         );

                //    }
                //    Auth_data.loans = loanS;
                //}
                var responseMobileUser = Request.CreateResponse(HttpStatusCode.OK, new ResponseModel
                {
                    status = new Status { CustomErrorCode = 1, Message = "Problem occurred trying to validate the user credentials.Please try again." },
                    data = Auth_data
                });
                return responseMobileUser;
            }
            var response = Request.CreateResponse(HttpStatusCode.OK, new ResponseModel
            {
                status = new Status { CustomErrorCode = 0, Message = "success" },
                data = Auth_data
            });
            return response;
        }

        /// <summary>
        /// Extend the session
        /// </summary>
        /// <returns></returns>
        [AuthorizationRequired]
        [AcceptVerbs("GET")]
        [Route("ExtendSession")]
        public HttpResponseMessage ExtendSession()
        {
            Debug.WriteLine("ExtendSession called..");
            IEnumerable<string> AuthTokenValues = HttpContext.Current.Request.Headers.GetValues("AuthorizationToken");

            if (AuthTokenValues.FirstOrDefault() != null && _tokenServices.ValidateToken(AuthTokenValues.FirstOrDefault()))
            {
                Debug.WriteLine("Validation successful within ExtendSession..");

                //To do - Write a call to TokenService to pull the username from the token
                // To do - Pass username into the method below
                return GetAuthTokenAsync("23423", "", "", false,"").Result;
            }
            return null;
        }

        [AcceptVerbs("POST")]
        [Route("forgotid")]
        public IHttpActionResult MailUserID(LoanNumberAndSsn userDetails)
        {
            Debug.WriteLine("MailUserID() invoked..");

            return Ok(_userServices.SendUserIdByMail(userDetails.loan_number, userDetails.ssn));
        }

        [AcceptVerbs("POST")]
        [Route("forgotpwd")]
        public IHttpActionResult MailPassword([FromBody] UserCred userCred)
        {
            Debug.WriteLine("MailPassword() invoked..");

            return Ok(_userServices.SendPasswordByMail(userCred.username));
        }
    }
}