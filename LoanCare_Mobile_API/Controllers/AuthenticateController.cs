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
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.IO;


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

            dynamic value = lcAuthTokenValueTask.Result;

            var id = ((Business_Services.Models.Helpers.ResponseWithToken)value).errorId;
            var errorMessage = ((Business_Services.Models.Helpers.ResponseWithToken)value).errorMessage;
            var tokenValue = ((Business_Services.Models.Helpers.ResponseWithToken)value).tokenValue;
            var pwdChange = ((Business_Services.Models.Helpers.ResponseWithToken)value).changePassword;
            var authenticateData = ((Business_Services.Models.Helpers.ResponseWithToken)value).authenticateResult;

           
            if (id == 0)
            {
                if (pwdChange == "Y") {
                   // var token = _tokenServices.GenerateToken(userCred.username, userCred.password, 0, authenticateData.objUserInfo.user.password,
               // userCred.username, userCred.resourcename, userCred.log, false, userCred.username);
                //    authenticateData.objUserInfo.user.password = token;
                    var responseChangePwd = Request.CreateResponse(HttpStatusCode.OK, new ResponseModel
                    {
                        status = new Status { CustomErrorCode = 100, Message = "success" },
                        data = authenticateData
                    });

                    return responseChangePwd;
                }
                else
                {
                    var GetTokenTask = GetAuthTokenAsync(userCred.username, userCred.password, tokenValue.ToString(),
                    userCred.Is_New_MobileUser, userCred.username, userCred.resourcename, userCred.log);
                    await Task.WhenAll(GetTokenTask);

                    return GetTokenTask.Result;
                }
            }
            else
            {
                var responseInvalidUser = Request.CreateResponse(HttpStatusCode.OK, new ResponseModel
                {
                    status = new Status { CustomErrorCode = id, Message = errorMessage },
                    data = ""
                });

                return responseInvalidUser;
            }
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
        private async Task<HttpResponseMessage> GetAuthTokenAsync(string userId, string Password, string lcAuthToken, bool Is_New_MobileUser,
            string UserName, string resourcename, string log)
        {
            AuthTokenAndUserDetails Auth_data = new AuthTokenAndUserDetails();
            IUserServices userService = new UserServices();
            try
            {
                var details = userService.getUserDetailsAsync(lcAuthToken, userId, Is_New_MobileUser);
                await Task.WhenAll(details);
                Business_Services.Models.User userDetails = (Business_Services.Models.User)details.Result.data;
                //Debug.WriteLine(userDetails.first_name);

                var token = _tokenServices.GenerateToken(userDetails.username, Password, userDetails.ClientId, lcAuthToken,
                    userDetails.username, resourcename, log, userDetails.is_enrolled, userId);

                var Decryptdata = Decrypt(token);

                dynamic objPassword = JsonConvert.DeserializeObject(Decryptdata);

                List<LoanSummarys> loanS = new List<LoanSummarys>();


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
                Auth_data.LoginId = userDetails.username;
                Auth_data.mae_steps_completed = userDetails.mae_steps_completed;
                Auth_data.SecurityQuestionFlag = userDetails.SecurityQuestionFlag;
                Auth_data.phone_primary_number = userDetails.phone_primary_number_concern;
                Auth_data.phone_secondary_number = userDetails.phone_secondary_number_concern;
                Auth_data.phone_other_1_number = userDetails.phone_other_1_number_concern;
                Auth_data.phone_other_2_number = userDetails.phone_other_2_number_concern;
                Auth_data.phone_other_3_number = userDetails.phone_other_3_number_concern;
                Auth_data.MobileSignedUp = userDetails.MobileSignedUp;

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
                bool checkClientForRollout = false;
                if (checkClientForRollout ) //&& Auth_data.MobileSignedUp == "False")
                {
                    var responsedata = Request.CreateResponse(HttpStatusCode.OK, new ResponseModel
                    {
                        status = new Status { CustomErrorCode = 0, Message = "You are not allowed to login from Mobile App at this time as it is still not enabled for " + Auth_data.ClientName + ". Please use website. " },
                        data = null
                    });

                } else  {
                    var responsedata = Request.CreateResponse(HttpStatusCode.OK, new ResponseModel
                    {
                        status = new Status { CustomErrorCode = 0, Message = "success" },
                        data = Auth_data
                    });
                    return responsedata;
                }
            }
            catch (Exception Ex)
            {

                var responseMobileUser = Request.CreateResponse(HttpStatusCode.OK, new ResponseModel
                {
                    status = new Status { CustomErrorCode = 1, Message = "Problem occurred trying to validate the user credentials. Please try again." },
                    data = Auth_data
                });
                return responseMobileUser;
            }

           
                ResponseModel responseModel = new ResponseModel();
                Status status = new Status();
            bool checkClientForRolloutt = false;
            if (checkClientForRolloutt)// && Auth_data.MobileSignedUp == "False")
            {
                responseModel.data = null;
                status.CustomErrorCode = 1;
                status.Message = "You are not allowed to login from Mobile App at this time as it is still not enabled for " + Auth_data.ClientName + ".Please use website. ";
                responseModel.status = status;
            }
            else {
                responseModel.data = Auth_data;
                status.CustomErrorCode = 1;
                status.Message = "Success";
                responseModel.status = status;
            }
            var responsemMobileflag = responseModel;
            var responsemsg = Request.CreateResponse(responsemMobileflag);
            return responsemsg;

        }

        static string _Pwd = "This_is_just_a_token_text_for_dev";
        // To do - move this to config file

        static byte[] _Salt = new byte[] { 0x45, 0xF1, 0x61, 0x6e, 0x20, 0x00, 0x65, 0x64, 0x76, 0x65, 0x64, 0x03, 0x76 };
        internal static string Decrypt(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_Pwd, _Salt);
            byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }

        private static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = null;
            try
            {
                Rijndael alg = Rijndael.Create();
                alg.Key = Key;
                alg.IV = IV;
                cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(cipherData, 0, cipherData.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
            catch
            {
                return null;
            }
            finally
            {
                cs.Close();
            }
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
                return GetAuthTokenAsync("23423", "", "", false,"","","").Result;
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