using Business_Services.Models;
using Business_Services.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Services.Models.DAL.LoancareEntites;
using Business_Services.Models.LC_WebApi_Models;

namespace Business_Services
{
    public interface ILoanService
    {
        Task<ResponseModel> DeletePaymentAsync(string mobileToken, Payment paymentData);
        Task<ResponseModel> InsertAutoDraftAsync(string MobileToken, AutodraftInsert autoDraft);
        ResponseModel getAllLoansForUser(int userId);
        Task<ResponseModel> PostBankAccountAsync(string MobileToken,Business_Services.Models.BankAccount BankAccountDetails);
        Task<ResponseModel> GetLoanAsync(string mobileToken, string loanNumber);
        Task<ResponseModel> GetPaymentHistoryForLoanAsync(string mobileToken, string loanNumber);
        Task<ResponseModel> GetPayDetailsAsync(string mobileToken, string loanNumber);

        Task<ResponseModel> GetUpcomingPaymentForLoan(string loanNumber, string MobileToken);

        Task<ResponseModel> PostModifyPaymentForLoanAsync(string MobileToken, Business_Services.Models.Payment ModifyPayment);
        Task<ResponseModel> EditPaymentForLoanAsync(string mobileToken, string loanNumber, DateTime schdate);

        Task<ResponseModel> GetEscrowDetailsForLoanAsync(string mobileToken, string loanNumber);
        Task<ResponseModel> GetPaymentFeeSchedule(string loanNumber, string MobileToken);

        Task<ResponseModel> PostPaymentForLoanAsync(string mobileToken, Payment paymentDetails);

        Task<ResponseModel> GetPaymentDescriptionsForLoanAsync(string mobileToken, string loanNumber);

        ResponseModel EnrollForStatements(string loanNumber);

        ResponseModel GetStatement(string loanNumber, DateTime statement_date);

        Task<ResponseModel> GetAutodraftSetupAsync(string MobileToken, string loanNumber);

        ResponseModel GetAutodraft(string loanNumber);

        Task<ResponseModel> PostAutoDraftAsync(string MobileToken, AutodraftInsert autoDraft);

        Task<ResponseModel> UpdateAutoDraftAsync(string MobileToken, string loanNumber, AutoDraft autoDraft);

        Task<ResponseModel> DeleteAutoDraftAsync(string MobileToken, string loanNumber);

        Task<ResponseModel> GetPendingPaymentsAsync(string mobileToken, string loanNumber);

        ResponseModel GetContactDetails(string loanNumber);

       
        Task<ResponseModel> GetLoanDetailsForLoanAsync(string lcAuthToken, string loan_number);
        Task<ResponseModel> getcontactdetailsAsync(string lcAuthToken, string loan_number);
        Task<ResponseModel> PostupdateemailForAsync(string MobileToken, UpdateEmail loanDetails);
        Task<ResponseModel> PostupdatephoneForAsync(string MobileToken, LoanContactDetail LoanContactDetail);
        Task<ResponseModel> PostupdateprofileForAsync(string MobileToken, LoanContactDetail LoanContactDetail);
        Task<ResponseModel> PosteStatementForLoanAsync(string MobileToken, Business_Services.Models.User estatementdetails);
        Task<ResponseModel> PostCancelStatementForLoanAsync(string MobileToken, Business_Services.Models.User Cstatementdetails);
        Task<ResponseModel> UpdatePaymentAsync(string tokenValue, string loan_number, DateTime payment_date, Payment payment);
        Task<ResponseModel> PostLogoutForAsync(string MobileToken);
        Task<ResponseModel> GetgetstatementsAsync(string lcAuthToken, string loan_number);
        Task<ResponseModel> GetLoanInfoForLoanAsync(string lcAuthToken, string loan_number);
        Task<ResponseModel> PostAddLoanForLoanAsync(string MobileToken, UpdateEmail loanDetails);
        Task<ResponseModel> GetRountingForLoan(string lcAuthToken, string RoutingNumber);
        Task<ResponseModel> GetMakePaymentPendingList(string MobileToken, string loanNumber);
        Task<ResponseModel> GetNotificationforLoanAsync(string mobileToken, string loanNumber);

        Task<ResponseModel> ConfirmationforLoanAsync(string MobileToken, ConfirmRegistration details);

        Task<ResponseModel> PostManageNotificationForAsync(string MobileToken, Business_Services.Models.ManageNotification ModifyPayment);
      
        Task<ResponseModel> GetpdfstreamAsync(string lcAuthToken, string statement_url);
    }
}
