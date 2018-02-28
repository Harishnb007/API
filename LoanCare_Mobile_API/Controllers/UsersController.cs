using Business_Services;
using Business_Services.Models;
using LoanCare_Mobile_API.Action_Filters;
using LoanCare_Mobile_API.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Business_Services.Models.DAL.LoancareEntites;
using Business_Services.Models.DAL;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace LoanCare_Mobile_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    //[AuthorizationRequired]
    [RoutePrefix("api/user")]
    public class UsersController : ApiController
    {
        private readonly IUserServices userService;

        public UsersController()
        {
            userService = new UserServices();
        }

        [Route("{userId}")]
        [AcceptVerbs("GET")]
        //public IHttpActionResult GetUserDetails(string userId)
        //{
        //    return Ok(userService.getUserDetailsAsync("").Result);
        //}

        public async Task<IHttpActionResult> GetUserDetails(string userId)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }



            // var payment = await userService.UpdatePasswordAsync(tokenValue, loanDetails, objPasswordUpd);

            var payment = await userService.getUserDetailsAsyn(tokenValue, userId);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("PushNotification")]
        [HttpPost]
        public async Task<IHttpActionResult> PushNotification(PushNotificationUser pushNotification)
        {
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await userService.GetPushNotificationForUser(tokenValue, pushNotification);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("MyLoanSetPin/{Pin}")]
        [HttpGet]
        public async Task<IHttpActionResult> myloanGetPin(string Pin)
        {
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await userService.myloanGetPinAsyn(tokenValue,Pin);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }



        //[Route("MyLoanSetpin/{LoanNumber}/{pin}")]
        //[HttpGet]
        //public IHttpActionResult Getpin(string LoanNumber, string pin)
        //{
        //    // To do - Move the following code to a single method & use it across the project
        //    IEnumerable<string> tokenValues;
        //    string tokenValue = "";
        //    if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
        //    {
        //        tokenValue = tokenValues.FirstOrDefault();
        //    }
        //    var payment = userService.myloanGetPinAsyn(tokenValue, pin);
        //    if (payment == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(payment);
        //}


        [Route("bankaccountsDelete")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteBankAccounts(BankAccount objBank)
        {
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await userService.DeleteBankAccountsForUser(tokenValue, objBank);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

       
        [Route("contactus")]
        [HttpGet]
        public async Task<IHttpActionResult> Contactus()
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await userService.ContactUsAsync(tokenValue);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("bankaccounts")]
        [HttpGet]
        public async Task<IHttpActionResult> GetBankAccounts()
        {
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await userService.GetBankAccountsForUser(tokenValue);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("ManageAccount/{LoanNumber}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetManageAccount(string LoanNumber)
        {
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await userService.GetManageAccountForUser(tokenValue, LoanNumber);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }    

        //[Route("Setpin")]
        //[HttpPut]
        //public  IHttpActionResult Postsetpin(UpdatePassword PinDetail)
        //{
        //    // To do - Move the following code to a single method & use it across the project
        //    IEnumerable<string> tokenValues;
        //    string tokenValue = "";
        //    if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
        //    {
        //        tokenValue = tokenValues.FirstOrDefault();
        //    }
        //    var payment =  userService.PostsetpinAsync(PinDetail,tokenValue);
        //    if (payment == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(payment);
        //}

        //[Route("Resetpin")]
        //[HttpPost]
        //public IHttpActionResult PostResetpin(UsersMDb userDetail)
        //{
        //    // To do - Move the following code to a single method & use it across the project
        //    IEnumerable<string> tokenValues;
        //    string tokenValue = "";
        //    if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
        //    {
        //        tokenValue = tokenValues.FirstOrDefault();
        //    }
        //    var payment = userService.ResetpinAsync(userDetail);
        //    if (payment == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(payment);
        //}

        [Route("SetPin")]
        [HttpPut]
        public async Task<IHttpActionResult> SetpinPost(UpdatePassword PinDetails)
        {

            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
           
            var payment = await userService.SetPinAsync(tokenValue, PinDetails);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("ReSetPin")]
        [HttpPut]
        public async Task<IHttpActionResult> ReSetpinPost(UpdatePassword PinDetails)
        {

            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var payment = await userService.ReSetPinAsync(tokenValue, PinDetails);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }




        [Route("updatepwd")]
        [HttpPut]
        public async Task<IHttpActionResult> updatepassword(UpdatePassword loanDetails)
        {
            
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            
            var Decryptdata = Decrypt(tokenValue);

            dynamic objPassword = JsonConvert.DeserializeObject(Decryptdata);

            string objPasswordUpd = objPassword.Password;

            var payment = await userService.UpdatePasswordAsync(tokenValue, loanDetails, objPasswordUpd);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
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

        [Route("updateprofile/{user_id}")]
        [HttpPut]
        public IHttpActionResult UpdateProfile(string user_id, [FromBody] Business_Services.Models.User user)
        {
            return Ok(userService.UpdateProfile(user_id, user));
        }

        //Added by BBSR_Team on 23rd Dec 2017
        [Route("securityquestion")]
        [AcceptVerbs("GET")]

        public async Task<IHttpActionResult> securityquestions()
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var question_dtls = await userService.GetSecurityQuestions(tokenValue);
            if (question_dtls == null)
            {
                return NotFound();
            }
            return Ok(question_dtls);
        }

        //[Route("securityquestions/{user_id}")]
        //[HttpPut]
        //public IHttpActionResult UpdateSecurityAnswers(string user_id, [FromBody] List<SecurityQuestion> answers)
        //{
        //    return Ok(userService.UpdateSecurityAnswers(user_id, answers));
        //}

        [Route("privacy/{user_id}")]
        [HttpGet]
        public IHttpActionResult GetPrivacyText(string user_id)
        {
            return Ok(userService.GetPrivacyForUser(user_id));
        }

        [Route("updatesecurityquestion")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateSecurityAnswers([FromBody]List<QuestionSummary> questionSummary)
        {
            //Debug.WriteLine("Authenticate method has been invoked..");    

            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var responsedata = await userService.UpdateSecurityAnswers(tokenValue, questionSummary);

            if (responsedata != null)
            {
                return Ok(responsedata);
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [Route("InsertSecurityAnswer")]
        [HttpPost]
        public async Task<IHttpActionResult> InsertSecurityAnswer([FromBody]List<QuestionSummary> questionSummary)
        {
            //Debug.WriteLine("Authenticate method has been invoked..");    

            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }


            var Decryptdata = Decrypt(tokenValue);

            dynamic ObjUserId = JsonConvert.DeserializeObject(Decryptdata);
            string objUserIdUpd = ObjUserId.UserId;

            var responsedata = await userService.InsertSecurityAnswerAsyn(tokenValue, questionSummary, objUserIdUpd);

            if (responsedata != null)
            {
                return Ok(responsedata);
            }
            else
            {
                return BadRequest("Error!");
            }
        }

       


        //Added by BBSR_Team on 9th Jan 2018




        //Modified by BBSR Team on 16th Jan 2018 : User Registration

        [Route("getregister")]
        [HttpPost]
        public async Task<IHttpActionResult> UserRegistration(Business_Services.Models.User userDetail)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await userService.PostUserRegistrationAsync(userDetail);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }


        [Route("getconfirmation")]
        [HttpPost]
        public async Task<IHttpActionResult> GetConfirmation(Business_Services.Models.User userDetail)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await userService.GetConfirmationAsync(userDetail);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("setregister")]
        [HttpPost]
        public async Task<IHttpActionResult> SetRegister(Business_Services.Models.User userDetail)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await userService.PostsetRegistrationAsync(userDetail);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        //Modified by BBSR Team on 16th Jan 2018 : User Registration


        [Route("ForgotUser")]
        [HttpPost]
        public async Task<IHttpActionResult> ForgotUserId(Business_Services.Models.User userDetail)
        {
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var Forgot_UserId = await userService.ForgotUser_Id(userDetail);

            if (Forgot_UserId == null)
            {
                return NotFound();
            }
            return Ok(Forgot_UserId);
        }

        [Route("ForgotPassword")]
        [HttpPost]
        public async Task<IHttpActionResult> ForgotPassword(Business_Services.Models.User userDetail)
        {
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var Forgot_Password = await userService.ForgotPassword(userDetail);

            if (Forgot_Password == null)
            {
                return NotFound();
            }
            return Ok(Forgot_Password);
        }

        [Route("ValidateSecurityAnswer")]
        [HttpPost]
        public async Task<IHttpActionResult> ValidateSecurityAnswer(Business_Services.Models.User userDetail)
        {
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var Forgot_Password = await userService.ValidateSecurityAnswer(userDetail);

            if (Forgot_Password == null)
            {
                return NotFound();
            }
            return Ok(Forgot_Password);
        }

        [Route("ResetSendPassword")]
        [HttpPost]
        public async Task<IHttpActionResult> ResetSendPassword(Business_Services.Models.User userDetail)
        {
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var Forgot_Password = await userService.ResetSendPassword(userDetail);

            if (Forgot_Password == null)
            {
                return NotFound();
            }
            return Ok(Forgot_Password);
        }
    }
}
