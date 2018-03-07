using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Services.Models;
using Business_Services.Models.Helpers;
using Business_Services.B2C_WebAPI;
using Newtonsoft.Json;
using Business_Services.Models.LC_WebApi_Models;
using System.Net.Http;
using Business_Services.Models.DAL.LoancareEntites;
using Business_Services.Models.DAL;
using System.Data.Entity;
using System.Web;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;
// business service comment
namespace Business_Services
{
    public class UserServices : IUserServices
    {
        private Business_Services.Models.User user;
        public async Task<ResponseModel> GetPushNotificationForUser(string mobileToken, PushNotificationUser pushNotification)
        {
            try
            {
                PushNotificationUser ObjpushNotificationUser = new PushNotificationUser();

                ObjpushNotificationUser.PaymentAlerts = false;
                ObjpushNotificationUser.PushNotification = false;
                using (var ctx = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
                {

                    var PushNotificationUser = ctx.MobileUsers.Where(s => s.User_Id == pushNotification.Login_Id).FirstOrDefault();
                    if (PushNotificationUser != null)
                    {
                        using (var context = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
                        {

                            PushNotificationUser.mae_steps_completed = "3";
                            PushNotificationUser.created_on = DateTime.Now;
                            PushNotificationUser.updated_on = DateTime.Now;
                            context.Entry(PushNotificationUser).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                }

                return new ResponseModel(ObjpushNotificationUser);
            }
            catch (Exception Ex)
            {

                return new ResponseModel(null, 1, Ex.Message);
            }
        }
        public async Task<ResponseModel> DeleteBankAccountsForUser(string mobileToken, BankAccount objBank)
        {
            TokenServices tokenServices = new TokenServices();
            string lcToken = tokenServices.GetLctoken(mobileToken);

            try
            {
                byte[] accountnum = System.Text.ASCIIEncoding.ASCII.GetBytes(objBank.account_number);
                string decodedaccountnumber = System.Convert.ToBase64String(accountnum);

                Dictionary<string, string> someDict = new Dictionary<string, string>();
                someDict.Add("id", Convert.ToString(objBank.id));
                someDict.Add("bankName", objBank.bank_name);
                someDict.Add("routingNumber", objBank.routing_number);
                someDict.Add("accountNumber", objBank.account_number);
                someDict.Add("accountType", objBank.account_type);
                someDict.Add("sourceFlag", "MSP");
                someDict.Add("LoanId", "0");

                var content = new FormUrlEncodedContent(someDict);

                var response = await API_Connection.DeleteAsync(lcToken, "/api/BankAccountInformation/DeleteBankDetails", content);

                var eventId = 5;
                var resourceName = "One-Time+Payment";
                var toEmail = "";
                var log = "Manage+Bank+Account+Page+-+DeleteBank";
                var actionName = "DELETE";

                var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

                return new ResponseModel(response);

            }
            catch (Exception ex)
            {
                return new ResponseModel(null, 1, ex.Message);
            }

        }
        public async Task<ResponseModel> GetSecurityQuestions(string lcAuthToken)
        {
            TokenServices tokenServices = new TokenServices();
            string lcToken = tokenServices.GetLctoken(lcAuthToken);

           

            var responseQuestionInfo = await API_Connection.GetAsync(lcToken, "/api/User/GetSecurtiyQuestions/");
            string returnedData = await responseQuestionInfo.Content.ReadAsStringAsync();

            dynamic objQuestion = JsonConvert.DeserializeObject(returnedData);

            Business_Services.Models.SecurityQuestion questionDetails = new Business_Services.Models.SecurityQuestion();

            questionDetails.questions = new List<SecurityQuestionSummary>();


            questionDetails.pin = objQuestion.pin;
            string strQuestion;
            string strAnswer;

            string strQuestionID;
            string strUserID;

            string strDecodeQuestion;
            string strDecodeAnswer;
            foreach (var questionNumber in objQuestion.secQuestions)
            {
                // Send request to pull all question
                try
                {
                    //Modified by BBSR Team on 12th Jan 2018
                    strQuestionID = questionNumber.questionID;
                    strUserID = questionNumber.userID;
                    //Modified by BBSR Team on 12th Jan 2018

                    strQuestion = questionNumber.secretQuestion;
                    //Decode the Encoded value from the B2C site                  
                    byte[] question = System.Convert.FromBase64String(strQuestion);
                    strDecodeQuestion = System.Text.Encoding.UTF8.GetString(question, 0, question.Length);


                    strAnswer = questionNumber.securityAnswer;


                    //Decode the Encoded value from the B2C site
                    if (!string.IsNullOrEmpty(strAnswer))
                    {
                        byte[] answer = System.Convert.FromBase64String(strAnswer);
                        strDecodeAnswer = System.Text.Encoding.UTF8.GetString(answer, 0, answer.Length);
                    }
                    else
                    { strDecodeAnswer = string.Empty; }

                    questionDetails.questions.Add(new SecurityQuestionSummary() { question = strDecodeQuestion, answer = strDecodeAnswer, questionid = strQuestionID, userid = strUserID });

                }
                catch (Exception ex)
                {
                    var Message = ex.Message;
                    return new ResponseModel(Message);

                }
            }

            var eventId = 5;
            var resourceName = "Profile";
            var toEmail = "";
            var log = "Viewed+Security+Question";
            var actionName = "VIEW";

            var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
            string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

            return new ResponseModel(questionDetails);
        }

        public async Task<ResponseModel> myloanGetPinAsyn(string lcAuthToken, string Pin)
        {

            try
            {
                Business_Services.Models.User getpinloan = new Models.User();
                TokenServices tokenServices = new TokenServices();
                string lcToken = tokenServices.GetLctoken(lcAuthToken);
                var responseQuestionInfo = await API_Connection.GetAsync(lcToken, "/api/User/ValidateForMobileUserPin?pin=" + Pin);
                string returnedData = await responseQuestionInfo.Content.ReadAsStringAsync();

                dynamic objQuestion = JsonConvert.DeserializeObject(returnedData);
                //string getpin = objQuestion;

                //if (Pin == getpin)
                //{

                //    getpinloan.is_successful = true;
                //}
                //else
                //{
                //    getpinloan.is_successful = false;
                //}
                // }
                return new ResponseModel(objQuestion);
            }
            catch (Exception Ex)
            {

                return new ResponseModel(null, 1, Ex.Message);
            }
        }






        public ResponseModel GetPrivacyForUser(string user_id)
        {
            // Note - Privacy text is specific to the client to which the user belongs
            return new ResponseModel(new GenericContent
            {
                heading = "FIDELITY NATIONAL FINANCIAL PRIVACY NOTICE",
                detail = "At Fidelity National Financial, Inc., we respect and believe it is important to protect the privacy of consumers and our customers.  This Privacy Notice explains how we collect, use, and protect any information that we collect from you, when and to whom we disclose such information, and the choices you have about the use of that information.  A summary of the Privacy Notice is below, and we encourage you to review the entirety of the Privacy Notice following this summary.  You can opt-out of certain disclosures by following our opt-out procedure set forth at the end of this Privacy Notice."
            });
        }

        public async Task<ResponseModel> GetBankAccountsForUser(string mobileToken)
        {
            TokenServices tokenServices = new TokenServices();
            string lcToken = tokenServices.GetLctoken(mobileToken);
            try
            {
                var response = await API_Connection.GetAsync(lcToken, "/api/BankAccountInformation/GetBanksByUserId");
                string returnedData = await response.Content.ReadAsStringAsync();

                

                Class1[] bankInfo = JsonConvert.DeserializeObject<Class1[]>(returnedData);

                List<Models.DAL.LoanCareContext.BankAccount> banklist = new List<Models.DAL.LoanCareContext.BankAccount>();

                foreach (var bdetail in bankInfo)
                {
                    byte[] loannotify = Convert.FromBase64String(bdetail.accountNumber);
                    string decodedaccountNumber = Encoding.UTF8.GetString(loannotify);
                    Models.DAL.LoanCareContext.BankAccount bankdetails = new Models.DAL.LoanCareContext.BankAccount()
                    {
                        bank_id = bdetail.id,
                        bank_name = bdetail.bankName,
                        account_number = decodedaccountNumber,
                        account_nickname = bdetail.bankName.Length > 10 ? bdetail.bankName.Substring(0, 10) : bdetail.bankName,
                        account_type = (bdetail.accountType == "C" ? "Checking Account" : "Saving Account"),
                        legal_name = bdetail.legalName,
                        routing_number = bdetail.routingNumber,
                        isNew = false, //Added by Avinash
                    };
                    banklist.Add(bankdetails);

                }

                var eventId = 2;
                var resourceName = "Payment";
                var toEmail = "";
                var log = "Viewed+Manage+Bank+Account+page";
                var actionName = "VIEW";

                var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

                return new ResponseModel(banklist);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }
        }

        public async Task<ResponseModel> GetManageAccountForUser(string mobileToken, string LoanNumber)
        {
            TokenServices tokenServices = new TokenServices();
            string lcToken = tokenServices.GetLctoken(mobileToken);

            var eventId = 7;
            var resourceName = "Account";
            var toEmail = "";
            var log = "Viewed+Manage+Account+page";
            var actionName = "VIEW";

            var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
            string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

            UserAlertCount Users = new UserAlertCount();
            var responseuser = await API_Connection.GetAsync(lcToken, "api/Personal/GetBorrowerContactInfo/" + LoanNumber);
            string returnedDatausername = await responseuser.Content.ReadAsStringAsync();
            personal_getborrowercontactInfo getusernameinfo = JsonConvert.DeserializeObject<personal_getborrowercontactInfo>(returnedDatausername);
            Users.user_first_name = getusernameinfo.contactinfo.contactInfo.firstName;
            Users.count_of_user_alerts = 0;
            return new ResponseModel(Users);
        }
        //Modified by BBSR_Team on 11th Jan 2018

        public async Task<ResponseModel> ForgotUser_Id(Business_Services.Models.User userDetail)
        {
            try
            {
                string ssn = userDetail.ssn;
                byte[] userSSN = System.Text.ASCIIEncoding.ASCII.GetBytes(ssn);
                string decodeduserSSN = System.Convert.ToBase64String(userSSN);

                string Loan_Number = userDetail.loanNumber;

                //Modified by BBSR_Team on 11th Jan 2018
                Dictionary<string, string> forgotDict = new Dictionary<string, string>();
                forgotDict.Add("userID", Loan_Number);
                forgotDict.Add("ssn", decodeduserSSN);
                string clientUrl = "www.myloancare.com";
                var forgotcontent = new FormUrlEncodedContent(forgotDict);
                var UserDetails = await API_Connection.PostUserAsync("/api/Register/SendUserId/", forgotcontent);
                //Modified by BBSR_Team on 11th Jan 2018

                string returnedData = await UserDetails.Content.ReadAsStringAsync();
                dynamic objForgotUser = JsonConvert.DeserializeObject(returnedData);

                if (returnedData.Contains("success"))
                {
                    string clientName = objForgotUser.client.clientName;

                    string strUserName = objForgotUser.user.userName;
                    byte[] userName = System.Text.ASCIIEncoding.ASCII.GetBytes(strUserName);
                    string decodeduserName = System.Convert.ToBase64String(userName);

                    string loanNo = objForgotUser.userLoan.loanNo;
                    string clientPhone = objForgotUser.client.clientPhone;
                   

                    string strUserEmail = objForgotUser.userLoan.emailAddress;
                    byte[] userEmail = System.Text.ASCIIEncoding.ASCII.GetBytes(strUserEmail);
                    string decodeduserEmail = System.Convert.ToBase64String(userEmail);

                    Dictionary<string, string> someDict = new Dictionary<string, string>();
                    someDict.Add("LoanNo", loanNo);
                    someDict.Add("userName", strUserName);
                    someDict.Add("email", strUserEmail);
                    someDict.Add("clientName", clientName);
                    someDict.Add("clientPhone", clientPhone);
                    someDict.Add("url", clientUrl);
                    someDict.Add("PROPERTY_STATE_CODE", "");
                    var content = new FormUrlEncodedContent(someDict);
                    var response = await API_Connection.PostUserAsync("/api/Register/SendEmailforUserId/", content);
                    string returnedDataemail = await response.Content.ReadAsStringAsync();
                    dynamic objForgotemail = JsonConvert.DeserializeObject(returnedDataemail);

                    return new ResponseModel(null, 0, "Your User ID has been sent to your email account that is on record. Please check your e-mail. If you have issues receiving the email, please contact customer support.");
                }
                else
                {
                    return new ResponseModel(returnedData, 1, returnedData);
                }
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, "Your account has not been registered or wrong credentials are provided. Please check your LoanNo and SSN/TaxID.");
            }
        }

        //Modified by BBSR_Team on 17th Jan 2018
        public async Task<ResponseModel> ForgotPassword(Business_Services.Models.User userDetail)
        {
            try
            {
                string ssn = userDetail.ssn;
                byte[] userSSN = System.Text.ASCIIEncoding.ASCII.GetBytes(ssn);
                string decodeduserSSN = System.Convert.ToBase64String(userSSN);

                string Loan_Number = userDetail.loanNumber;

                //Modified by BBSR_Team on 17th Jan 2018
                Dictionary<string, string> forgotDict = new Dictionary<string, string>();
                forgotDict.Add("userID", Loan_Number);
                forgotDict.Add("ssn", decodeduserSSN);

                var forgotcontent = new FormUrlEncodedContent(forgotDict);
                var UserDetails = await API_Connection.PostUserAsync("/api/Register/SendPassword", forgotcontent);
                //Modified by BBSR_Team on 17th Jan 2018           

                string returnedData = await UserDetails.Content.ReadAsStringAsync();

                dynamic objForgotUser = JsonConvert.DeserializeObject(returnedData);
                if (returnedData.Contains("success"))
                {
                    secQuesCollectionforgotuser listsecquestion = new secQuesCollectionforgotuser();
                    List<SecurityQuestionForgotUser> objSecurity = new List<SecurityQuestionForgotUser>();

                    SequrityQuestionUserLoan objsequserloan = new SequrityQuestionUserLoan()
                    {
                        id = objForgotUser.userLoan.id,
                        loanNo = objForgotUser.userLoan.loanNo

                    };

                    SequrityQuestionUser objsequstionuser = new SequrityQuestionUser()
                    {

                        id = objForgotUser.user.id,
                        ssn = objForgotUser.user.ssn
                    };

                    foreach (var SecQues in objForgotUser.secQuesCollection)
                    {

                        SecurityQuestionForgotUser ObjsecQuesCollection = new SecurityQuestionForgotUser()
                        {

                            isNew = SecQues.isNew,
                            phrases = SecQues.phrases,
                            questionID = SecQues.questionID,
                            questionNo = SecQues.questionNo,
                            secretQuestion = SecQues.secretQuestion,
                            securityAnswer = SecQues.securityAnswer,
                            skipChildrenRead = SecQues.skipChildrenRead,
                            userFrom = SecQues.userFrom,
                            userID = SecQues.userID
                        };
                        objSecurity.Add(ObjsecQuesCollection);
                    }
                    SecurityQuestionstatus objSecurityQuestionstatus = new SecurityQuestionstatus();
                    if (objSecurity.Count == 0)
                    {
                        objSecurityQuestionstatus.SecurityStatus = false;
                    }
                    else
                    {
                        objSecurityQuestionstatus.SecurityStatus = true;
                    }


                    listsecquestion.secQuesCollection = objSecurity;
                    listsecquestion.user = objsequstionuser;
                    listsecquestion.userLoan = objsequserloan;
                    listsecquestion.secQuesstatus = objSecurityQuestionstatus;
                    if (!returnedData.Contains("success"))
                    {


                        return new ResponseModel(listsecquestion, 1, "Error");
                    }

                    string clientUrl = "www.myloancare.com";
                    string loanNo = objForgotUser.userLoan.loanNo;
                    string userName = objForgotUser.user.userName;
                    string Password = objForgotUser.user.password;
                    string emailAddress = objForgotUser.userLoan.emailAddress;
                    string clientName= objForgotUser.client.clientName;
                    string clientPhone = objForgotUser.client.clientPhone;
                    Dictionary<string, string> someDict = new Dictionary<string, string>();
      
                    someDict.Add("LoanNo", loanNo);
                    someDict.Add("email", emailAddress);
                    someDict.Add("clientName", clientName);
                    someDict.Add("password", Password);
                    someDict.Add("url", clientUrl);
                    someDict.Add("PROPERTY_STATE_CODE", "");
                    var content = new FormUrlEncodedContent(someDict);
                    var response = await API_Connection.PostUserAsync("/api/Register/SendEmailforPassword/", content);
                    string returnedDataemail = await response.Content.ReadAsStringAsync();
                    dynamic objForgotemail = JsonConvert.DeserializeObject(returnedDataemail);

                    return new ResponseModel(listsecquestion);
                }
                return new ResponseModel(null, 1, returnedData);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, "Your account has not been registered or wrong credentials are provided. Please check your LoanNo and SSN/TaxID.");
            }
        }

        //Added by BBSR_Team on 24th Jan 2018
        public async Task<ResponseModel> ValidateSecurityAnswer(Business_Services.Models.User userDetail)
        {
            try
            {
                string strUserId = Convert.ToString(userDetail.id);
                string strStatus = Convert.ToString(userDetail.status);
                string strAttempt = Convert.ToString(userDetail.noOfAttempts);

                //Validate Security Answers.
                Dictionary<string, string> ConfDict = new Dictionary<string, string>();
                ConfDict.Add("noOfAttempts", strAttempt);
                ConfDict.Add("status", strStatus);
                ConfDict.Add("userId", strUserId);

                var Confcontent = new FormUrlEncodedContent(ConfDict);
                var ConfDetails = await API_Connection.PostUserAsync("/api/Register/ValidateSecurityAnswers", Confcontent);

                string returnedConfData = await ConfDetails.Content.ReadAsStringAsync();
                dynamic objConf = JsonConvert.DeserializeObject(returnedConfData);

                return new ResponseModel(objConf);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }
        }

        public async Task<ResponseModel> GetRefereshToken(string MobileToken, string loannumber,string password)
        {
            Business_Services.Models.GenerateNewToken objgenerateToken = new GenerateNewToken();
            try
            {
             var content = new FormUrlEncodedContent(new Dictionary<string, string> { { "userID", loannumber }, { "password", password } });
             var response = await API_Connection.PostAsync("/api/Auth/Authenticate", content);
             var Token =    response.tokenValue;
              var Decryptdata = objgenerateToken.Decrypt(MobileToken);

                dynamic ObjUserId = JsonConvert.DeserializeObject(Decryptdata);
                string objUId = ObjUserId.UserId;
                string objPWd = ObjUserId.Password;
                int objCId = ObjUserId.ClientId;
                bool eStatemente = false;
                string objusername = ObjUserId.UserName;
                string resourcename = ObjUserId.resourcename;
                string logview = ObjUserId.log;
                bool eStatementenr = ObjUserId.eStatement;
                var MobileTokenNew = objgenerateToken.GenerateToken(objUId, objPWd, objCId, Token, objusername, resourcename, logview, eStatemente);
               // loanDetails.Token = MobileTokenNew;
                return new ResponseModel(MobileTokenNew);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }
        }


        //Modified by BBSR_Team on 24th Jan 2018
        public async Task<ResponseModel> ResetSendPassword(Business_Services.Models.User userDetail)
        {
            try
            {
                string ssn = userDetail.ssn;
                byte[] userSSN = System.Text.ASCIIEncoding.ASCII.GetBytes(ssn);
                string decodeduserSSN = System.Convert.ToBase64String(userSSN);

                string Loan_Number = userDetail.loanNumber;

                //Modified by BBSR_Team on 17th Jan 2018
                Dictionary<string, string> forgotDict = new Dictionary<string, string>();
                forgotDict.Add("userID", Loan_Number);
                forgotDict.Add("ssn", decodeduserSSN);

                var forgotcontent = new FormUrlEncodedContent(forgotDict);
                var UserDetails = await API_Connection.PostUserAsync("/api/Register/SendPassword", forgotcontent);
                //Modified by BBSR_Team on 17th Jan 2018           

                string returnedData = await UserDetails.Content.ReadAsStringAsync();
                dynamic objForgotUser = JsonConvert.DeserializeObject(returnedData);

                string clientName = objForgotUser.client.clientName;
                string strUserId = Convert.ToString(objForgotUser.user.id);
                string loanNo = objForgotUser.userLoan.loanNo;
                string clientUrl = objForgotUser.client.privateLabelURL;
                string strUserEmail = objForgotUser.userLoan.emailAddress;
                string strPassword = objForgotUser.user.password;

                //Reset Password and Send email to user.
                Dictionary<string, string> ResetDict = new Dictionary<string, string>();
                ResetDict.Add("LoanNo", loanNo);
                ResetDict.Add("email", strUserEmail);
                ResetDict.Add("clientName", clientName);
                ResetDict.Add("password", strPassword);
                ResetDict.Add("url", clientUrl);
                ResetDict.Add("PROPERTY_STATE_CODE", string.Empty);

                var Resetcontent = new FormUrlEncodedContent(ResetDict);
                var ResetDetails = await API_Connection.PostUserAsync("/api/Register/SendEmailforPassword", Resetcontent);

                string returnedResetData = await ResetDetails.Content.ReadAsStringAsync();
                dynamic objResetPassword = JsonConvert.DeserializeObject(returnedResetData);

                return new ResponseModel(objResetPassword);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }
        }

        public async Task<ResponseModel> getUserDetailsAsync(string lcAuthToken, string loan_number, bool Is_New_MobileUser)
        {
            try
            {
                Business_Services.Models.User Auth_data = new Models.User();
                var responseUserInfo = await API_Connection.GetAsync(lcAuthToken, "/api/User/GetUserInformation");
                string returnedData = await responseUserInfo.Content.ReadAsStringAsync();
                dynamic objUserName = JsonConvert.DeserializeObject(returnedData);

                var responseClientName = await API_Connection.GetAsync(lcAuthToken, "/api/Helper/GetClientData/");
                string returnedDataClientName = await responseClientName.Content.ReadAsStringAsync();
                dynamic objClientName = JsonConvert.DeserializeObject(returnedDataClientName);

                string ClientName = objClientName.clientName;

                string userName = objUserName.user.userName;
                string LoanNumber = objUserName.currentUserLoan.loanNo;

                var responseUserName = await API_Connection.GetAsync(lcAuthToken, "/api/MyAccount/GetAccountInfo/" + LoanNumber);
                string returnedDataUserName = await responseUserName.Content.ReadAsStringAsync();
                MyAccount_GetAccountInfo getuserinfoUserName = JsonConvert.DeserializeObject<MyAccount_GetAccountInfo>(returnedDataUserName);
                string email = getuserinfoUserName.msg.emailAddress;

                var responsephoneNo = await API_Connection.GetAsync(lcAuthToken, "/api/Personal/GetBorrowerContactInfo/" + LoanNumber);
                string returnedDataPhoneNo = await responsephoneNo.Content.ReadAsStringAsync();
                personal_getborrowercontactInfo getuserinfoPhoneNo = JsonConvert.DeserializeObject<personal_getborrowercontactInfo>(returnedDataPhoneNo);



                var responsecelldisclosure = await API_Connection.GetAsync(lcAuthToken, "/api/Personal/GetBorrowerContactInfoPopup/" + LoanNumber);
                string returnedcelldisclosure = await responsephoneNo.Content.ReadAsStringAsync();
                personal_getborrowercontactInfo getdisclosure = JsonConvert.DeserializeObject<personal_getborrowercontactInfo>(returnedcelldisclosure);


                //var responseUserIn = await API_Connection.GetAsync(lcAuthToken, "/api/User/GetUserInformation");
                //string returnedDataUser = await responseUserIn.Content.ReadAsStringAsync();
                //dynamic getuserinfo = JsonConvert.DeserializeObject(returnedDataUser);
                // User_GetUserInformation getuserinfo = JsonConvert.DeserializeObject<User_GetUserInformation>(returnedDataUser);

                var responseLP = await API_Connection.GetAsync(lcAuthToken, "/api/User/LanguagePref/?userId=" + objUserName.user.userName);
                string returnedDataLP = await responseLP.Content.ReadAsStringAsync();

                 Business_Services.Models.User userDetails = new Business_Services.Models.User();

                if (getdisclosure.contactinfo.contactInfo.primaryTelecomNumber.consentRevokeIndicatorCode == null && getdisclosure.contactinfo.contactInfo.primaryTelecomNumber.type == "C")
                {
                    userDetails.phone_primary_number_concern = getdisclosure.contactinfo.contactInfo.primaryTelecomNumber.phoneNumber;
                    userDetails.phone_primary_type_concern = getdisclosure.contactinfo.contactInfo.primaryTelecomNumber.type;
                }
                if (getdisclosure.contactinfo.contactInfo.secondaryTelecomNumber.consentRevokeIndicatorCode == null && getdisclosure.contactinfo.contactInfo.secondaryTelecomNumber.type == "C")
                {
                    userDetails.phone_secondary_number_concern = getdisclosure.contactinfo.contactInfo.secondaryTelecomNumber.phoneNumber;
                    userDetails.phone_secondary_type_concern = getdisclosure.contactinfo.contactInfo.secondaryTelecomNumber.type;
                }
                    foreach (var OtherTeleNo in getdisclosure.contactinfo.contactInfo.otherTelecomNumbers)
                {

                    if (OtherTeleNo.sequenceNumber == 1 && OtherTeleNo.consentRevokeIndicatorCode==null&&OtherTeleNo.type == "C")
                    {
                        userDetails.phone_other_1_type_concern = OtherTeleNo.type;
                      userDetails.phone_other_1_number_concern = OtherTeleNo.phoneNumber;
                    }
                    if (OtherTeleNo.sequenceNumber == 2 && OtherTeleNo.consentRevokeIndicatorCode == null && OtherTeleNo.type == "C")
                    {
                        userDetails.phone_other_2_type_concern = OtherTeleNo.type;
                        userDetails.phone_other_2_number_concern = OtherTeleNo.phoneNumber;
                    }
                    if (OtherTeleNo.sequenceNumber == 3 && OtherTeleNo.consentRevokeIndicatorCode == null && OtherTeleNo.type == "C")
                    {
                        userDetails.phone_other_3_type_concern = OtherTeleNo.type;
                        userDetails.phone_other_3_number_concern = OtherTeleNo.phoneNumber;
                    }
                }

                using (var ctx = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
                {
                    var setpin = ctx.MobileUsers.Where(s => s.User_Id == userName).FirstOrDefault();

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
                                User_Id = userName,
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
                            userDetails.mae_steps_completed = "0";
                        }

                        var responseQuestionInfo = await API_Connection.GetAsync(lcAuthToken, "/api/User/GetSecurtiyQuestions/");
                        string returnedDatasecurity = await responseQuestionInfo.Content.ReadAsStringAsync();

                        dynamic objQuestion = JsonConvert.DeserializeObject(returnedDatasecurity);

                        Business_Services.Models.SecurityQuestion questionDetails = new Business_Services.Models.SecurityQuestion();

                        questionDetails.questions = new List<SecurityQuestionSummary>();


                        questionDetails.pin = objQuestion.pin;
                        string strQuestion;
                        string strAnswer;

                        string strQuestionID;
                        string strUserID;


                        foreach (var questionNumber in objQuestion.secQuestions)
                        {
                            // Send request to pull all question

                            //Modified by BBSR Team on 12th Jan 2018
                            strQuestionID = questionNumber.questionID;
                            strUserID = questionNumber.userID;
                            //Modified by BBSR Team on 12th Jan 2018

                            strQuestion = questionNumber.secretQuestion;


                            strAnswer = questionNumber.securityAnswer;
                            if (strUserID != "0" && strAnswer != "")
                            {
                                using (var ctxsecurity = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
                                {
                                    var setpinSecurity = ctxsecurity.MobileUsers.Where(s => s.User_Id == userName).FirstOrDefault();

                                    using (var context = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
                                    {
                                        setpinSecurity.mae_steps_completed = "1";
                                        context.Entry(setpinSecurity).State = EntityState.Modified;
                                        context.SaveChanges();
                                    }

                                }
                            }
                        }
                    }
                    else if (setpin != null)
                    {
                        userDetails.mae_steps_completed = setpin.mae_steps_completed;
                    }
                    else if (setpin.mae_steps_completed == "")
                    {
                        userDetails.mae_steps_completed = "0";
                    }
                }

                using (var ctx = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
                {
                    var setpin = ctx.MobileUsers.Where(s => s.User_Id == userName).FirstOrDefault();

                    if (setpin != null)
                    {

                        if (setpin.mae_steps_completed == "1" || setpin.mae_steps_completed == "2" || setpin.mae_steps_completed == "3")
                        {

                            var responseQuestionInfo = await API_Connection.GetAsync(lcAuthToken, "/api/User/GetSecurtiyQuestions/");
                            string returnedDatasecurity = await responseQuestionInfo.Content.ReadAsStringAsync();

                            dynamic objQuestion = JsonConvert.DeserializeObject(returnedDatasecurity);

                            Business_Services.Models.SecurityQuestion questionDetails = new Business_Services.Models.SecurityQuestion();

                            questionDetails.questions = new List<SecurityQuestionSummary>();


                            questionDetails.pin = objQuestion.pin;
                            string strQuestion;
                            string strAnswer;

                            string strQuestionID;
                            string strUserID;


                            foreach (var questionNumber in objQuestion.secQuestions)
                            {
                                // Send request to pull all question

                                //Modified by BBSR Team on 12th Jan 2018
                                strQuestionID = questionNumber.questionID;
                                strUserID = questionNumber.userID;
                                //Modified by BBSR Team on 12th Jan 2018

                                strQuestion = questionNumber.secretQuestion;


                                strAnswer = questionNumber.securityAnswer;
                                if (strUserID != "0" && strAnswer != "")
                                {
                                    userDetails.SecurityQuestionFlag = true;
                                }
                            }
                        }
                    }
                    userDetails.mae_steps_completed = setpin.mae_steps_completed;
                }


                if (objUserName.currentUserLoan.roleId == 5)
                {
                    var responseuser = await API_Connection.GetAsync(lcAuthToken, "api/Personal/GetBorrowerContactInfo/" + objUserName.currentUserLoan.loanNo);
                    string returnedDatausername = await responseuser.Content.ReadAsStringAsync();
                    personal_getborrowercontactInfo getusernameinfo = JsonConvert.DeserializeObject<personal_getborrowercontactInfo>(returnedDatausername);
                    userDetails.first_name = getusernameinfo.contactinfo.contactInfo.firstName;
                    userDetails.last_name = getusernameinfo.contactinfo.contactInfo.lastOrOrganizationName;
                }
                else if (objUserName.currentUserLoan.roleId == 4)
                {
                    var responseusercoborrower = await API_Connection.GetAsync(lcAuthToken, "api/Personal/GetCoBorrowerContactInfo/" + objUserName.currentUserLoan.loanNo);
                    string returnedDatausernamecoborrower = await responseusercoborrower.Content.ReadAsStringAsync();
                    dynamic obj4 = JsonConvert.DeserializeObject(returnedDatausernamecoborrower);
                    personal_getborrowercontactInfo getusernamecoborrowerinfo = JsonConvert.DeserializeObject<personal_getborrowercontactInfo>(returnedDatausernamecoborrower);
                    userDetails.first_name = getusernamecoborrowerinfo.contactinfo.contactInfo.firstName;
                    userDetails.last_name = getusernamecoborrowerinfo.contactinfo.contactInfo.lastOrOrganizationName;
                }
                userDetails.loanss = new List<LoanSummary>();
                string Property_Address;


                foreach (string loanNumber in objUserName.user.userLoansList)
                {

                    // Send request to pull address of each loan
                    try
                    {
                        var responseAcc = await API_Connection.GetAsync(lcAuthToken, "/api/MyAccount/GetAccountInfo/" + loanNumber);
                        string returnedDataloan = await responseAcc.Content.ReadAsStringAsync();
                        MyAccount_GetAccountInfo propertyaddress = JsonConvert.DeserializeObject<MyAccount_GetAccountInfo>(returnedDataloan);

                        Property_Address = propertyaddress.msg.custAddress;

                        var loanDetailsList = new LoanSummary
                        {

                            loan_number = loanNumber,
                            property_address = Property_Address
                        };
                        userDetails.loanss.Add(loanDetailsList);

                    }
                    catch (Exception ex)
                    {
                        foreach (var loan_Number in objUserName.user.userLoansList)
                        {
                                    var loanDetailsList = new LoanSummary
                                    {

                                        loan_number = loan_Number,
                                        property_address =""
                                    };
                                    userDetails.loanss.Add(loanDetailsList);
                    }

                        var Message = ex.Message;
                        userDetails.username = objUserName.user.userName;
                        userDetails.is_enrolled = (objUserName.currentUserLoan.eStatement == null) ? false : true;
                        userDetails.email = getuserinfoUserName.msg.emailAddress;
                        userDetails.addresss.isForeign = getuserinfoPhoneNo.contactinfo.contactInfo.isInternationalAddress;
                        userDetails.addresss.street = getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressStreet;
                        userDetails.addresss.city = getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressCityName;
                        userDetails.addresss.zipcode = getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressPostalCode;
                        userDetails.addresss.state = getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressStateAbbreviation;
                        userDetails.addresss.phone = getuserinfoPhoneNo.contactinfo.contactInfo.primaryTelecomNumber.phoneNumber;
                        userDetails.ClientId = objUserName.currentUserLoan.clientID;
                        userDetails.ClientName = ClientName;
                        userDetails.LoginId = loan_number;
                        return new ResponseModel(userDetails);
                    }
                }
                userDetails.username = objUserName.user.userName;
                userDetails.email = getuserinfoUserName.msg.emailAddress;
                userDetails.addresss.isForeign = getuserinfoPhoneNo.contactinfo.contactInfo.isInternationalAddress;
                userDetails.addresss.street = getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressStreet;
                userDetails.addresss.city = getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressCityName;
                userDetails.addresss.zipcode = getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressPostalCode;
                userDetails.addresss.state = getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressStateAbbreviation;
                userDetails.addresss.phone = getuserinfoPhoneNo.contactinfo.contactInfo.primaryTelecomNumber.phoneNumber;
                userDetails.ClientId = objUserName.currentUserLoan.clientID;
                userDetails.ClientName = ClientName;
                userDetails.BorrowerName = getuserinfoPhoneNo.contactinfo.contactInfo.borrower;
                userDetails.id = objUserName.user.id;
                userDetails.LoginId = loan_number;
                userDetails.is_enrolled = (objUserName.currentUserLoan.eStatement == null) ? false : true;

                return new ResponseModel(userDetails);
            }
            catch (Exception Ex)
            {

                return new ResponseModel(null, 1, Ex.Message);
            }
        }

        public async Task<ResponseModel> getUserDetailsAsyn(string lcAuthToken, string loan_number)
        {
            Token tokenObject = JsonConvert.DeserializeObject<Token>(Encryptor.Decrypt(lcAuthToken));
            Business_Services.Models.GenerateNewToken objgenerateToken = new GenerateNewToken();

            var Decryptdata = objgenerateToken.Decrypt(lcAuthToken);
            dynamic ObjUserId = JsonConvert.DeserializeObject(Decryptdata);
            bool objisenrolled = ObjUserId.eStatement;
            //string UserName = ObjUserId.UserName;
            try
            {
                string lcToken = tokenObject.Lcauth;
                //var responseUserInfo = await API_Connection.GetAsync(lcToken, "/api/User/GetUserInformation");
                //string returnedData = await responseUserInfo.Content.ReadAsStringAsync();
                //dynamic obj = JsonConvert.DeserializeObject(returnedData);
                //string userName = obj.user.userName;
                var responseEscrow = await API_Connection.GetAsync(lcToken, "/api/Escrow/CallEscrow/?LoanNo=" + loan_number);
                string returnedData = await responseEscrow.Content.ReadAsStringAsync();
                Escrow_CallEscrow escrowInfo = JsonConvert.DeserializeObject<Escrow_CallEscrow>(returnedData);

                var response_princilAmout = await API_Connection.GetAsync(lcToken, "/api/Loan/GetCurrentLoanInfo/" + loan_number);
                string returnedDataAmount = await response_princilAmout.Content.ReadAsStringAsync();
                Loan_GetCurrentLoanInfo loanInfo = JsonConvert.DeserializeObject<Loan_GetCurrentLoanInfo>(returnedDataAmount);


                var responseGetUserInfo = await API_Connection.GetAsync(lcToken, "/api/User/GetUserInformation");
                string returnedDataUser = await responseGetUserInfo.Content.ReadAsStringAsync();
                dynamic getuserinfo = JsonConvert.DeserializeObject(returnedDataUser);
                string userName = getuserinfo.user.userName;

                var responseLP = await API_Connection.GetAsync(lcToken, "/api/User/LanguagePref/?userId=" + userName);
                string returnedDataLP = await responseLP.Content.ReadAsStringAsync();

                var responseEstatement = await API_Connection.GetAsync(lcToken, "/api/User/GetLoanData/?id="+ loan_number);
                string returnedDateEstement = await responseEstatement.Content.ReadAsStringAsync();
                dynamic getloanestatement = JsonConvert.DeserializeObject(returnedDateEstement);
                string isenrolledloan = getloanestatement.currentUserLoan.eStatement;
              
                Business_Services.Models.User userDetails = new Business_Services.Models.User();


                if (getuserinfo.currentUserLoan.roleId == 5)
                {
                    var responseuser = await API_Connection.GetAsync(lcToken, "api/Personal/GetBorrowerContactInfo/" + getuserinfo.currentUserLoan.loanNo);
                    string returnedDatausername = await responseuser.Content.ReadAsStringAsync();
                    personal_getborrowercontactInfo getusernameinfo = JsonConvert.DeserializeObject<personal_getborrowercontactInfo>(returnedDatausername);
                    userDetails.first_name = getusernameinfo.contactinfo.contactInfo.firstName;
                    userDetails.last_name = getusernameinfo.contactinfo.contactInfo.lastOrOrganizationName;
                }
                if (getuserinfo.currentUserLoan.roleId == 4)
                {
                    var responseusercoborrower = await API_Connection.GetAsync(lcToken, "api/Personal/GetCoBorrowerContactInfo/" + getuserinfo.currentUserLoan.loanNo);
                    string returnedDatausernamecoborrower = await responseusercoborrower.Content.ReadAsStringAsync();
                    dynamic obj4 = JsonConvert.DeserializeObject(returnedDatausernamecoborrower);
                    personal_getborrowercontactInfo getusernamecoborrowerinfo = JsonConvert.DeserializeObject<personal_getborrowercontactInfo>(returnedDatausernamecoborrower);
                    userDetails.first_name = getusernamecoborrowerinfo.contactinfo.contactInfo.firstName;
                    userDetails.last_name = getusernamecoborrowerinfo.contactinfo.contactInfo.lastOrOrganizationName;
                }
                userDetails.loanss = new List<LoanSummary>();
                string Property_Address;

                // Send request to pull address of each loan
                var responseGetAccount = await API_Connection.GetAsync(lcToken, "/api/MyAccount/GetAccountInfo/" + loan_number);
                string returnedDataloan = await responseGetAccount.Content.ReadAsStringAsync();
                MyAccount_GetAccountInfo propertyaddress = JsonConvert.DeserializeObject<MyAccount_GetAccountInfo>(returnedDataloan);

                Property_Address = propertyaddress.msg.custAddress;

                var response = await API_Connection.GetAsync(lcToken, "/api/OneTimePayment/GetMockedPendingTransactions/?loanNo=" + loan_number + "&schDate=&");
                string returnedData1 = await response.Content.ReadAsStringAsync();
                List<OneTimePayment_GetMockedPendingTransactions> pendingInfo = JsonConvert.DeserializeObject<List<OneTimePayment_GetMockedPendingTransactions>>(returnedData1);
                int no_of_payments;

                no_of_payments = pendingInfo.Count();

                var lastPPD = loanInfo.lastPrinPD;
                if (lastPPD == "")
                {
                    lastPPD = "0";
                }
                var loan_duedate = "";
                try
                {
                    if (Convert.ToDateTime(loanInfo.lastTransactionAppliedDate).ToString("MM/dd/yy") == null)
                    {

                        loan_duedate = "";
                    }
                    else
                    {

                        loan_duedate = Convert.ToDateTime(loanInfo.lastTransactionAppliedDate).ToString("MM/dd/yy");
                    }
                }
                catch (Exception Ex)
                {
                    loan_duedate = "";
                }

                UserLoaninfo userLoanAmount = new UserLoaninfo();
                userLoanAmount.escrow_balance = Convert.ToDecimal(loanInfo.escrowBalance);
                userLoanAmount.loan_duedate = Convert.ToDateTime(loanInfo.dueDate).ToString("MM/dd/yy");
                userLoanAmount.loan_total_amount = loanInfo.netPresent;
                userLoanAmount.loan_principal_balance = Convert.ToDecimal(loanInfo.firstPB);
                userLoanAmount.last_pending_payments = Convert.ToString(no_of_payments);
                userLoanAmount.last_payment_date = loan_duedate;
                userDetails.username = getuserinfo.user.userName;


                if (isenrolledloan == null)
                {
                    userLoanAmount.is_enrolled = false;
                }
                if (isenrolledloan != null)
                {
                    userLoanAmount.is_enrolled = true;
                }


                DateTime date = new DateTime();
                date = Convert.ToDateTime(userLoanAmount.loan_duedate);

                DateTime startDate = System.DateTime.Now;
                TimeSpan objTimeSpan = startDate - date;
                double Days = Convert.ToDouble(objTimeSpan.TotalDays);


                if (Days > 60)
                {

                    userLoanAmount.account_status = 1;
                    userLoanAmount.last_pending_payments = "0";
                    userLoanAmount.is_payment_past_due = true;
                }

                UserLoaninfo UserLoanIfo = new UserLoaninfo
                {
                    UserId = getuserinfo.currentUserLoan.userID,
                    loan_number = loan_number,
                    property_address = Property_Address,
                    co_borrower_name = propertyaddress.msg.coBorrower,
                    // loan_duedate = userLoanAmount.loan_duedate.ToString("mm/dd/yyy"),
                    loan_duedate = Convert.ToDateTime(userLoanAmount.loan_duedate).ToString("MM/dd/yy"),
                    last_pending_payments = userLoanAmount.last_pending_payments,
                    is_payment_past_due = userLoanAmount.is_payment_past_due,
                    account_status = userLoanAmount.account_status,
                    escrow_balance = userLoanAmount.escrow_balance,
                    loan_total_amount = userLoanAmount.loan_total_amount,
                    loan_principal_balance = userLoanAmount.loan_principal_balance,
                    last_payment_date = userLoanAmount.last_payment_date,
                    is_enrolled = userLoanAmount.is_enrolled,
                    is_escrow_loan = escrowInfo.lastAna.Contains('*') ? false : true

                };
                return new ResponseModel(UserLoanIfo);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }
        }

        //Modified by BBSR_Team on 11th Jan 2018
        public async Task<ResponseModel> ForgotUser_Id(string Loan_Number, string ssn)
        {
            try
            {

                byte[] userSSN = System.Text.ASCIIEncoding.ASCII.GetBytes(ssn);
                string decodeduserSSN = System.Convert.ToBase64String(userSSN);

                //Modified by BBSR_Team on 11th Jan 2018
                Dictionary<string, string> forgotDict = new Dictionary<string, string>();
                forgotDict.Add("userID", Loan_Number);
                forgotDict.Add("ssn", decodeduserSSN);

                var forgotcontent = new FormUrlEncodedContent(forgotDict);
                var UserDetails = await API_Connection.PostUserAsync("/api/Register/SendUserId/", forgotcontent);
                //Modified by BBSR_Team on 11th Jan 2018


                string returnedData = await UserDetails.Content.ReadAsStringAsync();
                dynamic objForgotUser = JsonConvert.DeserializeObject(returnedData);

                string clientName = objForgotUser.client.clientName;

                string strUserName = objForgotUser.user.userName;
                byte[] userName = System.Text.ASCIIEncoding.ASCII.GetBytes(strUserName);
                string decodeduserName = System.Convert.ToBase64String(userName);

                string loanNo = objForgotUser.userLoan.loanNo;
                string clientPhone = objForgotUser.client.clientPhone;
                string clientUrl = "www.myloancare.com";

                string strUserEmail = objForgotUser.userLoan.emailAddress;
                byte[] userEmail = System.Text.ASCIIEncoding.ASCII.GetBytes(strUserEmail);
                string decodeduserEmail = System.Convert.ToBase64String(userEmail);

                Dictionary<string, string> someDict = new Dictionary<string, string>();
                someDict.Add("emailData[0][key]", "clientname");
                someDict.Add("emailData[0][value]", clientName);
                someDict.Add("emailData[0][update]", "undefined");
                someDict.Add("emailData[1][key]", "username");
                someDict.Add("emailData[1][value]", decodeduserName);
                someDict.Add("emailData[1][update]", "undefined");
                someDict.Add("emailData[2][key]", "loanNo");
                someDict.Add("emailData[2][value]", loanNo);
                someDict.Add("emailData[2][update]", "undefined");
                someDict.Add("emailData[3][key]", "clientPhone");
                someDict.Add("emailData[3][value]", clientPhone);
                someDict.Add("emailData[3][update]", "undefined");
                someDict.Add("emailData[4][key]", "url");
                someDict.Add("emailData[4][value]", clientUrl);
                someDict.Add("emailData[4][update]", "undefined");
                someDict.Add("update", "undefined");
                strUserEmail = "vignesh.hari1-external@tcs.com";
                var content = new FormUrlEncodedContent(someDict);

                var response = await API_Connection.PostUserRegisAsync("/api/Register/SendEmailforUserId/", content);

                return new ResponseModel(response);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }
        }
        public async Task<ResponseModel> ForgotPassword(string Loan_Number, string ssn)
        {
            try
            {
                byte[] userSSN = System.Text.ASCIIEncoding.ASCII.GetBytes(ssn);
                string decodeduserSSN = System.Convert.ToBase64String(userSSN);

                var UserDetails = await API_Connection.GetAsync("/api/Register/SendPassword/?loanNo=" + Loan_Number + "&ssn=" + decodeduserSSN + "&userId=");

                string returnedData = await UserDetails.Content.ReadAsStringAsync();
                dynamic objForgotUser = JsonConvert.DeserializeObject(returnedData);
                string clientName = objForgotUser.client.clientName;

                string strUserName = objForgotUser.user.userName;
                byte[] userName = System.Text.ASCIIEncoding.ASCII.GetBytes(strUserName);
                string decodeduserName = System.Convert.ToBase64String(userName);

                string loanNo = objForgotUser.userLoan.loanNo;
                string clientPhone = objForgotUser.client.clientPhone;
                string clientUrl = "www.myloancare.com";

                string strUserEmail = objForgotUser.userLoan.emailAddress;
                byte[] userEmail = System.Text.ASCIIEncoding.ASCII.GetBytes(strUserEmail);
                string decodeduserEmail = System.Convert.ToBase64String(userEmail);

                Dictionary<string, string> someDict = new Dictionary<string, string>();
                someDict.Add("emailData[0][key]", "clientname");
                someDict.Add("emailData[0][value]", clientName);
                someDict.Add("emailData[0][update]", "undefined");
                someDict.Add("emailData[1][key]", "username");
                someDict.Add("emailData[1][value]", decodeduserName);
                someDict.Add("emailData[1][update]", "undefined");
                someDict.Add("emailData[2][key]", "loanNo");
                someDict.Add("emailData[2][value]", loanNo);
                someDict.Add("emailData[2][update]", "undefined");
                someDict.Add("emailData[3][key]", "clientPhone");
                someDict.Add("emailData[3][value]", clientPhone);
                someDict.Add("emailData[3][update]", "undefined");
                someDict.Add("emailData[4][key]", "url");
                someDict.Add("emailData[4][value]", clientUrl);
                someDict.Add("emailData[4][update]", "undefined");
                someDict.Add("update", "undefined");
                strUserEmail = "vignesh.hari1-external@tcs.com";
                var content = new FormUrlEncodedContent(someDict);

                var response = await API_Connection.PostUserRegisAsync("/api/EmailNotification/SendEmailConfirmationForTemplate/?template=Forgotpassword&toEmail=" + strUserEmail + "&pageName=forgotPassword&userID=" + objForgotUser.user.id, content);


                return new ResponseModel(response);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }
        }


        public async Task<ResponseModel> ContactUsAsync(string mobileToken)
        {
            // To do - Use DI

            TokenServices tokenServices = new TokenServices();
            string lcToken = tokenServices.GetLctoken(mobileToken);
            try
             {

                

                ContactUs contactUs_details = new ContactUs();

                contactUs_details.address_helptext = null;
                contactUs_details.phone = "1-800-274-6600";
                contactUs_details.hours_helptext = "You can access your loan information by phone 24 Hours a day, 7 days a week using our Automated Loan Information System.  Just call 1-800-274-6600 from any touch-tone phone, and the automated system will guide you through a simple menu of options to provide you access to your account information.";
                contactUs_details.liveperson_url = "https://server.iad.liveperson.net/hc/s-24064726/web/ticketpub/msgcontroller.jsp?surveyname=Stearnsemail";

                List<mailing_address> AddressList = new List<mailing_address>();


                mailing_address addressList1 = new mailing_address
                {
                    contact_type = "Loan Inquiry:",
                    address_line_1 = "LoanCare ",
                    address_line_2 = "Attn: Consumer Solutions Department",
                    address_line_3 = "P.O. Box 8068",
                    address_line_4 = "Virginia Beach, VA 23450",
                };
                mailing_address addressList2 = new mailing_address
                {
                    contact_type = "Payments",
                    address_line_1 = "LoanCare",
                    address_line_2 = "P.O. Box 37628",
                    address_line_3 = "Philadelphia, PA 19101-0628",
                    address_line_4 = null,
                };
                mailing_address addressList3 = new mailing_address
                {
                    contact_type = "Notices of Error/Information requests",
                    address_line_1 = "LoanCare P.O. Box 8068",
                    address_line_2 = "Virginia Beach, VA 23450",
                    address_line_3 = "Attn: Mortgage Resolution",
                    address_line_4 = null,
                };
                mailing_address addressList4 = new mailing_address
                {
                    contact_type = "Overnight Address",
                    address_line_1 = "LoanCare",
                    address_line_2 = "3637 Sentara Way",
                    address_line_3 = "Virginia Beach, VA 23452",
                    address_line_4 = null,
                };


                List<business_hours> BusinesshrsList = new List<business_hours>();


                business_hours businesshrsList1 = new business_hours
                {
                    days_text = "Monday through Friday",
                    time_text = "8:00 AM - 10:00 PM EST",

                };
                business_hours businesshrsList2 = new business_hours
                {
                    days_text = "Saturday",
                    time_text = "8:00 AM - 03:00 PM EST",

                };
                var Etopics = new List<string>()
            {
            "Address Change Request",
            "Authorization Request",
            "Auto Draft Inquiry",
            "Document Request",
            "Escrow Inquiry",
            "Fee Inquiry",
            "Home Owners Insurance",
            "Loss Mitigation",
            "Mortgage Insurance Inquiry (PMI/MIP)",
            "Name Change Request",
            "Payment/Statement Inquiry",
            "Refund Request",
            "Release Loan Satisfaction Inquiry",
            "Seller Finance - All Request",
            "Tax Inquiry",
            "Website Inquiry",
            "Year End Statement Inquiry (1098)",
            "Credit Reporting Dispute"
            };
                AddressList.Add(addressList1);
                AddressList.Add(addressList2);
                AddressList.Add(addressList3);
                AddressList.Add(addressList4);
                BusinesshrsList.Add(businesshrsList1);
                BusinesshrsList.Add(businesshrsList2);
                contactUs_details.mailing_address = AddressList;
                contactUs_details.business_hours = BusinesshrsList;
                contactUs_details.email_topics = Etopics;

                var eventId = 6;
                var resourceName = "Account+Services";
                var toEmail = "";
                var log = "Viewed+Contact+Us+Page";
                var actionName = "VIEW";

                var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

                return new ResponseModel(contactUs_details);
            }
            catch (Exception Ex)
            {

                return new ResponseModel(null, 1, Ex.Message);
            }
        }




        public ResponseModel SendPasswordByMail(string user_id)
        {
            return new ResponseModel();
        }

        public ResponseModel SendUserIdByMail(string loan_number, string last4digits_ssn)
        {
            return new ResponseModel();
        }

        // private string m_exePath = string.Empty;

        public void LogWriter(string PropertyName, string logMessage)
        {
            LogWrite(PropertyName, logMessage);
        }
        public void LogWrite(string PropertyName, string logMessage)
        {
            try
            {
                using (StreamWriter w = File.AppendText(@"E:\API_Log\Log.txt"))
                {
                    Log(PropertyName, logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Log(string PropertyName, string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                txtWriter.WriteLine(" {0} {1}", PropertyName, logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<ResponseModel> UpdatePasswordAsync(string MobileToken, UpdatePassword loanDetails, string Password)
        {
            try
            {
                // To do - Use DI
                TokenServices tokenServices = new TokenServices();

                Business_Services.Models.GenerateNewToken objgenerateToken = new GenerateNewToken();

                var Decryptdata = objgenerateToken.Decrypt(MobileToken);

                dynamic ObjUserId = JsonConvert.DeserializeObject(Decryptdata);
                string objUId = ObjUserId.UserId;
                string objPWd = ObjUserId.Password;
                int objCId = ObjUserId.ClientId;
                string objusername = ObjUserId.UserName;
                string resourcename = ObjUserId.resourcename;
                string logview = ObjUserId.log;
                bool eStatementenr = ObjUserId.eStatement;
                LogWriter("current_password:", loanDetails.current_password);
                LogWriter("Password:", loanDetails.password);

                string lcToken = tokenServices.GetLctoken(MobileToken);

                var lenthPassWord = loanDetails.password.Length;

                if (loanDetails.current_password == objPWd)
                {

                    if (loanDetails.password != Password)
                    {
                        //var responseIn = await API_Connection.GetAsync(lcToken, "/api/User/GetUserInformation");
                        //string returnedDataUser = await responseIn.Content.ReadAsStringAsync();
                        //dynamic getuserinfo = JsonConvert.DeserializeObject(returnedDataUser);
                        string UserName = objusername;
                        string User_Name = UserName.Trim();
                        byte[] password = System.Text.ASCIIEncoding.ASCII.GetBytes(loanDetails.password);
                        string decodedStringpassword = System.Convert.ToBase64String(password);

                        byte[] userId = System.Text.ASCIIEncoding.ASCII.GetBytes(User_Name);
                        string decodedStringuserId = System.Convert.ToBase64String(userId);
                        string decodedStringexistinguserId = System.Convert.ToBase64String(userId);

                        Dictionary<string, string> someDict = new Dictionary<string, string>();
                        someDict.Add("password", decodedStringpassword);
                        LogWriter("Encodedpassword", decodedStringpassword);
                        someDict.Add("userId", decodedStringuserId);
                        LogWriter("EncodeduserId", decodedStringuserId);
                        someDict.Add("existinguserId", decodedStringexistinguserId);
                        LogWriter("EncodedexistinguserId", decodedStringexistinguserId);
                        someDict.Add("ssn", "");
                        var content = new FormUrlEncodedContent(someDict);
                        var response = await API_Connection.PostAsync(lcToken, "/api/User/UpdateUseridPassword/", content);

                        dynamic Message = await response.message.Content.ReadAsStringAsync();

                        var ErrorMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Message);

                        if (ErrorMessage.updated == false)
                        {
                            loanDetails.password = loanDetails.current_password;
                            loanDetails.Message = ErrorMessage.msg;
                            LogWriter("Message", loanDetails.Message);
                            loanDetails.is_Success = false;
                        }
                        if (ErrorMessage.updated == true)
                        {
                            loanDetails.Message = "Success";
                            LogWriter("Message", loanDetails.Message);
                            loanDetails.is_Success = true;
                        }

                        if (ErrorMessage.updated == true)
                        {
                            string freedommortageURL = ErrorMessage.client.privateLabelURL;
                            string FreedomMortage = ErrorMessage.client.clientName;
                            Dictionary<string, string> someDictMail = new Dictionary<string, string>();
                            someDictMail.Add("emailData[0][key]", "timeVal");
                            someDictMail.Add("emailData[0][value]", Convert.ToString(DateTime.Now));
                            someDictMail.Add("emailData[0][update]", "undefined");
                            someDictMail.Add("emailData[1][key]", "Url");
                            someDictMail.Add("emailData[1][value]", freedommortageURL);
                            someDictMail.Add("emailData[1][update]", "undefined");
                            someDictMail.Add("emailData[2][key]", "client");
                            someDictMail.Add("emailData[2][value]", FreedomMortage);
                            someDictMail.Add("emailData[2][update]", "undefined");
                            someDictMail.Add("emailData[3][key]", "PROPERTY_STATE_CODE");
                            someDictMail.Add("emailData[3][value]", "ME");
                            someDictMail.Add("emailData[3][update]", "undefined");
                            someDictMail.Add("update", "undefined");


                            var contentmail = new FormUrlEncodedContent(someDictMail);
                            var responsemail = await API_Connection.PostAsync(lcToken, "/api/EmailNotification/SendEmailConfirmationForTemplate/?template=UpdateUserPassword&toEmail=bGFtZXJlLm5pY2hvbGFzQGdtYWlsLmNvbQ==&pageName=manageSecurityPref-UpdateUserPassword&userID=&securityEnabled=true", contentmail);

                            var eventId = 5;
                            var resourceName = "Manage+Security+Preference";
                            var toEmail = "";
                            var log = "Viewed+Security+Preference+page+-+Update+Password";
                            var actionName = "UPDATE";

                            var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                            string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();
                        }

                        var contentregeneratedToken = new FormUrlEncodedContent(new Dictionary<string, string> { { "userID", objUId }, { "password", loanDetails.password } });
                        var responseregeneratedToken = await API_Connection.PostAsync("/api/Auth/Authenticate", contentregeneratedToken);

                        var Token = responseregeneratedToken.tokenValue;

                        var MobileTokenNew = objgenerateToken.GenerateToken(objUId, loanDetails.password, objCId, Token,objusername,resourcename,logview,eStatementenr);

                        loanDetails.Token = MobileTokenNew;

                        if (ErrorMessage.updated == false)
                        {
                            loanDetails.Message = ErrorMessage.msg;
                            LogWriter("Message", loanDetails.Message);
                            loanDetails.Token = MobileToken;
                            return new ResponseModel(loanDetails, 1, "Failed");
                        }

                        else
                        {
                            return new ResponseModel(loanDetails);
                        }

                    }


                    else
                    {
                        loanDetails.is_Success = false;
                        loanDetails.Message = "Password has been previously used";
                        LogWriter("Message", loanDetails.Message);
                        loanDetails.Token = MobileToken;
                        return new ResponseModel(loanDetails, 2, "Invalid Password");
                    }
                }
                else
                {
                    loanDetails.is_Success = false;
                    loanDetails.Message = "Invalid Current Password";
                    LogWriter("Message", loanDetails.Message);
                    loanDetails.Token = MobileToken;
                    return new ResponseModel(loanDetails, 2, "Invalid Current Password");
                }

            }

            catch (Exception Ex)
            {
                return new ResponseModel(loanDetails, 1, Ex.Message);
            }
        }


        //public async Task<ResponseModel> SetPinAsync(string MobileToken, UpdatePassword PinDetail)
        //{
        //    try
        //    {
        //        // To do - Use DI

        //        TokenServices tokenServices = new TokenServices();

        //        Business_Services.Models.GenerateNewToken objgenerateToken = new GenerateNewToken();

        //        var Decryptdata = objgenerateToken.Decrypt(MobileToken);

        //        dynamic ObjUserId = JsonConvert.DeserializeObject(Decryptdata);
        //        string objUId = ObjUserId.UserId;
        //        string objPWd = ObjUserId.Password;
        //        int objCId = ObjUserId.ClientId;
        //        string objusername = ObjUserId.UserName;
        //        string resourcename = ObjUserId.resourcename;
        //        string logview = ObjUserId.log;
        //        bool eStatementenr = ObjUserId.eStatement;
        //        string lcToken = tokenServices.GetLctoken(MobileToken);

        //        //var responseIn = await API_Connection.GetAsync(lcToken, "/api/User/GetUserInformation");
        //        //string returnedDataUser = await responseIn.Content.ReadAsStringAsync();
        //        //dynamic getuserinfo = JsonConvert.DeserializeObject(returnedDataUser);
        //        string UserName = objusername;
        //        string User_Name = UserName.Trim();


        //        byte[] Pin = System.Text.ASCIIEncoding.ASCII.GetBytes(PinDetail.Pin);
        //        string decodedStringPin = System.Convert.ToBase64String(Pin);

        //        byte[] userId = System.Text.ASCIIEncoding.ASCII.GetBytes(User_Name);
        //        string decodedStringuserId = System.Convert.ToBase64String(userId);
        //        string decodedStringexistinguserId = System.Convert.ToBase64String(userId);

        //        Dictionary<string, string> someDict = new Dictionary<string, string>();

        //        someDict.Add("password", "");
        //        someDict.Add("userId", "");
        //        someDict.Add("existinguserId", decodedStringexistinguserId);
        //        someDict.Add("ssn", "");
        //        someDict.Add("Pin", decodedStringPin);
        //        someDict.Add("OldPin", "");
        //        someDict.Add("ContactType", "");

        //        var content = new FormUrlEncodedContent(someDict);
        //        var response = await API_Connection.PostAsync(lcToken, "/api/User/UpdateUseridPassword/", content);

        //        dynamic Message = await response.message.Content.ReadAsStringAsync();

        //        var ErrorMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Message);

        //        if (ErrorMessage.msg == "Incorrect Old Pin")
        //        {
        //            PinDetail.Message = "Pin has been previously set";

        //        }
        //        if (ErrorMessage.updated == true)
        //        {

        //            Business_Services.Models.DAL.LoancareDBContext.MobileUser MUser = new Business_Services.Models.DAL.LoancareDBContext.MobileUser();
        //            using (var ctx = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
        //            {
        //                var setpin = ctx.MobileUsers.Where(s => s.User_Id == UserName).FirstOrDefault();
        //                if (setpin != null)
        //                {
        //                    using (var context = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
        //                    {
        //                        setpin.mae_steps_completed = "2";
        //                        context.Entry(setpin).State = EntityState.Modified;
        //                        context.SaveChanges();

        //                    }
        //                }
        //            }
        //            if (ErrorMessage.updated == true)
        //            {
        //                string freedommortageURL = ErrorMessage.client.privateLabelURL;
        //                string FreedomMortage = ErrorMessage.client.clientName;
        //                PinDetail.Message = ErrorMessage.msg;
        //                Dictionary<string, string> someDictMail = new Dictionary<string, string>();
        //                someDictMail.Add("emailData[0][key]", "timeVal");
        //                someDictMail.Add("emailData[0][value]", Convert.ToString(DateTime.Now));
        //                someDictMail.Add("emailData[0][update]", "undefined");
        //                someDictMail.Add("emailData[1][key]", "Url");
        //                someDictMail.Add("emailData[1][value]", freedommortageURL);
        //                someDictMail.Add("emailData[1][update]", "undefined");
        //                someDictMail.Add("emailData[2][key]", "client");
        //                someDictMail.Add("emailData[2][value]", FreedomMortage);
        //                someDictMail.Add("emailData[2][update]", "undefined");
        //                someDictMail.Add("emailData[3][key]", "PROPERTY_STATE_CODE");
        //                someDictMail.Add("emailData[3][value]", "MA");
        //                someDictMail.Add("emailData[3][update]", "undefined");
        //                someDictMail.Add("update", "undefined");


        //                var contentmail = new FormUrlEncodedContent(someDictMail);
        //                var responsemail = await API_Connection.PostAsync(lcToken, "/api/EmailNotification/SendEmailConfirmationForTemplate/?template=UpdateUserPassword&toEmail=bGFtZXJlLm5pY2hvbGFzQGdtYWlsLmNvbQ==&pageName=manageSecurityPref-UpdateUserPassword&userID=&securityEnabled=true", contentmail);
        //            }
        //        }
        //        var contentregeneratedToken = new FormUrlEncodedContent(new Dictionary<string, string> { { "userID", objUId }, { "password", objPWd } });
        //        var responseregeneratedToken = await API_Connection.PostAsync("/api/Auth/Authenticate", contentregeneratedToken);

        //        var Token = responseregeneratedToken.tokenValue;

        //        var MobileTokenNew = objgenerateToken.GenerateToken(objUId, objPWd, objCId, Token,objusername,resourcename,logview,eStatementenr);

        //        PinDetail.Token = MobileTokenNew;

        //        if (ErrorMessage.updated == false && ErrorMessage.msg != "Incorrect Old Pin")
        //        {
        //            PinDetail.Message = ErrorMessage.msg;
        //            PinDetail.Token = MobileToken;
        //            return new ResponseModel(PinDetail, 1, "Failed");
        //        }
        //        else
        //        {
        //            return new ResponseModel(PinDetail);
        //        }


        //    }
        //    catch (Exception Ex)
        //    {
        //        return new ResponseModel(PinDetail, 1, Ex.Message);
        //    }
        //}

        public async Task<ResponseModel> GetPinforMobileAsync(string lcAuthToken)
        {

            try
            {

                TokenServices tokenServices = new TokenServices();
                string lcToken = tokenServices.GetLctoken(lcAuthToken);
                var responseQuestionInfo = await API_Connection.GetAsync(lcToken, "/api/User/GetSecurtiyQuestions");
                string returnedData = await responseQuestionInfo.Content.ReadAsStringAsync();

                dynamic objQuestion = JsonConvert.DeserializeObject(returnedData);

                Business_Services.Models.UpdatePassword PinDetails = new Business_Services.Models.UpdatePassword();



                PinDetails.Pin = objQuestion.pin;
                string Pin = objQuestion.pin;

                return new ResponseModel(Pin);
            }
            catch (Exception Ex)
            {

                return new ResponseModel(null, 1, Ex.Message);
            }
        }
        public async Task<ResponseModel> SetPinAsync(string MobileToken, UpdatePassword PinDetail)
        {
            try
            {
                // To do - Use DI

                TokenServices tokenServices = new TokenServices();

                Business_Services.Models.GenerateNewToken objgenerateToken = new GenerateNewToken();

                var Decryptdata = objgenerateToken.Decrypt(MobileToken);

                dynamic ObjUserId = JsonConvert.DeserializeObject(Decryptdata);
                string objUId = ObjUserId.UserId;
                string objPWd = ObjUserId.Password;
                int objCId = ObjUserId.ClientId;
                string objusername = ObjUserId.UserName;
                string resourcename = ObjUserId.resourcename;
                string logview = ObjUserId.log;
                bool eStatementenr = ObjUserId.eStatement;
                string lcToken = tokenServices.GetLctoken(MobileToken);

                //var responseIn = await API_Connection.GetAsync(lcToken, "/api/User/GetUserInformation");
                //string returnedDataUser = await responseIn.Content.ReadAsStringAsync();
                //dynamic getuserinfo = JsonConvert.DeserializeObject(returnedDataUser);
                string UserName = objusername;
                string User_Name = UserName.Trim();

                var responseQuestionInfo = await API_Connection.GetAsync(lcToken, "/api/User/GetSecurtiyQuestions/");
                string returnedData = await responseQuestionInfo.Content.ReadAsStringAsync();

                dynamic objQuestion = JsonConvert.DeserializeObject(returnedData);

                // Business_Services.Models.SecurityQuestion questionDetails = new Business_Services.Models.SecurityQuestion();

                // questionDetails.questions = new List<SecurityQuestionSummary>();


                string OldPinuser = objQuestion.pin;
                if (OldPinuser == "")
                {
                    byte[] Pin = System.Text.ASCIIEncoding.ASCII.GetBytes(PinDetail.Pin);
                    string decodedStringPin = System.Convert.ToBase64String(Pin);

                    byte[] userId = System.Text.ASCIIEncoding.ASCII.GetBytes(User_Name);
                    string decodedStringuserId = System.Convert.ToBase64String(userId);
                    string decodedStringexistinguserId = System.Convert.ToBase64String(userId);

                    Dictionary<string, string> someDict = new Dictionary<string, string>();

                    someDict.Add("password", "");
                    someDict.Add("userId", "");
                    someDict.Add("existinguserId", decodedStringexistinguserId);
                    someDict.Add("ssn", "");
                    someDict.Add("Pin", decodedStringPin);
                    someDict.Add("OldPin", "");
                    someDict.Add("ContactType", "");

                    var content = new FormUrlEncodedContent(someDict);
                    var response = await API_Connection.PostAsync(lcToken, "/api/User/UpdateUseridPassword/", content);

                    dynamic Message = await response.message.Content.ReadAsStringAsync();

                    var ErrorMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Message);

                    if (ErrorMessage.updated == true)
                    {

                        Business_Services.Models.DAL.LoancareDBContext.MobileUser MUser = new Business_Services.Models.DAL.LoancareDBContext.MobileUser();
                        using (var ctx = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
                        {
                            var setpin = ctx.MobileUsers.Where(s => s.User_Id == UserName).FirstOrDefault();
                            if (setpin != null)
                            {
                                using (var context = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
                                {
                                    setpin.mae_steps_completed = "2";
                                    context.Entry(setpin).State = EntityState.Modified;
                                    context.SaveChanges();

                                }
                            }
                        }
                    }
                    if (ErrorMessage.updated == true)
                    {

                        var responsepropertyCode = await API_Connection.GetAsync(lcToken, "/api/Helper/GetStatePropertyCode/?loanNo=" + PinDetail.User_Id);
                        string returnedpropertyCode = await responsepropertyCode.Content.ReadAsStringAsync();
                        dynamic propertycode = JsonConvert.DeserializeObject(returnedpropertyCode);

                        string freedommortageURL = ErrorMessage.client.privateLabelURL;
                        string FreedomMortage = ErrorMessage.client.clientName;
                        PinDetail.Message = ErrorMessage.msg;
                        Dictionary<string, string> someDictMail = new Dictionary<string, string>();
                        someDictMail.Add("emailData[0][key]", "timeVal");
                        someDictMail.Add("emailData[0][value]", Convert.ToString(DateTime.Now));
                        someDictMail.Add("emailData[0][update]", "undefined");
                        someDictMail.Add("emailData[1][key]", "Url");
                        someDictMail.Add("emailData[1][value]", freedommortageURL);
                        someDictMail.Add("emailData[1][update]", "undefined");
                        someDictMail.Add("emailData[2][key]", "client");
                        someDictMail.Add("emailData[2][value]", FreedomMortage);
                        someDictMail.Add("emailData[2][update]", "undefined");
                        someDictMail.Add("emailData[3][key]", "PROPERTY_STATE_CODE");
                        someDictMail.Add("emailData[3][value]", propertycode);
                        someDictMail.Add("emailData[3][update]", "undefined");
                        someDictMail.Add("update", "undefined");


                        var contentmail = new FormUrlEncodedContent(someDictMail);
                        var responsemail = await API_Connection.PostAsync(lcToken, "/api/EmailNotification/SendEmailConfirmationForTemplate/?template=UpdateUserPassword&toEmail=bGFtZXJlLm5pY2hvbGFzQGdtYWlsLmNvbQ==&pageName=manageSecurityPref-UpdateUserPassword&userID=&securityEnabled=true", contentmail);
                        var contentregeneratedToken = new FormUrlEncodedContent(new Dictionary<string, string> { { "userID", objUId }, { "password", objPWd } });
                        var responseregeneratedToken = await API_Connection.PostAsync("/api/Auth/Authenticate", contentregeneratedToken);

                        var Token = responseregeneratedToken.tokenValue;

                        var MobileTokenNew = objgenerateToken.GenerateToken(objUId, objPWd, objCId, Token, objusername, resourcename, logview, eStatementenr);

                        PinDetail.Token = MobileTokenNew;

                        if (ErrorMessage.updated == false && ErrorMessage.msg != "Incorrect Old Pin")
                        {
                            PinDetail.Message = ErrorMessage.msg;
                            PinDetail.Token = MobileToken;
                            return new ResponseModel(PinDetail, 1, "Failed");
                        }
                        else
                        {
                            return new ResponseModel(PinDetail);
                        }


                    }

                }
                if (OldPinuser != "")
                {

                    var responsepropertyCode = await API_Connection.GetAsync(lcToken, "/api/Helper/GetStatePropertyCode/?loanNo=" + PinDetail.User_Id);
                    string returnedpropertyCode = await responsepropertyCode.Content.ReadAsStringAsync();
                    dynamic propertycode = JsonConvert.DeserializeObject(returnedpropertyCode);

                    byte[] Pin = System.Text.ASCIIEncoding.ASCII.GetBytes(PinDetail.Pin);
                    string decodedStringPin = System.Convert.ToBase64String(Pin);

                    byte[] userId = System.Text.ASCIIEncoding.ASCII.GetBytes(User_Name);
                    string decodedStringuserId = System.Convert.ToBase64String(userId);
                    string decodedStringexistinguserId = System.Convert.ToBase64String(userId);

                    byte[] OldPin = System.Text.ASCIIEncoding.ASCII.GetBytes(OldPinuser);
                    string decodedStringOldPin = System.Convert.ToBase64String(OldPin);


                    Dictionary<string, string> someDictResetpin = new Dictionary<string, string>();

                    someDictResetpin.Add("password", "");
                    someDictResetpin.Add("userId", "");
                    someDictResetpin.Add("existinguserId", decodedStringexistinguserId);
                    someDictResetpin.Add("ssn", "");
                    someDictResetpin.Add("Pin", decodedStringPin);
                    someDictResetpin.Add("OldPin", decodedStringOldPin);
                    someDictResetpin.Add("ContactType", "");

                    var contentreset = new FormUrlEncodedContent(someDictResetpin);
                    var responsereset = await API_Connection.PostAsync(lcToken, "/api/User/UpdateUseridPassword/", contentreset);

                    dynamic Messagereset = await responsereset.message.Content.ReadAsStringAsync();

                    var ErrorMessagereset = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Messagereset);


                    Business_Services.Models.DAL.LoancareDBContext.MobileUser MUser = new Business_Services.Models.DAL.LoancareDBContext.MobileUser();
                    using (var ctx = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
                    {
                        var setpin = ctx.MobileUsers.Where(s => s.User_Id == UserName).FirstOrDefault();
                        if (setpin != null)
                        {
                            using (var context = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
                            {
                                setpin.mae_steps_completed = "2";
                                context.Entry(setpin).State = EntityState.Modified;
                                context.SaveChanges();

                            }
                        }
                    }
                    var contentregeneratedToken = new FormUrlEncodedContent(new Dictionary<string, string> { { "userID", objUId }, { "password", objPWd } });
                    var responseregeneratedToken = await API_Connection.PostAsync("/api/Auth/Authenticate", contentregeneratedToken);

                    var Token = responseregeneratedToken.tokenValue;

                    var MobileTokenNew = objgenerateToken.GenerateToken(objUId, objPWd, objCId, Token, objusername, resourcename, logview, eStatementenr);

                    PinDetail.Token = MobileTokenNew;

                    if (ErrorMessagereset.updated == false && ErrorMessagereset.msg != "Incorrect Old Pin")
                    {
                        PinDetail.Message = ErrorMessagereset.msg;
                        PinDetail.Token = MobileToken;
                        return new ResponseModel(PinDetail, 1, "Failed");
                    }
                    else
                    {
                        return new ResponseModel(PinDetail);
                    }
                }
                return new ResponseModel(PinDetail);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(PinDetail, 1, Ex.Message);
            }
        }

        public async Task<ResponseModel> ReSetPinAsync(string MobileToken, UpdatePassword PinDetail)
        {
            try
            {
                // To do - Use DI

                TokenServices tokenServices = new TokenServices();

                Business_Services.Models.GenerateNewToken objgenerateToken = new GenerateNewToken();

                var Decryptdata = objgenerateToken.Decrypt(MobileToken);

                dynamic ObjUserId = JsonConvert.DeserializeObject(Decryptdata);
                string objUId = ObjUserId.UserId;
                string objPWd = ObjUserId.Password;
                int objCId = ObjUserId.ClientId;
                string userName = ObjUserId.UserName;
                string resourcename = ObjUserId.resourcename;
                string logview = ObjUserId.log;
                bool eStatementenr = ObjUserId.eStatement;
                string lcToken = tokenServices.GetLctoken(MobileToken);

                //var responseIn = await API_Connection.GetAsync(lcToken, "/api/User/GetUserInformation");
                //string returnedDataUser = await responseIn.Content.ReadAsStringAsync();
                //dynamic getuserinfo = JsonConvert.DeserializeObject(returnedDataUser);
                string UserName = userName;
                string User_Name = UserName.Trim();

                var responseQuestionInfo = await API_Connection.GetAsync(lcToken, "/api/User/GetSecurtiyQuestions/");
                string returnedData = await responseQuestionInfo.Content.ReadAsStringAsync();

                dynamic objQuestion = JsonConvert.DeserializeObject(returnedData);

               // Business_Services.Models.SecurityQuestion questionDetails = new Business_Services.Models.SecurityQuestion();

               // questionDetails.questions = new List<SecurityQuestionSummary>();


               string OldPinuser = objQuestion.pin;

                //byte[] password = System.Text.ASCIIEncoding.ASCII.GetBytes(PinDetail.password);
                //string decodedStringpassword = System.Convert.ToBase64String(password);

                byte[] Pin = System.Text.ASCIIEncoding.ASCII.GetBytes(PinDetail.Pin);
                string decodedStringPin = System.Convert.ToBase64String(Pin);

                byte[] OldPin = System.Text.ASCIIEncoding.ASCII.GetBytes(OldPinuser);
                string decodedStringOldPin = System.Convert.ToBase64String(OldPin);

                byte[] userId = System.Text.ASCIIEncoding.ASCII.GetBytes(User_Name);
                string decodedStringuserId = System.Convert.ToBase64String(userId);
                string decodedStringexistinguserId = System.Convert.ToBase64String(userId);

                Dictionary<string, string> someDict = new Dictionary<string, string>();

                someDict.Add("password", "");
                someDict.Add("userId", "");
                someDict.Add("existinguserId", decodedStringexistinguserId);
                someDict.Add("ssn", "");
                someDict.Add("Pin", decodedStringPin);
                someDict.Add("OldPin", decodedStringOldPin);
                someDict.Add("ContactType", "");

                var content = new FormUrlEncodedContent(someDict);
                var response = await API_Connection.PostAsync(lcToken, "/api/User/UpdateUseridPassword/", content);

                dynamic Message = await response.message.Content.ReadAsStringAsync();

                var ErrorMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Message);

                var responsepropertyCode = await API_Connection.GetAsync(lcToken, "/api/Helper/GetStatePropertyCode/?loanNo=" + PinDetail.User_Id);
                string returnedpropertyCode = await responsepropertyCode.Content.ReadAsStringAsync();
                dynamic propertycode = JsonConvert.DeserializeObject(returnedpropertyCode);


                if (ErrorMessage.updated == true)
                {
                    string freedommortageURL = ErrorMessage.client.privateLabelURL;
                    string FreedomMortage = ErrorMessage.client.clientName;
                    PinDetail.Message = ErrorMessage.updated;
                    Dictionary<string, string> someDictMail = new Dictionary<string, string>();
                    someDictMail.Add("emailData[0][key]", "timeVal");
                    someDictMail.Add("emailData[0][value]", Convert.ToString(DateTime.Now));
                    someDictMail.Add("emailData[0][update]", "undefined");
                    someDictMail.Add("emailData[1][key]", "Url");
                    someDictMail.Add("emailData[1][value]", freedommortageURL);
                    someDictMail.Add("emailData[1][update]", "undefined");
                    someDictMail.Add("emailData[2][key]", "client");
                    someDictMail.Add("emailData[2][value]", FreedomMortage);
                    someDictMail.Add("emailData[2][update]", "undefined");
                    someDictMail.Add("emailData[3][key]", "PROPERTY_STATE_CODE");
                    someDictMail.Add("emailData[3][value]", propertycode);
                    someDictMail.Add("emailData[3][update]", "undefined");
                    someDictMail.Add("update", "undefined");


                    var contentmail = new FormUrlEncodedContent(someDictMail);
                    var responsemail = await API_Connection.PostAsync(lcToken, "/api/EmailNotification/SendEmailConfirmationForTemplate/?template=UpdateUserPin&toEmail=c2NyZWRwYXRoQGdtYWlsLmNvbQ==&pageName=manageSecurityPref-UpdateUserPin&userID=&securityEnabled=false", contentmail);
                }

                var contentregeneratedToken = new FormUrlEncodedContent(new Dictionary<string, string> { { "userID", objUId }, { "password", objPWd } });
                var responseregeneratedToken = await API_Connection.PostAsync("/api/Auth/Authenticate", contentregeneratedToken);

                var Token = responseregeneratedToken.tokenValue;

                var MobileTokenNew = objgenerateToken.GenerateToken(objUId, objPWd, objCId, Token,UserName,resourcename,logview,eStatementenr);

                PinDetail.Token = MobileTokenNew;

                if (ErrorMessage.updated == false)
                {
                    PinDetail.Message = ErrorMessage.msg;
                    PinDetail.Token = MobileToken;
                    return new ResponseModel(PinDetail, 1, "Failed");
                }
                else
                {
                    return new ResponseModel(PinDetail);
                }
            }
            catch (Exception Ex)
            {
                return new ResponseModel(PinDetail, 1, Ex.Message);
            }
        }



        public async Task<ResponseModel> getpinAsync(string MobileToken, string loanNumber, string pin)
        {
            Business_Services.Models.User getpinloan = new Models.User();
            TokenServices tokenServices = new TokenServices();
            try
            {
                string lcToken = tokenServices.GetLctoken(MobileToken);

                var responsePin = await API_Connection.GetAsync(lcToken, "/api/User/GetMPSecurtiyQuestionsPostLogin/");
                string returnedDataPin = await responsePin.Content.ReadAsStringAsync();
                dynamic getuserinfo = JsonConvert.DeserializeObject(returnedDataPin);


                if (pin == getuserinfo.pin)
                {

                    getpinloan.is_successful = true;
                }
                else
                {
                    // setpin.is_successful = false;
                }
                // }
                return new ResponseModel(getuserinfo.pin);
            }
            catch (Exception Ex)
            {

                return new ResponseModel(null, 1, Ex.Message);
            }
        }


        public ResponseModel UpdateProfile(string user_id, Business_Services.Models.User user)
        {
            return new ResponseModel(user);
        }

        //    public async Task<ResponseModel> PostsetpinAsync(UpdatePassword PinDetail,string MobileToken)
        //{

        //    TokenServices tokenServices = new TokenServices();

        //    Business_Services.Models.GenerateNewToken objgenerateToken = new GenerateNewToken();
        //    var Decryptdata = objgenerateToken.Decrypt(MobileToken);
        //    dynamic ObjUserId = JsonConvert.DeserializeObject(Decryptdata);
        //    string objUId = ObjUserId.UserId;
        //    string objPWd = ObjUserId.Password;
        //    string ObjUserName = ObjUserId.UserName;
        //    int objCId = ObjUserId.ClientId;

        //    try
        //    {
        //        string lcToken = tokenServices.GetLctoken(MobileToken);

        //        string UserName = ObjUserName;
        //        string User_Name = UserName.Trim();


        //        byte[] password = System.Text.ASCIIEncoding.ASCII.GetBytes(objPWd);
        //        string decodedStringpassword = System.Convert.ToBase64String(password);

        //        byte[] Pin = System.Text.ASCIIEncoding.ASCII.GetBytes(PinDetail.Pin);
        //        string decodedStringPin = System.Convert.ToBase64String(Pin);

        //        byte[] userId = System.Text.ASCIIEncoding.ASCII.GetBytes(User_Name);
        //        string decodedStringuserId = System.Convert.ToBase64String(userId);

        //        string decodedStringexistinguserId = System.Convert.ToBase64String(userId);

        //        Dictionary<string, string> someDict = new Dictionary<string, string>();
        //        someDict.Add("password", "");
        //        someDict.Add("userId", "");
        //        someDict.Add("existinguserId", decodedStringexistinguserId);
        //        someDict.Add("ssn", "");
        //        someDict.Add("Pin", decodedStringPin);
        //        someDict.Add("OldPin", "");
        //        someDict.Add("ContactType", "");               

        //        var content = new FormUrlEncodedContent(someDict);
        //        var response = await API_Connection.PostAsync(lcToken, "/api/User/UpdateUseridPassword/", content);

        //        dynamic Message = await response.message.Content.ReadAsStringAsync();

        //        var ErrorMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Message);

        //        var contentregeneratedToken = new FormUrlEncodedContent(new Dictionary<string, string> { { "userID", objUId }, { "password", objPWd } });
        //        var responseregeneratedToken = await API_Connection.PostAsync("/api/Auth/Authenticate", contentregeneratedToken);

        //        var Token = responseregeneratedToken.tokenValue;

        //        var MobileTokenNew = objgenerateToken.GenerateToken(objUId, objPWd, objCId, Token);

        //        PinDetail.Token = MobileTokenNew;
        //        PinDetail.Message = ErrorMessage.msg;

        //        return new ResponseModel(PinDetail);
        //    }
        //    catch (Exception Ex)
        //    {

        //        return new ResponseModel(null, 1, Ex.Message);
        //    }
        //}

        //public ResponseModel ResetpinAsync(UsersMDb userDetail)
        //{
        //    Business_Services.Models.DAL.User setpin = new Models.DAL.User();
        //    Business_Services.Models.User setpinsuccess = new Models.User();
        //    try
        //    {
        //        using (var ctx = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
        //        {
        //            var setpin = ctx.MobileUsers.Where(s => s.User_Id == userDetail.User_Id && s.pin == userDetail.Old_Pin).FirstOrDefault();
        //            if (setpin != null)
        //            {
        //                using (var context = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
        //                {
        //                    setpin.pin = userDetail.New_Pin;
        //                    context.Entry(setpin).State = EntityState.Modified;
        //                    context.SaveChanges();
        //                    setpinsuccess.is_successful = true;
        //                }
        //            }
        //            else
        //            {
        //                setpinsuccess.is_successful = false;
        //            }
        //        }

        //        return new ResponseModel(setpinsuccess);
        //    }
        //    catch (Exception Ex)
        //    {
        //        return new ResponseModel(null, 1, "Invalid Pin");
        //    }
        //}

        //Modified by BBSR Team on 5th Jan 2018
        //public async Task<string> UpdateSecurityAnswers(string lcAuthToken, List<QuestionSummary> secQuestions)
        public async Task<ResponseModel> UpdateSecurityAnswers(string lcAuthToken, List<QuestionSummary> secQuestions)
        {

            //HttpContent content = null;
            TokenServices tokenServices = new TokenServices();
            string lcToken = tokenServices.GetLctoken(lcAuthToken);

            try
            {
                string sData = string.Empty;
                foreach (var secQuestion in secQuestions)
                {
                    //Modified by BBSR_Team on 2nd Jan 2017

                    string secretQuestion = secQuestion.secretQuestion;
                    byte[] secrQuestion = System.Text.ASCIIEncoding.ASCII.GetBytes(secretQuestion);
                    string decodedsecretQuestion = System.Convert.ToBase64String(secrQuestion);

                    string secretAnswer = secQuestion.securityAnswer;
                    byte[] secrAnswer = System.Text.ASCIIEncoding.ASCII.GetBytes(secretAnswer);
                    string decodedsecretAnswer = System.Convert.ToBase64String(secrAnswer);

                    sData += "$" + secQuestion.userID + ",null," + secQuestion.questionID + "," + decodedsecretQuestion + "," + decodedsecretAnswer; //+ "\\n"
                }

                var content = new System.Net.Http.StringContent(sData, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = await API_Connection.PostAsync(lcToken, "/api/User/Updatesecurityquesions/", content);
                return new ResponseModel(response);

                var eventId = 5;
                var resourceName = "Manage+Security+Preference";
                var toEmail = "";
                var log = "Manage+Security+Preference+page+-+Security+Questions";
                var actionName = "UPDATE";

                var trackresponse = await API_Connection.GetAsync("/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();
            }
            catch (Exception Ex)
            {
                return new ResponseModel(secQuestions, 1, "Error! Failed to Update Security Preferences!");
            }
        }

        public async Task<ResponseModel> InsertSecurityAnswerAsyn(string lcAuthToken, Question secQuestions, string objUserIdUpd)
        {

            //HttpContent content = null;
            TokenServices tokenServices = new TokenServices();
            string lcToken = tokenServices.GetLctoken(lcAuthToken);
            

            Business_Services.Models.GenerateNewToken objgenerateToken = new GenerateNewToken();

            var Decryptdata = objgenerateToken.Decrypt(lcAuthToken);

            dynamic ObjUser = JsonConvert.DeserializeObject(Decryptdata);
            string str_Loan = ObjUser.Loan_Number;

            try
            {
                string sData = string.Empty;
                foreach (var secQuestion in secQuestions.secquestions)
                {
                    //Modified by BBSR_Team on 2nd Jan 2017

                    string secretQuestion = secQuestion.secretQuestion;
                    byte[] secrQuestion = System.Text.ASCIIEncoding.ASCII.GetBytes(secretQuestion);
                    string decodedsecretQuestion = System.Convert.ToBase64String(secrQuestion);

                    string secretAnswer = secQuestion.securityAnswer;
                    byte[] secrAnswer = System.Text.ASCIIEncoding.ASCII.GetBytes(secretAnswer);
                    string decodedsecretAnswer = System.Convert.ToBase64String(secrAnswer);

                    sData += "$" + secQuestion.userID + ",null," + secQuestion.questionID + "," + decodedsecretQuestion + "," + decodedsecretAnswer; //+ "\\n"

                }
                var Content = new System.Net.Http.StringContent(sData, Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = await API_Connection.PostAsync(lcToken, "/api/User/UpdatePopupSecurityQuesions/", Content);

                string Updated_value = await response.message.Content.ReadAsStringAsync();
                dynamic Updated_SecurityQuesion = JsonConvert.DeserializeObject(Updated_value);

                string InsertResponse = Updated_SecurityQuesion.updated;

              
                var responsePropertystateCD = await API_Connection.GetAsync(lcToken, "/api/Helper/GetStatePropertyCode/?loanNo=" + str_Loan);
                dynamic Message = await responsePropertystateCD.Content.ReadAsStringAsync();
                var PropcodeMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Message);
                string PropertyStatecode = PropcodeMessage;

                var responseUserInfo = await API_Connection.GetAsync(lcToken, "/api/User/GetUserInformation");
                string returnedData = await responseUserInfo.Content.ReadAsStringAsync();
                dynamic objUseremail = JsonConvert.DeserializeObject(returnedData);
                string Useremail = objUseremail.currentUserLoan.emailAddress;
             
                byte[] secr_email = System.Text.ASCIIEncoding.ASCII.GetBytes(Useremail);
                string decoded_email = System.Convert.ToBase64String(secr_email);

                if (InsertResponse == "True") {

                    string freedommortageURL = Updated_SecurityQuesion.client.privateLabelURL;
                    string FreedomMortage = Updated_SecurityQuesion.client.clientName;
             
                    Dictionary<string, string> someDictMail = new Dictionary<string, string>();
                    someDictMail.Add("emailData[0][key]", "timeVal");
                    someDictMail.Add("emailData[0][value]", Convert.ToString(DateTime.Now));
                    someDictMail.Add("emailData[0][update]", "undefined");
                    someDictMail.Add("emailData[1][key]", "Url");
                    someDictMail.Add("emailData[1][value]", freedommortageURL);
                    someDictMail.Add("emailData[1][update]", "undefined");
                    someDictMail.Add("emailData[2][key]", "client");
                    someDictMail.Add("emailData[2][value]", FreedomMortage);
                    someDictMail.Add("emailData[2][update]", "undefined");
                    someDictMail.Add("emailData[3][key]", "PROPERTY_STATE_CODE");
                    someDictMail.Add("emailData[3][value]", PropertyStatecode);
                    someDictMail.Add("emailData[3][update]", "undefined");
                    someDictMail.Add("update", "undefined");
                    string Page_Name = "manageSecurityPref-UpdateUserPassword";
                    string Update_Password = "UpdateUserPassword";
                    var contentmail = new FormUrlEncodedContent(someDictMail);
                    var responsemail = await API_Connection.PostAsync(lcToken, "/api/EmailNotification/SendEmailConfirmationForTemplate/?template="+ Update_Password + "&toEmail="+ decoded_email + "&pageName="+ Page_Name + "&userID="+""+"&securityEnabled="+false, contentmail);
                    string returnedSendemail = await responsemail.message.Content.ReadAsStringAsync();
                    dynamic objSendUseremail = JsonConvert.DeserializeObject(returnedSendemail);
                }
                if (secQuestions.Delete_Flag == false)
                {
                    if (InsertResponse == "True")
                    {

                        using (var ctx = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
                        {
                            var setpin = ctx.MobileUsers.Where(s => s.User_Id == objUserIdUpd).FirstOrDefault();

                            using (var context = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
                            {
                                setpin.mae_steps_completed = "1";
                                context.Entry(setpin).State = EntityState.Modified;
                                context.SaveChanges();
                            }

                        }
                    }
                }
                return new ResponseModel(Updated_SecurityQuesion);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(secQuestions, 1, Ex.Message);
            }
        }

        //Modified by BBSR Team on 16th Jan 2018 : User Registration
        public async Task<ResponseModel> PostUserRegistrationAsync(Business_Services.Models.User userDetail)
        {
            try
            {
                string ssn = userDetail.ssn;
                byte[] userSSN = System.Text.ASCIIEncoding.ASCII.GetBytes(ssn);
                string decodeduserSSN = System.Convert.ToBase64String(userSSN);

                Dictionary<string, string> someDict = new Dictionary<string, string>();
                someDict.Add("user[ssn]", decodeduserSSN);
                someDict.Add("user[userName]", userDetail.username);
                someDict.Add("user[Password]", userDetail.password);
                someDict.Add("CurrentUserLoan[loanNo]", userDetail.loanNumber);
                someDict.Add("CurrentUserLoan[notifyEmail]", userDetail.NotifyEmail);
                someDict.Add("CurrentUserLoan[emailAddress]", userDetail.email);
                someDict.Add("CurrentUserLoan[discVer]", string.Empty);
                someDict.Add("CurrentUserLoan[discAccept]", string.Empty);

                var content = new FormUrlEncodedContent(someDict);
                var response = await API_Connection.PostUserAsync("/api/Register/CheckRegisterSso", content);

                string returnedData = await response.Content.ReadAsStringAsync();
                dynamic objUser = JsonConvert.DeserializeObject(returnedData);

                return new ResponseModel(objUser);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }

        }
        public async Task<ResponseModel> GetConfirmationAsync(Business_Services.Models.User userDetail)
        {
            try
            {
                Dictionary<string, string> someDict = new Dictionary<string, string>();
                someDict.Add("user[userID]", string.Empty);
                someDict.Add("user[ssn]", string.Empty);

                var content = new FormUrlEncodedContent(someDict);
                var response = await API_Connection.PostUserAsync("/api/Register/GetConfirmation", content);

                string returnedData = await response.Content.ReadAsStringAsync();
                dynamic objUser = JsonConvert.DeserializeObject(returnedData);

                return new ResponseModel(objUser);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }

        }
        public async Task<ResponseModel> PostsetRegistrationAsync(Business_Services.Models.User userDetail)
        {
            try
            {
                string ssn = userDetail.ssn;
                byte[] userSSN = System.Text.ASCIIEncoding.ASCII.GetBytes(ssn);
                string decodeduserSSN = System.Convert.ToBase64String(userSSN);

                Dictionary<string, string> someDict = new Dictionary<string, string>();
                someDict.Add("user[ssn]", decodeduserSSN);
                someDict.Add("user[userName]", userDetail.username);
                someDict.Add("user[Password]", userDetail.password);
                someDict.Add("CurrentUserLoan[loanNo]", userDetail.loanNumber);
                someDict.Add("CurrentUserLoan[notifyEmail]", userDetail.NotifyEmail);
                someDict.Add("CurrentUserLoan[emailAddress]", userDetail.email);
                someDict.Add("CurrentUserLoan[discVer]", userDetail.discVer);
                someDict.Add("CurrentUserLoan[discAccept]", userDetail.discAccept);

                var content = new FormUrlEncodedContent(someDict);
                var response = await API_Connection.PostUserAsync("/api/Register/SetRegister/", content);

                string returnedData = await response.Content.ReadAsStringAsync();
               // var replacedText = returnedData.Replace("'", "");
                //dynamic objUser = Newtonsoft.Json.Linq.JObject.Parse(replacedText);
              //  dynamic objUser = JsonConvert.DeserializeObject(replacedText);

                IEnumerable<string> tokenValues;
                string tokenValue = "";
                if (response.Headers.TryGetValues("AuthorizationToken", out tokenValues))
                {
                    tokenValue = tokenValues.FirstOrDefault();
                }
              //  dynamic objUser = JsonConvert.DeserializeObject(returnedData);
                var ErrorMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(returnedData);

                var responsepropertyCode = await API_Connection.GetAsync("/api/Helper/GetStatePropertyCode/?loanNo=" + userDetail.loanNumber);
                string returnedpropertyCode = await responsepropertyCode.Content.ReadAsStringAsync();
                dynamic propertycode = JsonConvert.DeserializeObject(returnedpropertyCode);

                if (ErrorMessage.updated == true)
                {
                    //    string clientName = ErrorMessage.client.clientName;

                    //    string strUserName = ErrorMessage.user.userName;
                    //    byte[] userName = System.Text.ASCIIEncoding.ASCII.GetBytes(strUserName);
                    //    string decodeduserName = System.Convert.ToBase64String(userName);


                    //    string strUserID = ErrorMessage.user.userID;


                    //    string loanNo = ErrorMessage.userLoan.loanNo;
                    //    string clientPhone = ErrorMessage.client.clientPhone;
                    //    string clientUrl = "www.myloancare.com";

                    //    string strUserEmail = ErrorMessage.userLoan.emailAddress;
                    //    byte[] userEmail = System.Text.ASCIIEncoding.ASCII.GetBytes(strUserEmail);
                    //    string decodeduserEmail = System.Convert.ToBase64String(userEmail);

                    //    ////string freedommortageURL = ErrorMessage.client.privateLabelURL;
                    //    ////string FreedomMortage = ErrorMessage.client.clientName;

                    //    Dictionary<string, string> someDictMail = new Dictionary<string, string>();
                    //    someDictMail.Add("emailData[0][key]", "clientname");
                    //    someDictMail.Add("emailData[0][value]", clientName);
                    //    someDictMail.Add("emailData[0][update]", "undefined");
                    //    someDictMail.Add("emailData[1][key]", "username");
                    //    someDictMail.Add("emailData[1][value]", decodeduserName);
                    //    someDictMail.Add("emailData[1][update]", "undefined");
                    //    someDictMail.Add("emailData[2][key]", "loanNo");
                    //    someDictMail.Add("emailData[2][value]", loanNo);
                    //    someDictMail.Add("emailData[2][update]", "undefined");
                    //    someDictMail.Add("emailData[3][key]", "clientPhone");
                    //    someDictMail.Add("emailData[3][value]", clientPhone);
                    //    someDictMail.Add("emailData[3][update]", "undefined");
                    //    someDictMail.Add("emailData[3][key]", "url");
                    //    someDictMail.Add("emailData[3][value]", clientUrl);
                    //    someDictMail.Add("emailData[3][update]", "undefined");
                    //    someDictMail.Add("emailData[3][key]", "PROPERTY_STATE_CODE");
                    //    someDictMail.Add("emailData[3][value]", propertycode);
                    //    someDictMail.Add("emailData[3][update]", "undefined");
                    //    someDictMail.Add("update", "undefined");


                    //    var contentmail = new FormUrlEncodedContent(someDictMail);
                    //    var responsemail = await API_Connection.PostAsync(tokenValue, "/api/EmailNotification/SendEmailConfirmationForTemplate/?template=LoanCareRegistration&toEmail=" + decodeduserEmail + "&pageName=disclosure&userID=" + strUserID, contentmail);

                }
                    return new ResponseModel(ErrorMessage);
            }
            catch (Exception Ex)
            {

                return new ResponseModel(null, 1, Ex.Message);
            }
        }

    }
}
