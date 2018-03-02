using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Services.Models.Helpers;
using Business_Services.Models;
using Business_Services.B2C_WebAPI;
using Newtonsoft.Json;
using Business_Services.Models.LC_WebApi_Models;
using System.Net;
using System.Diagnostics;
using System.Net.Http;

namespace Business_Services
{
    public class AppService : IAppService
    {
        private readonly ITokenServices tokenServices;

        public AppService(ITokenServices _tokenServices)
        {
            tokenServices = _tokenServices;
        }
        public ResponseModel GetDraftDateDelay()
        {
            return new ResponseModel(new List<string>() {
                "On payment due date",
                "1 day after due date",
                "2 days after due date",
                "3 days after due date",
                "4 days after due date",
                "5 days after due date",
                "6 days after due date",
                "7 days after due date",
                "8 days after due date",
                "9 days after due date",
                "10 days after due date",
                "11 days after due date",
                "12 days after due date",
                "13 days after due date",
                "14 days after due date",
                "15 days after due date"
            });
        }

        public ResponseModel GetAlertDetailsAsync(string loanNumber)
        {
            // To do - Use DI

            try
            {
                List<Loan_alert> objLiAlert = new List<Loan_alert>();

                //objLiAlert.Add(new Loan_alert() { alert_id = "1234", message_body = "Dear Homeowner,<br><br>Online Payment Notification Notice<br>Date: 15/08/2017 10:22:00 AM<br>Last 4 digits of the mortgage account: 8402<br>Amount (subject to fund availability): $1,762.00<br>Last 4 digits of bank account: 7119<br>Confirmation number: 8662120316<br><br>This is a system generated email. Please do not reply to this email. This mailbox is used for out-bound communication only.<br><br>Should you need further assistance, please go to myloancare.com, or contact us at 1-800-274-6600. Thank you for your business.", message_date = Convert.ToDateTime("2017-08-15T10:22Z"), message_title = "A payment was posted to your account", alert_type = "", read_status = "Read" });
                //objLiAlert.Add(new Loan_alert() { alert_id = "1235", message_body = "Dear Homeowner,<br><br>Online Payment Notification Notice<br>Date: 16/09/2017 10:22:00 AM<br>Last 4 digits of the mortgage account: 8402<br>Amount (subject to fund availability): $1,762.00<br>Last 4 digits of bank account: 7119<br>Confirmation number: 8662120316<br><br>This is a system generated email. Please do not reply to this email. This mailbox is used for out-bound communication only.<br><br>Should you need further assistance, please go to myloancare.com, or contact us at 1-800-274-6600. Thank you for your business.", message_date = Convert.ToDateTime("2017-09-16T10:22Z"), message_title = "A payment was posted to your account", alert_type = "", read_status = "NotRead" });
                //objLiAlert.Add(new Loan_alert() { alert_id = "1236", message_body = "Dear Homeowner,<br><br>Online Payment Notification Notice<br>Date: 17/10/2017 10:22:00 AM<br>Last 4 digits of the mortgage account: 8402<br>Amount (subject to fund availability): $1,762.00<br>Last 4 digits of bank account: 7119<br>Confirmation number: 8662120316<br><br>This is a system generated email. Please do not reply to this email. This mailbox is used for out-bound communication only.<br><br>Should you need further assistance, please go to myloancare.com, or contact us at 1-800-274-6600. Thank you for your business.", message_date = Convert.ToDateTime("2017-10-17T10:22Z"), message_title = "A payment was posted to your account", alert_type = "", read_status = "Read" });
                //objLiAlert.Add(new Loan_alert() { alert_id = "1237", message_body = "Dear Homeowner,<br><br>Online Payment Notification Notice<br>Date: 17/10/2017 10:22:00 AM<br>Last 4 digits of the mortgage account: 8402<br>Amount (subject to fund availability): $1,762.00<br>Last 4 digits of bank account: 7119<br>Confirmation number: 8662120316<br><br>This is a system generated email. Please do not reply to this email. This mailbox is used for out-bound communication only.<br><br>Should you need further assistance, please go to myloancare.com, or contact us at 1-800-274-6600. Thank you for your business.", message_date = Convert.ToDateTime("2017-10-17T10:22Z"), message_title = "A payment was posted to your account", alert_type = "", read_status = "Read" });
                //objLiAlert.Add(new Loan_alert() { alert_id = "1237", message_body = "Dear Homeowner,<br><br>Online Payment Notification Notice<br>Date: 17/10/2017 10:22:00 AM<br>Last 4 digits of the mortgage account: 8402<br>Amount (subject to fund availability): $1,762.00<br>Last 4 digits of bank account: 7119<br>Confirmation number: 8662120316<br><br>This is a system generated email. Please do not reply to this email. This mailbox is used for out-bound communication only.<br><br>Should you need further assistance, please go to myloancare.com, or contact us at 1-800-274-6600. Thank you for your business.", message_date = Convert.ToDateTime("2017-11-18T10:22Z"), message_title = "A payment was posted to your account", alert_type = "", read_status = "NotRead" });
                return new ResponseModel(objLiAlert);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }
        }

