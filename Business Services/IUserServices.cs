using Business_Services.Models;
using Business_Services.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Services.Models.DAL.LoancareEntites;
using Business_Services.Models.DAL;

namespace Business_Services
{
    public interface IUserServices
    {
        // To do - Change the return type to user
        
        Task<ResponseModel> getUserDetailsAsyn(string lcAuthToken, string loan_number);
        Task<ResponseModel> GetBankAccountsForUser(string mobileToken);
        Task<ResponseModel> ContactUsAsync(string mobileToken);
        ResponseModel UpdateProfile(string user_id, Business_Services.Models.User user);
        Task<ResponseModel> DeleteBankAccountsForUser(string mobileToken, BankAccount objBank);
        //ResponseModel ResetpinAsync(UsersMDb userDetail);
        //Modified by BBSR Team on 5th Jan 2018
        Task<ResponseModel> UpdateSecurityAnswers(string lcAuthToken, List<QuestionSummary> secQuestions);
        ResponseModel SendUserIdByMail(string loan_number, string last4digits_ssn);
        ResponseModel SendPasswordByMail(string user_id);
        ResponseModel GetPrivacyForUser(string user_id);
        //Task<ResponseModel> PostsetpinAsync(UpdatePassword PinDetail, string MobileToken);
        Task<ResponseModel> GetManageAccountForUser(string mobileToken, string LoanNumber);
        Task<ResponseModel> getUserDetailsAsync(string lcAuthToken, string loan_number, bool Is_New_MobileUser);
        Task<ResponseModel> GetSecurityQuestions(string lcAuthToken);
        Task<ResponseModel> ForgotPassword(string Loan_Number, string ssn);
        //Added by BBSR_Team on 9th Jan 2018
        Task<ResponseModel> ForgotUser_Id(string Loan_Number, string SSN);
        Task<ResponseModel> InsertSecurityAnswerAsyn(string lcAuthToken, Question secQuestions, string objUserIdUpd);
        Task<ResponseModel> getpinAsync(string MobileToken, string loanNumber, string pin);
        Task<ResponseModel> myloanGetPinAsyn(string lcAuthToken, string Pin);
        Task<ResponseModel> UpdatePasswordAsync(string MobileToken, UpdatePassword loanDetails, string Password);
        Task<ResponseModel> GetPushNotificationForUser(string mobileToken, PushNotificationUser pushNotification);
        Task<ResponseModel> ReSetPinAsync(string MobileToken, UpdatePassword PinDetail);
        //Modified by BBSR Team on 16th Jan 2018 : User Registration
       Task<ResponseModel> PostUserRegistrationAsync(Business_Services.Models.User userDetail);
        Task<ResponseModel> GetConfirmationAsync(Business_Services.Models.User userDetail);
        Task<ResponseModel> PostsetRegistrationAsync(Business_Services.Models.User userDetail);
        Task<ResponseModel> GetRefereshToken(string MobileToken, string loannumber, string password);
        //Modified by BBSR Team on 16th Jan 2018 : User Registration

        //Modified by BBSR Team on 18th Jan 2018 : Forgot User / Password
        Task<ResponseModel> ForgotUser_Id(Business_Services.Models.User userDetail);
       // Task<ResponseModel> ForgotPassword(Business_Services.Models.User userDetail);
        Task<ResponseModel> ValidateSecurityAnswer(Business_Services.Models.User userDetail);
        Task<ResponseModel> ResetSendPassword(Business_Services.Models.User userDetail);
        Task<ResponseModel> SetPinAsync(string MobileToken, UpdatePassword loanDetails);
        //Modified by BBSR Team on 18th Jan 2018 : Forgot User / Password
        Task<string> trackinglog(string lcAuthToken, Tracking tracking);
        Task<ResponseModel> GetPinforMobileAsync(string MobileToken);
    }
}
