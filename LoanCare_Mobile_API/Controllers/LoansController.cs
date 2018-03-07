
using Business_Services;
using Business_Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LoanCare_Mobile_API.Action_Filters;
using LoanCare_Mobile_API.Filters;
using System.Diagnostics;
using Business_Services.Models.Helpers;
using System.Threading.Tasks;
using Business_Services.Models.DAL.LoancareEntites;
using System.Web.Http.Cors;

namespace LoanCare_Mobile_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [AuthorizationRequired]
    [RoutePrefix("api/user/loan")]
    public class LoansController : ApiController
    {
        private readonly ILoanService loanService;

        public LoansController()
        {
            loanService = new LoanService(new TokenServices());
        }

        [Route("delete/payment")]
        [AcceptVerbs("DELETE")]
        public async Task<IHttpActionResult> DeletePayment(Payment paymentData)
        {
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out IEnumerable<string> tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var responseBody = await loanService.DeletePaymentAsync(tokenValue, paymentData);
            if (responseBody == null)
            {
                ;
                return NotFound();
            }
            return Ok(responseBody);
        }

        [Route("")]
        public IHttpActionResult GetAllLoans()
        {
            var basicAuthenticationIdentity = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
            if (basicAuthenticationIdentity != null)
            {
                var userId = basicAuthenticationIdentity.UserId;
                ResponseModel result = loanService.getAllLoansForUser(userId);
                return Ok(result);
            }

            return null;
        }
        [Route("GetNotification/{loan_number}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetNotification(string loan_number)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.GetNotificationforLoanAsync(tokenValue, loan_number);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("registerconfirmation")]
        [HttpPost]
        public async Task<IHttpActionResult> Confirmation(ConfirmRegistration details)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.ConfirmationforLoanAsync(tokenValue, details);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }


        [Route("{loan_number}")]
         [HttpGet]
        public async Task<IHttpActionResult> GetLoan(string loan_number)
        {
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var loan = await loanService.GetLoanAsync(tokenValue, loan_number);
            if (loan == null)
            {
                return NotFound();
            }
            return Ok(loan);
        }



       

        [Route("getpaymentdetails/{loan_number}")]
        public async Task<IHttpActionResult> GetPaymentDetails(string loan_number)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.GetPayDetailsAsync(tokenValue, loan_number);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("getstatements/{loan_number}")]
        public async Task<IHttpActionResult> getstatements(string loan_number)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.GetgetstatementsAsync(tokenValue, loan_number);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("getstatementspdf/{Date}/{loan_number}/{Key}")]
        public async Task<IHttpActionResult> getstatementspdf(string Date, string loan_number, string Key)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.GetgetstatementspdfAsync(tokenValue,loan_number,Date, Key);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("getpdfStream/{statement_url}")]
        public async Task<IHttpActionResult> getpdfStream(string statement_url)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.GetpdfstreamAsync(tokenValue, statement_url);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }



        [Route("getupcomingpayment/{loan_number}")]
        public async Task<IHttpActionResult> GetUpcomingPayment(string loan_number)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.GetUpcomingPaymentForLoan(loan_number, tokenValue);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("payment/{loan_number}/{payment_date}")]
        [HttpGet]
        public async Task<IHttpActionResult> EditPaymentAsync(string loan_number, DateTime payment_date)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.EditPaymentForLoanAsync(tokenValue, loan_number, payment_date);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("payment/{loan_number}/{payment_date}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdatePayment(string loan_number, DateTime payment_date, Payment payment)
        {
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out IEnumerable<string> tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var responseBody = await loanService.UpdatePaymentAsync(tokenValue, loan_number, payment_date, payment);
            if (responseBody == null)
            {
                return NotFound();
            }
            return Ok(responseBody);
        }

        [Route("getcalender/{loan_number}")]
        public async Task<IHttpActionResult> getcalender(string loan_number)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.GetUpcomingPaymentForLoan(loan_number, tokenValue);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("getFeeSchedule/{loan_number}")]
        public async Task<IHttpActionResult> getFeeSchedule(string loan_number)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.GetPaymentFeeSchedule(loan_number, tokenValue);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }


        [Route("makepayment")]
        [HttpPost]
        public async Task<IHttpActionResult> MakePaymentAsync(Payment paymentData)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.PostPaymentForLoanAsync(tokenValue, paymentData);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("ManageNotification")]
        [HttpPost]
        public async Task<IHttpActionResult> ManageNotification(ManageNotification manageNotification)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.PostManageNotificationForAsync(tokenValue, manageNotification);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }


        [Route("ModifyPayment")]
        [HttpPost]
        public async Task<IHttpActionResult> ModifyPaymentAsync(Payment paymentData)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.PostModifyPaymentForLoanAsync(tokenValue, paymentData);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("CancelStatement")]
        [AcceptVerbs("Post")]
        public async Task<IHttpActionResult> CancelStatement([FromBody]string loanNumber)
        {
            User estatementData = new User();
            estatementData.loanNumber = loanNumber;
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.PostCancelStatementForLoanAsync(tokenValue, estatementData);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("AddLoan")]
        [HttpPost]
        public async Task<IHttpActionResult> AddLoan(UpdateEmail loanDetails)
        {
           
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.PostAddLoanForLoanAsync(tokenValue, loanDetails);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("eStatement")]
        [HttpPost]
        public async Task<IHttpActionResult> PosteStatementdiscloserAsync([FromBody]string loanNumber)
        {
            User estatementData = new User();
            estatementData.loanNumber = loanNumber;
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.PosteStatementForLoanAsync(tokenValue, estatementData);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("updatephone")]
        [HttpPost]
        public async Task<IHttpActionResult> updatephone(LoanContactDetail ContactData)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.PostupdatephoneForAsync(tokenValue, ContactData);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("updateprofile")]
        [HttpPost]
        public async Task<IHttpActionResult> updateprofile(LoanContactDetail ContactData)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.PostupdateprofileForAsync(tokenValue, ContactData);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }
        [Route("updateemail")]
        [HttpPost]
        public async Task<IHttpActionResult> updateemail(UpdateEmail loanDetails)
        {

            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.PostupdateemailForAsync(tokenValue, loanDetails);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [Route("SandwichMenu")]
        [HttpPost]
        public async Task<IHttpActionResult> Logout()
        {

            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.PostLogoutForAsync(tokenValue);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }


        [Route("escrow/{loan_number}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEscrowDetailsAsync(string loan_number)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var escrow = await loanService.GetEscrowDetailsForLoanAsync(tokenValue, loan_number);
            if (escrow == null)
            {
                return BadRequest("Error!");
            }

            return Ok(escrow);
        }
        [Route("GetLoanData/{loan_number}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetLoanDetailsAsync(string loan_number)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var escrow = await loanService.GetLoanDetailsForLoanAsync(tokenValue, loan_number);
            if (escrow == null)
            {
                return BadRequest("Error!");
            }

            return Ok(escrow);
        }

        [Route("GetRouting/{RoutingNumber}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRounting(string RoutingNumber)
        {
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }
            var payment = await loanService.GetRountingForLoan(tokenValue, RoutingNumber);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }



        [Route("GetLoanInfo/{loan_number}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetLoanInfoAsync(string loan_number)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var escrow = await loanService.GetLoanInfoForLoanAsync(tokenValue, loan_number);
            if (escrow == null)
            {
                return BadRequest("Error!");
            }

            return Ok(escrow);
        }

        [Route("getcontactdetails/{loan_number}")]
        [HttpGet]
        public async Task<IHttpActionResult> getcontactdetails(string loan_number)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var ActivityList = await loanService.getcontactdetailsAsync(tokenValue, loan_number);
            if (ActivityList == null)
            {
                return BadRequest("Error!");
            }

            return Ok(ActivityList);
        }

        //Modified By BBSR Team on 10th Jan 2018
        [Route("getactivity/{loan_number}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetActivityAsync(string loan_number)
        {
            Debug.WriteLine("GetActivityAsync has been called");

            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var ActivityList = await loanService.GetPaymentDescriptionsForLoanAsync(tokenValue, loan_number);

            if (ActivityList == null)
            {
                return BadRequest("Error!");
            }

            return Ok(ActivityList);
        }



        //Modified By BBSR Team on 9th Jan 2018
        [Route("getactivitytypes/{loan_number}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetActivityTypesAsync(string loan_number)
        {
            Debug.WriteLine("GetActivityTypes() method is called..");

            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var ActivityTypesList = await loanService.GetPaymentHistoryForLoanAsync(tokenValue, loan_number);

            if (ActivityTypesList == null)
            {
                return BadRequest("Error!");
            }

            return Ok(ActivityTypesList);
        }

        [Route("enrollstatements/{loan_number}")]
        [HttpPost]
        public IHttpActionResult EnrollForStatements(string loan_number)
        {
            return Ok(loanService.EnrollForStatements(loan_number));
        }

        [Route("getstatement/{loan_number}/{statement_date}")]
        [HttpGet]
        public IHttpActionResult GetStatement(string loan_number, DateTime statement_date)
        {      
            return Ok(loanService.GetStatement(loan_number, statement_date));
        }

        [Route("getautodraft/{loan_number}")]
        [HttpGet]
        public IHttpActionResult GetAutodraft(string loan_number)
        {
            var returnData = loanService.GetAutodraft(loan_number);
            return Ok(returnData);
        }

        [Route("getautodraftsetup/{loan_number}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAutodraftSetupAsync(string loan_number)
        {
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var returnData = await loanService.GetAutodraftSetupAsync(tokenValue, loan_number);
            if (returnData != null)
            {
                return Ok(returnData);
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [Route("autodraft")]
        [HttpPut]
        public async Task<IHttpActionResult> PostAutoDraft(AutodraftInsert autoDraft)
        {
            Debug.WriteLine("InsertAutoDraftAsync() is invoked..");
            // To do - Move the following code to a single method & use it across the project
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out IEnumerable<string> tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var returnData = await loanService.PostAutoDraftAsync(tokenValue, autoDraft);
            if (returnData != null)
            {
                return Ok(returnData);
            }
            else
            {
                return BadRequest("Error!");
            }
        }
        [Route("Insertautodraft")]
        [HttpPut]
        public async Task<IHttpActionResult> Insertautodraft(AutodraftInsert autoDraft)
        {
            Debug.WriteLine("UpdateAutoDraftAsync() is invoked..");
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out IEnumerable<string> tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var returnData = await loanService.InsertAutoDraftAsync(tokenValue, autoDraft);
            if (returnData != null)
            {
                return Ok(returnData);
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [Route("PostBankAccount")]
        [HttpPost]
        public async Task<IHttpActionResult> PostBankAccount(Business_Services.Models.BankAccount BankAccountdetails)
        {
            Debug.WriteLine("UpdateAutoDraftAsync() is invoked..");
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out IEnumerable<string> tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var returnData = await loanService.PostBankAccountAsync(tokenValue, BankAccountdetails);
            if (returnData != null)
            {
                return Ok(returnData);
            }
            else
            {
                return BadRequest("Error!");
            }
        }


        [Route("autodraft/{loan_number}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAutoDraftAsync(string loan_number, AutoDraft autoDraft)
        {
            Debug.WriteLine("UpdateAutoDraftAsync() is invoked..");
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out IEnumerable<string> tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var returnData = await loanService.UpdateAutoDraftAsync(tokenValue, loan_number, autoDraft);
            if (returnData != null)
            {
                return Ok(returnData);
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [Route("autodraft/{loan_number}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAutoDraftAsync(string loan_number)
        {
            // To do - Move the following code to a single method & use it across the project
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out IEnumerable<string> tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var returnData = await loanService.DeleteAutoDraftAsync(tokenValue, loan_number);
            if (returnData != null)
            {
                return Ok(returnData);
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [Route("getpendingpayments/{loan_number}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPendingPaymentsAsync(string loan_number)
        {
    
            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var PendingList = await loanService.GetPendingPaymentsAsync(tokenValue, loan_number);


            if (PendingList == null)
            {
                return BadRequest("Error!");
            }

            return Ok(PendingList);
        }

        

        [Route("getmakepaymentpendinglist/{loan_number}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMakePaymentPendingListAsync(string loan_number)
        {
            Debug.WriteLine("GetPaymentCounter has been called");

            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var paymentCounter = await loanService.GetMakePaymentPendingList(tokenValue, loan_number);


            if (paymentCounter == null)
            {
                return BadRequest("Error!");
            }

            return Ok(paymentCounter);
        }

        [Route("validatePassword")]
        [HttpPost]
        public async Task<IHttpActionResult> ValidatePasswordAsync(UserAuth userData)
        {

            // To do - Move the following code to a single method & use it across the project
            IEnumerable<string> tokenValues;
            string tokenValue = "";
            if (Request.Headers.TryGetValues("AuthorizationToken", out tokenValues))
            {
                tokenValue = tokenValues.FirstOrDefault();
            }

            var PendingList = await loanService.ValidatePasswordAsync(tokenValue, userData);


            if (PendingList == null)
            {
                return BadRequest("Error!");
            }

            return Ok(PendingList);
        }
    }
}