        public ResponseModel DeleteAlertDetailsAsync(string Alert_id)
        {
            // To do - Use DI

            try
            {
                List<Loan_alert> objLiAlert = new List<Loan_alert>();

                objLiAlert.Add(new Loan_alert() { alert_id = "1234", message_body = "Dear Homeowner,<br><br>Online Payment Notification Notice<br>Date: 15/08/2017 10:22:00 AM<br>Last 4 digits of the mortgage account: 8402<br>Amount (subject to fund availability): $1,762.00<br>Last 4 digits of bank account: 7119<br>Confirmation number: 8662120316<br><br>This is a system generated email. Please do not reply to this email. This mailbox is used for out-bound communication only.<br><br>Should you need further assistance, please go to myloancare.com, or contact us at 1-800-274-6600. Thank you for your business.", message_date = Convert.ToDateTime("2017-08-15T10:22Z"), message_title = "A payment was posted to your account", alert_type = "", read_status = "Read" });
                objLiAlert.Add(new Loan_alert() { alert_id = "1235", message_body = "Dear Homeowner,<br><br>Online Payment Notification Notice<br>Date: 16/09/2017 10:22:00 AM<br>Last 4 digits of the mortgage account: 8402<br>Amount (subject to fund availability): $1,762.00<br>Last 4 digits of bank account: 7119<br>Confirmation number: 8662120316<br><br>This is a system generated email. Please do not reply to this email. This mailbox is used for out-bound communication only.<br><br>Should you need further assistance, please go to myloancare.com, or contact us at 1-800-274-6600. Thank you for your business.", message_date = Convert.ToDateTime("2017-09-16T10:22Z"), message_title = "A payment was posted to your account", alert_type = "", read_status = "NotRead" });
                objLiAlert.Add(new Loan_alert() { alert_id = "1236", message_body = "Dear Homeowner,<br><br>Online Payment Notification Notice<br>Date: 17/10/2017 10:22:00 AM<br>Last 4 digits of the mortgage account: 8402<br>Amount (subject to fund availability): $1,762.00<br>Last 4 digits of bank account: 7119<br>Confirmation number: 8662120316<br><br>This is a system generated email. Please do not reply to this email. This mailbox is used for out-bound communication only.<br><br>Should you need further assistance, please go to myloancare.com, or contact us at 1-800-274-6600. Thank you for your business.", message_date = Convert.ToDateTime("2017-10-17T10:22Z"), message_title = "A payment was posted to your account", alert_type = "", read_status = "Read" });
                objLiAlert.Add(new Loan_alert() { alert_id = "1237", message_body = "Dear Homeowner,<br><br>Online Payment Notification Notice<br>Date: 17/10/2017 10:22:00 AM<br>Last 4 digits of the mortgage account: 8402<br>Amount (subject to fund availability): $1,762.00<br>Last 4 digits of bank account: 7119<br>Confirmation number: 8662120316<br><br>This is a system generated email. Please do not reply to this email. This mailbox is used for out-bound communication only.<br><br>Should you need further assistance, please go to myloancare.com, or contact us at 1-800-274-6600. Thank you for your business.", message_date = Convert.ToDateTime("2017-10-17T10:22Z"), message_title = "A payment was posted to your account", alert_type = "", read_status = "Read" });
                objLiAlert.Add(new Loan_alert() { alert_id = "1237", message_body = "Dear Homeowner,<br><br>Online Payment Notification Notice<br>Date: 17/10/2017 10:22:00 AM<br>Last 4 digits of the mortgage account: 8402<br>Amount (subject to fund availability): $1,762.00<br>Last 4 digits of bank account: 7119<br>Confirmation number: 8662120316<br><br>This is a system generated email. Please do not reply to this email. This mailbox is used for out-bound communication only.<br><br>Should you need further assistance, please go to myloancare.com, or contact us at 1-800-274-6600. Thank you for your business.", message_date = Convert.ToDateTime("2017-11-18T10:22Z"), message_title = "A payment was posted to your account", alert_type = "", read_status = "NotRead" });

                var Deleteid = from alertdid in objLiAlert
                                           where alertdid.alert_id.Contains(Alert_id)
                                           select new {
                                               alertdid.alert_id
                                           };
                if (Deleteid == null)
                {
                    return new ResponseModel(objLiAlert, 1, "Failed");
                }
                else if (Deleteid != null) {

                    return new ResponseModel(objLiAlert, 0, "Success");
                }

                return new ResponseModel(objLiAlert);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }
        }

        public ResponseModel GetLegalTermsPrivacy(string type)
        {
            // Note - Privacy text is specific to the client to which the user belongs
            
            LegalPrivacyTerms lpttext = new LegalPrivacyTerms();
            try
            {
               

                using (var ctx = new Business_Services.Models.DAL.LoancareDBContext.MDBService())
                {
                    var getlpttext = ctx.LegalPrivacyTerms.Where(s => s.Type == type).FirstOrDefault();
                    lpttext.GetLegalPrivacyText = getlpttext.Formatted_Text;
                   
                }

                return new ResponseModel(lpttext);

            }

            catch (Exception ex)
            {
                return new ResponseModel(null, 1, ex.Message);
            }

        }
        public ResponseModel GetHelpText()
        {           

            //Modified and Added $$ for Answer by BBSR_Team on 3rd January 2018 : Use different delimeter for question and answer
            List<FAQ> faq = new List<FAQ>()
            {
                new FAQ
                {
                    heading="General Inquiries",
                    detail="---Q1. I have been charged a fee that I do not understand; what can I do? $$ A1. You can view a description of fees by logging into your account. If you would like additional information about the fees listed on your account, please contact our customer service department.--- Q2. My loan was paid off. Why was it not released? $$ A2. The release of your mortgage may take up to 90 days due to county recordation times and other factors. If the time frame has exceeded 90 days, please contact our customer service department for assistance.--- Q3. My loan was recently transferred to LoanCare, but the transfer was delayed. I have missed a payment. What do I do now?$$ A3. Once your loan transfers to us, we will perform a verification process to ensure all information was transferred correctly. Any payments that were made to your previous servicer will be transferred to LoanCare and posted to your account. There are no late fees or credit reporting on your account for 60 days post transfer. If you would like to mail in a payment, we can accept it and will post it once your loan has been moved to our system. Please contact us if you are having difficulty with these options.--- Q4. What do I do if there are errors on my credit report?$$A4. If you suspect an error on your credit report regarding your mortgage, please contact the appropriate credit bureau agency for specific resolution information. Credit report disputes may also be sent in writing to the mortgage resolution unit. Please include your loan number, name, and a copy of your credit report from one of the following credit repositories: TransUnion, Equifax, Experian or Innovis. Also include any pertinent documents supporting your dispute. Upon receipt, our mortgage resolution unit will research the dispute and respond within 30 days."
                },
                 new FAQ
                {
                    heading="Notifications",
                    detail="---Q1. I received a welcome letter. What does the letter mean? $$ A1. Your loan servicer is requested to notify you in writing at least 15 days before the servicing of your loan is transferred to a new servicer. The letter includes the effective date of transfer, the date your current servicer will stop accepting payments and the date the new servicer will be accepting them; the name, address, and telephone number of new servicer; optional insurance information; and that the transfer of servicing does not affect any term or condition of your mortgage documents, other than the terms directly related to the servicing of your loan. Please note that on the effective date of transfer during this 60-day transition period, that payment will not be treated as late if you mistakenly sent it to the previous mortgage servicer instead of the new one.--- Q2. I am having trouble understanding my monthly mortgage statement. Do you provide any assistance in understanding the statement?$$ A2. We have a monthly mortgage statement guide that should have been mailed with your first monthly mortgage statement. If you need any additional copies for reference, we have uploaded a PDF that is available to you once you log into your borrower website. The PDF can be found on the website under the My Statements menu.---Q3. I received a late notice, but I made my payment. Why has it not posted? $$A3. The first step is to verify with your local financial institution that the funds were available and deducted from your account. If this is the case, you will need to provide proof of the payment, such as a bank statement or a check showing the payment. Please remember to include your name and loan number when submitting proof. You may fax this to our customer service department.--- Q4. I received a goodbye letter. What does the letter mean?$$A4. A goodbye letter is a notification required by the Real Estate Settlement Procedures Act (RESPA) that advises borrowers that the servicing of their loan has transferred. The letter also provides important information, such as your new loan number, payment address and contact information for customer service."
                },
                  new FAQ
                {
                    heading="Tax or Insurance Bills",
                    detail="---Q1. I received a delinquent tax or insurance bill, but I have an escrow account. What should I do? $$ A1. Please send a copy of your delinquency notice to our Jacksonville office. You may fax it to our escrow department at 1.866.221.5274 or mail it to Attn: Escrow Department, P.O.Box 43070, Jacksonville, FL 32203.If you are unable to send a copy of the notice, please contact us. "

                },
                   new FAQ
                {
                    heading="Loss Mitigation",
                    detail="---Q1. How do I know which loss mitigation solution is available to me? $$ A1. There are several factors that determine which options may be available to you. One of the first steps is to request help from us and provide us with your financial details. A loan counselor will review this information and will call you back to discuss your options.---Q2. How do I check the status of my loss mitigation request? $$ A2. You may call us for questions regarding the status of your request. ---Q3. How do I submit a Borrower Response Package?$$A3. There are several ways to submit a package. You can fax it to 1.800.909.9525, email it to LC-LossMitigation@loancare.net or mail it to Attn: Loss Mitigation Department, 3637 Sentara Way, Virginia Beach, VA 23452.---Q4. What is the time frame to get an answer on my Loss Mitigation request when submitting a Borrower Response Package?$$A4. Within 30 days of receipt of a complete Borrower Response Package, we will let you know which foreclosure alternatives, if any, are available to you. We will then inform you of your next steps to accept our offer. However, if you submit your complete Borrower Response Package less than 37 days prior to a scheduled foreclosure sale date, we will strive to process your request as quickly as possible. However, you may not receive a notice of incompleteness or a decision on your request prior to the sale. Please submit your Borrower Response Package as soon as possible.---Q5. What documents do I need to submit for a loan modification?$$A5. A list of required documents can be viewed by visiting  www.MyLoanCare.com/HomeOwnerAssistance.htm---Q6. I disagree with the decision made on my modification request; how do I request a review?$$A6. A review of your loss mitigation decision can be requested by sending a written request to our loss mitigation appeals team. A separate group will review and respond to your concerns when completed.--- Q7. What are my options if my modification request was denied and I cannot afford my payments?$$A7. Transitioning into affordable housing with the help of a short sale can be an alternative to foreclosure. Even if you owe more than your home is worth, short selling your property may be an option. The first step is to contact an experienced realtor in order to list the property for sale. Second, submit a Borrower Response Package along with the appropriate documents to us for a short sale review. An additional option is to request a deed-in-lieu or a mortgage release. Both options allow homeowners facing foreclosure opportunities to relieve themselves from the mortgage obligation.---Q8. What happens if I let my home go into foreclosure?$$A8. Foreclosure processes are different in every state. If you are worried about making your mortgage payments, please review your state’s  foreclosure laws and processes, and contact us directly to inquire about foreclosure prevention options that are available. If your income or expenses have changed so much that you are not able to continue paying your mortgage, and you have exhausted all other options, please understand that there are additional alternatives for help, such as a pre-foreclosure sale or deed-in-lieu of foreclosure. Click here to view the Federal Housing Administration’s Save Your Home: Tips to Avoid Foreclosure brochure.---Q9. What if my property is scheduled for a foreclosure sale date in the future?$$A9. If you submit a complete Borrower Response Package less than 37 calendar days before a scheduled foreclosure sale, there is no guarantee we can evaluate you for a foreclosure alternative in time to stop the foreclosure sale. If you are approved for a foreclosure alternative, please note that a court jurisdiction over the foreclosure proceeding or a public official charged with carrying out the sale, may not stop the scheduled foreclosure sale date.---Q10. Why did LoanCare inspect my home?$$A10. If we have not received a mortgage payment in the last 30 days or if we cannot contact you about the resolution of mortgage loan delinquency, we will order a property inspection no later than 45 days after a missed mortgage payment. The inspection is to determine if the property is occupied or vacant, the condition of the property, if the property is listed for sale, if there is any waste, deterioration, or vandalism, if there is any deferred maintenance or if visible asset preservation is needed."
                },
                    new FAQ
                {
                    heading="Notice of Error/Information Requests ",
                    detail="---Q1. How do I request information relating to the servicing of my mortgage loan? $$ A1. Please submit a written notice addressed to LoanCare’s mortgage resolution unit. Include your full name, loan number, and a statement of the information you are requesting regarding your mortgage loan account. Please note that only a written notice in this format is considered a request for information. Other means of contacting us, such as a notice on a payment coupon or a request for a payoff balance, are not considered requests for information. ---Q2. What if I have a question or concern regarding specific errors?$$ A2. Please submit a written notice addressed to LoanCare’s mortgage resolution unit stating the specific error(s) that you believe to have occurred. Include your full name, loan number, and submit the written notice to the address found on the contact us page. Please note that a notice on a payment coupon or other payment form is not considered a notice of error. ---Q3. How long will it be before I receive a response?$$A3. For questions or concerns regarding something that you believe may be an error, LoanCare will acknowledge our receipt of your written notice (if received in the format stated in Question 1 in this section) within five business days after receipt. If your notice asserts a failure to provide an accurate payoff balance amount, we will respond no later than seven business days after receiving it. For all other errors, you will receive a response no later than 30 days after we have received the notice of error. We will either correct the error and provide a written notification of the correction, or we will conduct an investigation and provide a written notification that no error occurred."
                }
            };
            return new ResponseModel(faq);
        }
        public async Task<ResponseModel> GetHolidayListAsync(string MobileToken)
        {
            // To do - Use DI

            try
            {
                TokenServices tokenServices = new TokenServices();
                string lcToken = tokenServices.GetLctoken(MobileToken);

                var response = await API_Connection.GetAsync(lcToken, "/api/OnetimePayment/GetHolidayList");
                string returnedData = await response.Content.ReadAsStringAsync();
                var HolidaysLis = JsonConvert.DeserializeObject<List<Holidays>>(returnedData);

                DateTime calenderData = new DateTime();

                List<DateTime> calenderDataList = new List<DateTime>();

                foreach (var h_list in HolidaysLis)
                {

                    if (h_list.h_Date == null)
                    {

                        h_list.h_Date = System.DateTime.Now;
                    }

                    calenderData = h_list.h_Date;

                    calenderDataList.Add(calenderData);
                }

                return new ResponseModel(calenderDataList);

            }
            catch (Exception ex)
            {
                return new ResponseModel(null, 1, ex.Message);

            }




        }
    }
}
