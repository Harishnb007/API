using System;
using System.Collections.Generic;
using System.Linq;
using Business_Services.Models;
using System.Diagnostics;
using Business_Services.Models.Helpers;
using System.Threading.Tasks;
using Business_Services.B2C_WebAPI;
using Business_Services.Models.LC_WebApi_Models;
using Newtonsoft.Json;
using System.Web;
using System.Net.Http;
using System.Text;
using System.Globalization;
using Business_Services.Models.DAL.LoancareEntites;
using System.Data.Entity;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using RasterEdge.XDoc.Converter;

namespace Business_Services
{
    public class LoanService : ILoanService
    {
        private readonly ITokenServices tokenServices;

        public LoanService(ITokenServices _tokenServices)
        {
            tokenServices = _tokenServices;
        }

        public async Task<ResponseModel> DeletePaymentAsync(string mobileToken, Payment paymentData)
        {
            // To do - Use DI

            string lcToken = tokenServices.GetLctoken(mobileToken);

            try
            { 
                var response = await API_Connection.GetAsync(lcToken, "/api/OneTimePayment/CancelOnetimePayment/?loanNo="
                    + paymentData.loan_number + "&schDate=" + paymentData.payment_date.Replace('-','/') + "&isRegularDelete=true&dateCreated="
                    + paymentData.date_created);
                string returnedData = await response.Content.ReadAsStringAsync();

                return new ResponseModel();
            }
            catch (HttpRequestException)
            {
                return new ResponseModel(null, 1, "Failed to delete the payment!");
            }
        }

        public async Task<ResponseModel> DeleteAutoDraftAsync(string mobileToken, string loanNumber)
        {
            // To do - Use DI

            string lcToken = tokenServices.GetLctoken(mobileToken);

            try
            {
                var response = await API_Connection.DeleteAsync(lcToken, "/api/AutoDraft/DeleteAutoDraft/" + loanNumber);

                return new ResponseModel();
            }
            catch (HttpRequestException)
            {
                Debug.WriteLine("Error!  Received HttpRequestException when deleting the auto draft details for loan # " + loanNumber);
                return new ResponseModel(null, 1, "Failed to delete the AutoDraft!");
            }
        }

        public ResponseModel EnrollForStatements(string loanNumber)
        {
            return new ResponseModel(true);

        }

        public ResponseModel getAllLoansForUser(int userId)
        {
            return new ResponseModel();
        }

        public ResponseModel GetAutodraft(string loanNumber)
        {
            return new ResponseModel();
        }

        public async Task<ResponseModel> GetAutodraftSetupAsync(string mobileToken, string loanNumber)
        {
            // To do - Use DI
            string lcToken = tokenServices.GetLctoken(mobileToken);
            try
            {

                var eventId = 2;
                var resourceName = "Payment";
                var toEmail = "";
                var log = "Viewed+Autodraft";
                var actionName = "VIEW";

                var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();


                var response = await API_Connection.GetAsync(lcToken, "/api/AutoDraft/GetAutoDraft/" + loanNumber);
                string returnedData = await response.Content.ReadAsStringAsync();
                AutoDraft_GetAutoDraft autodraftInfo = JsonConvert.DeserializeObject<AutoDraft_GetAutoDraft>(returnedData);

                AutoDraft autoDraft = new AutoDraft()
                {
                    mortgage_amount = autodraftInfo.autoDraftInfo.paymentAmount,
                    total_amount = autodraftInfo.autoDraftInfo.totalDraftAmntPrint,
                    additional_principal = Convert.ToDecimal(autodraftInfo.autoDraftInfo.addlPrin),
                    autodraft_startdate = autodraftInfo.dueDate,
                    bank_account = new BankAccount()
                    {
                        account_nickname = Convert.ToString(autodraftInfo.autoDraftInfo.accountName),
                        account_number = autodraftInfo.autoDraftInfo.accountNumber,
                        account_type = autodraftInfo.autoDraftInfo.acctType,
                        routing_number = Convert.ToString(autodraftInfo.autoDraftInfo.transitNo),
                        legal_name = Convert.ToString(autodraftInfo.autoDraftInfo.accountName),
                        bank_name = autodraftInfo.autoDraftInfo.bankName
                    },
                    draft_payment_on = 0,
                    draft_delayDays = Convert.ToString(autodraftInfo.autoDraftInfo.delayDays),

                };

                return new ResponseModel(autoDraft);
            }
            catch (HttpRequestException)
            {
                Debug.WriteLine("Error!  Received HttpRequestException when pulling the auto draft details for loan # " + loanNumber);
                return new ResponseModel(null, 1, "Auto Draft enrollment will become available once all past due payment amounts have been paid");
            }
        }

        public async Task<ResponseModel> ConfirmationforLoanAsync(string MobileToken, ConfirmRegistration details)
        {
            // To do - Use DI

            string lcToken = tokenServices.GetLctoken(MobileToken);

            try
            {
                byte[] userSSN = System.Text.ASCIIEncoding.ASCII.GetBytes(details.ssn);
                string decodeduserSSN = System.Convert.ToBase64String(userSSN);


                Dictionary<string, string> someDict = new Dictionary<string, string>();
                someDict.Add("userID", "");
                someDict.Add("ssn", "");

                var confirmcontent = new FormUrlEncodedContent(someDict);

                var response = await API_Connection.PostUserAsync("/api/Register/GetConfirmation/", confirmcontent);
                var registercontent = await response.Content.ReadAsStringAsync();
                Eft_disclosure eftdisclosureInfo = JsonConvert.DeserializeObject<Eft_disclosure>(registercontent);


                ConfirmRegistration regdetails = new ConfirmRegistration();


                regdetails.content = eftdisclosureInfo.msg;
                string[] ElementAddloan = eftdisclosureInfo.disclosureAccept;
                regdetails.discAccept = ElementAddloan[1];
                regdetails.discVer = ElementAddloan[2];

                return new ResponseModel(regdetails);
            }
            catch (Exception ex)
            {
                return new ResponseModel(null, 1, ex.Message);
            }

        }


        public async Task<ResponseModel> GetNotificationforLoanAsync(string mobileToken, string loanNumber)
        {

            try
            {
                TokenServices tokenServices = new TokenServices();
                string lcToken = tokenServices.GetLctoken(mobileToken);

                var eventId = 1;
                var resourceName = "Others";
                var toEmail = "";
                var log = "Viewed+Notification";
                var actionName = "VIEW";

                var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();


                var response = await API_Connection.GetAsync(lcToken, "/api/EStatement/GetAllNotificationByLoanNumber/?loanNo=" + loanNumber);
                var returnNotification = await response.Content.ReadAsStringAsync();
                dynamic GetNotification = JsonConvert.DeserializeObject(returnNotification);

                ManageNotification objgetManage = new ManageNotification();

                GetNotification objgetNotification = new GetNotification();

                List<GetNotification> listNotification = new List<GetNotification>();

                foreach (var Notification in GetNotification.data)
                {

                    if (Notification.notificationTypesRowId == 171 || Notification.notificationTypesRowId == 172 || Notification.notificationTypesRowId == 173 || Notification.notificationTypesRowId == 175)
                    {

                        objgetManage.Payment_Received = true;
                    }
                    if (Notification.notificationTypesRowId == 311 || Notification.notificationTypesRowId == 312 || Notification.notificationTypesRowId == 3123 || Notification.notificationTypesRowId == 314 || Notification.notificationTypesRowId == 315 || Notification.notificationTypesRowId == 316 || Notification.notificationTypesRowId == 317 || Notification.notificationTypesRowId == 318 || Notification.notificationTypesRowId == 320)
                    {

                        objgetManage.Taxes_Disbursed = true;
                    }
                    if (Notification.notificationTypesRowId == 351)
                    {
                        objgetManage.Home_Owner_Insurence_Disbursed = true;
                    }
                    if (Notification.notificationTypesRowId == 352)
                    {
                        objgetManage.Flood_Insurence_Disbursed = true;
                    }
                    if (Notification.notificationTypesRowId == 353 || Notification.notificationTypesRowId == 354 || Notification.notificationTypesRowId == 355)
                    {
                        objgetManage.Other_Insurence_Disbursed = true;
                    }
                    var GN = new GetNotification
                    {

                        userLoanRowId = Notification.userLoanRowId,
                        isNew = Notification.isNew,
                        skipChildrenRead = Notification.skipChildrenRead,
                        notificationTypesRowId = Notification.notificationTypesRowId,
                        loanNumber = Notification.loanNumber,
                        roleId = Notification.roleId

                    };
                    listNotification.Add(GN);

                }
                objgetManage.GetNotifyList = listNotification;
                return new ResponseModel(objgetManage);
            }
            catch (Exception ex)
            {           
                return new ResponseModel(null, 1, ex.Message);
            }

        }

        public ResponseModel GetContactDetails(string loanNumber)
        {
            ContactDetails contactData = new ContactDetails
            {
                phone_primary_number = "469-605-9581",
                phone_primary_type = "Mobile",
                phone_secondary_number = "469-090-2381",
                phone_secondary_type = "Home"
            };

            switch (loanNumber)
            {
                case "12348538":
                    contactData.email = "john.smith@gmail.com";
                    contactData.phone_other_1_number = "469-605-3384";
                    contactData.phone_other_1_type = "Other";
                    contactData.phone_other_2_number = "469-605-2348";
                    contactData.phone_other_2_type = "Other";
                    break;
                case "12348537":
                    contactData.email = "john.smith@outlook.com";
                    contactData.phone_other_1_number = "469-605-3384";
                    contactData.phone_other_1_type = "Other";
                    contactData.phone_other_2_number = "469-605-2348";
                    contactData.phone_other_2_type = "Other";
                    break;
                case "12348402":
                    contactData.email = "john.smith@gmail.com";
                    break;
                case "12345327":
                    contactData.email = "john.smith@gmail.com";
                    contactData.phone_other_1_number = "469-605-3384";
                    contactData.phone_other_1_type = "Other";
                    break;

                default:
                    throw new NotImplementedException();
            }

            return new ResponseModel(contactData);
        }

        public async Task<ResponseModel> GetEscrowDetailsForLoanAsync(string mobileToken, string loanNumber)
        {
            // To do - Use DI

            string lcToken = tokenServices.GetLctoken(mobileToken);
            try
            {

               

                var response = await API_Connection.GetAsync(lcToken, "/api/Escrow/CallEscrow/?LoanNo=" + loanNumber);
                string returnedData = await response.Content.ReadAsStringAsync();
                Escrow_CallEscrow escrowInfo = JsonConvert.DeserializeObject<Escrow_CallEscrow>(returnedData);
                PropertyInsurancePolicyCollection policyInfo = JsonConvert.DeserializeObject<PropertyInsurancePolicyCollection>(returnedData);

                var responsePayment = await API_Connection.GetAsync(lcToken, "/api/OnetimePayment/GetPaymentInfo/?loanNo=" + loanNumber + "&schDate=" + "");
                returnedData = await responsePayment.Content.ReadAsStringAsync();

                OnetimePayment_GetPaymentInfo loanInfo = JsonConvert.DeserializeObject<OnetimePayment_GetPaymentInfo>(returnedData);



                List<Tax> taxItemsList = new List<Tax>();
                foreach (EscrowTaxAggregates a in escrowInfo.escrowTaxAggregates)
                {

                    Tax tempTaxData = new Tax
                    {
                        disbursement_frequency = a.getBillFrequencyTypeText(a.billFrequencyType),
                        escrow_tax_type = a.taxEscrowItemDescription,
                        est_annual_disbursement_amount = a.totalExpectedDisbAmt,
                        next_disbursement_due_amount = a.taxExpectedDisbursementAmount,
                        next_disbursement_due_date = a.taxDisbursementDueDate,
                        tax_parcel_id = a.parcelID
                    };

                    taxItemsList.Add(tempTaxData);
                }

                List<PropertyInsurance> propertyInsuranceList = new List<PropertyInsurance>();
                foreach (PropertyInsurancePolicyCollection b in escrowInfo.propertyInsurancePolicyCollection)
                {
                    PropertyInsurance insuranceData = new PropertyInsurance
                    {
                        property_insurance_company = b.insuranceCompanyName,
                        property_insurance_policy_number = b.hazardPolicyNumber,
                        property_insurance_type = b.getpolicyType(b.policyType),
                        expiration_date = b.policyExpirationDate,
                        annual_premium = b.premiumAmount
                    };

                    propertyInsuranceList.Add(insuranceData);
                }

                var eventId = 7;
                var resourceName = "Account";
                var toEmail = "";
                var log = "Viewed+Escrow";
                var actionName = "VIEW";

                var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();



                return new ResponseModel(new Escrow
                {
                    current_escrow_advance = escrowInfo.balAdvance,
                    current_escrow_balance = escrowInfo.balEscrow,
                    //escrow_shortage_amount = 0.0M,
                    last_escrow_analysis_date = Convert.ToDateTime(escrowInfo.lastAna),
                    mortgage_next_pmi_due_date = Convert.ToDateTime(escrowInfo.miDisbDueDate),
                    mortgage_annual_premium_amount = escrowInfo.pmiDisbAmount,
                    mortgage_insurance_company = escrowInfo.payee1,
                    mortgage_policy_number = escrowInfo.guarantyNo,
                    old_monthly_escrow_payment = escrowInfo.oldEscrowMth,
                    new_monthly_escrow_payment = escrowInfo.pmtEscrow,
                    taxes = taxItemsList,
                    property_insurance = propertyInsuranceList,
                    loan_number = loanNumber,
                    loan_type = loanInfo.payment.loanType,
                    user_rowId = loanInfo.payment.userRowId,
                    principal_balance = loanInfo.payment.principalBalance
                });
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }
        }

        public async Task<ResponseModel> GetgetstatementspdfAsync(string lcAuthToken, string URL)
        {
            // To do - Use DI

            string lcToken = tokenServices.GetLctoken(lcAuthToken);
            try
            {


                MemoryStream mem = new MemoryStream();
                var responsestream = await API_Connection.GetAsync(lcToken,URL);
                string returnedDatastream = await responsestream.Content.ReadAsStringAsync();
                byte[] datastream = Encoding.ASCII.GetBytes(returnedDatastream);

                //byte[] bytes;
                //BinaryFormatter bf = new BinaryFormatter();
                //MemoryStream ms = new MemoryStream();
                //bf.Serialize(ms, returnedDatastream);
                //bytes = ms.ToArray();
                //System.IO.File.WriteAllBytes("C:\\Users\\harivigneshm.FNFSECURE.003\\Desktop\\pdfhello.pdf", bytes);




                //    //using (FileStream stream = new FileStream("C:\\Users\\harivigneshm.FNFSECURE.003\\Desktop\\pdfhello.pdf" + "\\" + datastream, FileMode.CreateNew))

                //    //{

                //    //    using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))

                //    //    {

                //    //        byte[] buffer = datastream;

                //    //        stream.Write(buffer, 0, buffer.Length);

                //    //        writer.Close();

                //    //    }

                //    //}
                //    //using (FileStream stream = new FileStream("C:\\Users\\harivigneshm.FNFSECURE.003\\Desktop\\pdf" + "\\" + datastream, FileMode.CreateNew))

                //    //{

                //    //    using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))

                //    //    {

                //    //        byte[] buffer = datastream;

                //    //        stream.Write(buffer, 0, buffer.Length);

                //    //        writer.Close();

                //    //    }

                //    //}
                //}

                return new ResponseModel(returnedDatastream);
            }
            catch (Exception Ex)
            {

                return new ResponseModel(null, 1, Ex.Message);
            }
        }


        public async Task<ResponseModel> GetgetstatementsAsync(string lcAuthToken, string loan_number)
        {
            // To do - Use DI

            string lcToken = tokenServices.GetLctoken(lcAuthToken);
            try
            {
                var response = await API_Connection.GetAsync(lcToken, "/api/EStatement/GetPdfListStr/?loanNo=" + loan_number);
                string returnedData = await response.Content.ReadAsStringAsync();
                Getdetails_estatement getEstatementInfo = JsonConvert.DeserializeObject<Getdetails_estatement>(returnedData);

                var eventId = 4;
                var resourceName = "eStatement";
                var toEmail = "";
                var log = "Viewed+eStatement";
                var actionName = "VIEW";

                var trackresponse = await API_Connection.GetAsync(lcToken,"/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

                List<EsatementDateurl> estatement = new List<EsatementDateurl>();

                foreach (var Estatementdata in getEstatementInfo.data)
                {
                    estatement.Add(new EsatementDateurl

                    {
                        statement_date = Estatementdata.statementDate,
                        statement_url ="/Statements/EStatementHandler.Pdf?loanNo=" + loan_number + "&statementDate=" + Estatementdata.statementDate + "&statementKey=" + Estatementdata.key
                        
                    }

                      );

                   //  estatement.Add(statement_Date);
                }

                EstatementDetails estatementresult = new EstatementDetails()
                {
                    estatement = estatement
                };
   //             foreach (var estatemen in estatement)
   //             {
                    
   //                 MemoryStream mem = new MemoryStream();
   //                 var responsestream = await API_Connection.GetAsync(lcToken, estatemen.statement_url);
   //                 string returnedDatastream = await responsestream.Content.ReadAsStringAsync();
   //                 //byte[] datastream = Encoding.ASCII.GetBytes(returnedDatastream);
   //                 var byteArray = Encoding.UTF8.GetBytes(returnedDatastream);
   //                 //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
   //                  MemoryStream stream = new MemoryStream(byteArray);
   //                 // MemoryStream stream =  byteArray;
   //                 String outputFilePath = "C:\\Users\\harivigneshm\\Desktop\\pdfurl.pdf";
   //               //  MemoryStream outputStream = new MemoryStream();
   //                 DocumentConverter.ToDocument(returnedDatastream, outputFilePath, FileType.DOC_PDF);
                   

   //                 byte[] bytes;
   //                 BinaryFormatter bf = new BinaryFormatter();
   //                 MemoryStream ms = new MemoryStream();
   //                 bf.Serialize(ms, returnedDatastream);
   //                 bytes = ms.ToArray();
   //                 System.IO.File.WriteAllBytes("C:\\Users\\harivigneshm\\Desktop\\pdfhello.pdf", bytes);
   //                 string sampleHtml = "<html><body><p>Simple HTML string</p></body></html> ";
   //// Converter.ConvertHtmlString(sampleHtml, @"C:\\Document.pdf");
                    
   //                 //using (FileStream stream = new FileStream("C:\\Users\\harivigneshm.FNFSECURE.003\\Desktop\\pdf" + "\\" + datastream, FileMode.CreateNew))

   //                 //{

   //                 //    using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))

   //                 //    {

   //                 //        byte[] buffer = datastream;

   //                 //        stream.Write(buffer, 0, buffer.Length);

   //                 //        writer.Close();

   //                 //    }

   //                 //}
   //             }

                return new ResponseModel(estatementresult);
            }
            catch (Exception Ex)
            {

                return new ResponseModel(null, 1, Ex.Message);
            }
        }

        public async Task<ResponseModel> GetpdfstreamAsync(string lcAuthToken, string statement_url)
        {
            // To do - Use DI

            string lcToken = tokenServices.GetLctoken(lcAuthToken);
            try
            {
               
                List<EsatementDateurl> estatement = new List<EsatementDateurl>();

                    MemoryStream mem = new MemoryStream();
                    var responsestream = await API_Connection.GetAsync(lcToken, statement_url);
                    string returnedDatastream = await responsestream.Content.ReadAsStringAsync();

                    byte[] bytes;
                    BinaryFormatter bf = new BinaryFormatter();
                    MemoryStream ms = new MemoryStream();
                    bf.Serialize(ms, returnedDatastream);
                    bytes = ms.ToArray();
                return new ResponseModel(bytes);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }
        }

        //private static Stream ConvertToStream(string fileUrl)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fileUrl);
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //    try
        //    {
        //        MemoryStream mem = new MemoryStream();
        //        Stream stream = response.GetResponseStream();

        //        stream.CopyTo(mem, 4096);

        //        return mem;
        //    }
        //    finally
        //    {
        //        response.Close();
        //    }
        //}

        public async Task<ResponseModel> GetLoanInfoForLoanAsync(string lcAuthToken, string loan_number)
        {
            // To do - Use DI
            string lcToken = tokenServices.GetLctoken(lcAuthToken);
            try
            {
                var responseisenrolled = await API_Connection.GetAsync(lcToken, "/api/User/GetUserInformation");
                string returnedisenrolled = await responseisenrolled.Content.ReadAsStringAsync();
                User_GetUserInformation userInfo = JsonConvert.DeserializeObject<User_GetUserInformation>(returnedisenrolled);

                LoanInfo LoanList = new LoanInfo();
                List<LoanDetails> LoanDetail = new List<LoanDetails>();
                LoanDetails LoanDetailProperty = new LoanDetails();

                //var eventId = 7;
                //var resourceName = "Account";
                //var toEmail = "";
                //var log = "Viewed+Manage+Accounts";
                //var actionName = "VIEW";

                //var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                //string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

                foreach (string loanNumber in userInfo.user.userLoansList)
                {
                    // Send request to pull address of each loan
                    var responseAcc = await API_Connection.GetAsync(lcToken, "/api/MyAccount/GetAccountInfo/" + loanNumber);
                    string returnedDataloan = await responseAcc.Content.ReadAsStringAsync();
                    MyAccount_GetAccountInfo propertyaddress = JsonConvert.DeserializeObject<MyAccount_GetAccountInfo>(returnedDataloan);

                    var LD = new LoanDetails
                    {
                        Property_Address = propertyaddress.msg.custAddress,
                        Loan_Number = propertyaddress.msg.loanNo

                    };
                    LoanDetail.Add(LD);
                }

                return new ResponseModel(LoanDetail);
            }
            catch (Exception Ex)
            {

                return new ResponseModel(null, 1, Ex.Message);
            }

        }



        public async Task<ResponseModel> GetRountingForLoan(string lcAuthToken, string RoutingNumber)
        {
            // To do - Use DI

            string lcToken = tokenServices.GetLctoken(lcAuthToken);
            try
            {
                var responseisenrolled = await API_Connection.GetAsync(lcToken, "/api/BankAccount/CheckRoutingNumber/?routingNo=" + RoutingNumber);
                string returnedisenrolled = await responseisenrolled.Content.ReadAsStringAsync();
                dynamic userInfo = JsonConvert.DeserializeObject(returnedisenrolled);
                return new ResponseModel(userInfo);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }
        }

        public async Task<ResponseModel> GetLoanDetailsForLoanAsync(string lcAuthToken, string loan_number)
        {
            // To do - Use DI

            string lcToken = tokenServices.GetLctoken(lcAuthToken);
            try
            {
                Business_Services.Models.GenerateNewToken objgenerateToken = new GenerateNewToken();

               
                var Decryptdata = objgenerateToken.Decrypt(lcAuthToken);

                dynamic ObjUserId = JsonConvert.DeserializeObject(Decryptdata);
                bool objisenrolled = ObjUserId.eStatement;


                var response = await API_Connection.GetAsync(lcToken, "/api/MyAccount/GetAccountInfo/" + loan_number);
                string returnedData = await response.Content.ReadAsStringAsync();
                MyAccount_GetAccountInfo getuserinfo = JsonConvert.DeserializeObject<MyAccount_GetAccountInfo>(returnedData);
                LoanContactDetail LoanDetailsemail = new LoanContactDetail();
                LoanDetailsemail.email = getuserinfo.msg.emailAddress;

                //var responseisenrolled = await API_Connection.GetAsync(lcToken, "/api/User/GetUserInformation");
                //string returnedisenrolled = await responseisenrolled.Content.ReadAsStringAsync();
                //User_GetUserInformation userInfo = JsonConvert.DeserializeObject<User_GetUserInformation>(returnedisenrolled);



                LoanContactDetail LoanDetails = new LoanContactDetail
                {
                    email = LoanDetailsemail.email,
                    LoanNumber = loan_number,
                    is_enrolled = objisenrolled
                };

                var eventId = 7;
                var resourceName = "Account";
                var toEmail = "";
                var log = "Viewed+Loans";
                var actionName = "VIEW";

                var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

                return new ResponseModel(LoanDetails);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }
        }



        public async Task<ResponseModel> getcontactdetailsAsync(string lcAuthToken, string loan_number)
        {
            // To do - Use DI
            Business_Services.Models.GenerateNewToken objgenerateToken = new GenerateNewToken();
            try
            {

                string lcToken = tokenServices.GetLctoken(lcAuthToken);

                var Decryptdata = objgenerateToken.Decrypt(lcAuthToken);

                dynamic ObjUserId = JsonConvert.DeserializeObject(Decryptdata);
                string objUName = ObjUserId.UserName;

                

                var response = await API_Connection.GetAsync(lcToken, "/api/MyAccount/GetAccountInfo/" + loan_number);
                string returnedData = await response.Content.ReadAsStringAsync();
                MyAccount_GetAccountInfo getuserinfo = JsonConvert.DeserializeObject<MyAccount_GetAccountInfo>(returnedData);
                LoanContactDetail LoanDetailsemail = new LoanContactDetail();
                LoanDetailsemail.email = getuserinfo.msg.emailAddress;

                var responsephoneNo = await API_Connection.GetAsync(lcToken, "/api/Personal/GetBorrowerContactInfo/" + loan_number);
                string returnedDataPhoneNo = await responsephoneNo.Content.ReadAsStringAsync();
                dynamic obj = JsonConvert.DeserializeObject(returnedDataPhoneNo);
                personal_getborrowercontactInfo getuserinfoPhoneNo = JsonConvert.DeserializeObject<personal_getborrowercontactInfo>(returnedDataPhoneNo);

                //var responseUserName = await API_Connection.GetAsync(lcToken, "/api/User/GetUserInformation");
                //string returnedDataUserName = await responseUserName.Content.ReadAsStringAsync();
                //dynamic getuserinfoUserName = JsonConvert.DeserializeObject(returnedDataUserName);


                if (getuserinfoPhoneNo.contactinfo.contactInfo.primaryTelecomNumber.type == "H")
                {

                    LoanDetailsemail.phone_primary_type = "Home";
                }
                else if (getuserinfoPhoneNo.contactinfo.contactInfo.primaryTelecomNumber.type == "B")
                {
                    LoanDetailsemail.phone_primary_type = "Work";
                }
                else if (getuserinfoPhoneNo.contactinfo.contactInfo.primaryTelecomNumber.type == "C")
                {
                    LoanDetailsemail.phone_primary_type = "Cell";
                }

                if (getuserinfoPhoneNo.contactinfo.contactInfo.secondaryTelecomNumber.type == "H")
                {

                    LoanDetailsemail.phone_secondary_type = "Home";
                }
                else if (getuserinfoPhoneNo.contactinfo.contactInfo.secondaryTelecomNumber.type == "B")
                {
                    LoanDetailsemail.phone_secondary_type = "Work";
                }
                else if (getuserinfoPhoneNo.contactinfo.contactInfo.secondaryTelecomNumber.type == "C")
                {
                    LoanDetailsemail.phone_secondary_type = "Cell";
                }

                foreach (var OtherTeleNo in getuserinfoPhoneNo.contactinfo.contactInfo.otherTelecomNumbers)
                {

                    if (OtherTeleNo.sequenceNumber == 1)
                    {
                        LoanDetailsemail.phone_other_1_type = OtherTeleNo.type;
                        LoanDetailsemail.phone_other_1_number = OtherTeleNo.phoneNumber;
                    }
                    if (OtherTeleNo.sequenceNumber == 2)
                    {
                        LoanDetailsemail.phone_other_2_type = OtherTeleNo.type;
                        LoanDetailsemail.phone_other_2_number = OtherTeleNo.phoneNumber;
                    }
                    if (OtherTeleNo.sequenceNumber == 3)
                    {
                        LoanDetailsemail.phone_other_3_type = OtherTeleNo.type;
                        LoanDetailsemail.phone_other_3_number = OtherTeleNo.phoneNumber;
                    }
                }

                if (LoanDetailsemail.phone_other_1_type == "H")
                {

                    LoanDetailsemail.phone_other_1_type = "Home";
                }
                else if (LoanDetailsemail.phone_other_1_type == "B")
                {
                    LoanDetailsemail.phone_other_1_type = "Work";
                }
                else if (LoanDetailsemail.phone_other_1_type == "C")
                {
                    LoanDetailsemail.phone_other_1_type = "Cell";
                }

                if (LoanDetailsemail.phone_other_2_type == "H")
                {

                    LoanDetailsemail.phone_other_2_type = "Home";
                }
                else if (LoanDetailsemail.phone_other_2_type == "B")
                {
                    LoanDetailsemail.phone_other_2_type = "Work";
                }
                else if (LoanDetailsemail.phone_other_2_type == "C")
                {
                    LoanDetailsemail.phone_other_2_type = "Cell";
                }

                if (LoanDetailsemail.phone_other_3_type == "H")
                {

                    LoanDetailsemail.phone_other_3_type = "Home";
                }
                else if (LoanDetailsemail.phone_other_3_type == "B")
                {
                    LoanDetailsemail.phone_other_3_type = "Work";
                }
                else if (LoanDetailsemail.phone_other_3_type == "C")
                {
                    LoanDetailsemail.phone_other_3_type = "Cell";
                }
                if (getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressLine1 == null)
                {
                    getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressLine1 = "";
                }
                if (getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressLine2 == null)
                {
                    getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressLine2 = "";
                }


                string a = Convert.ToString(getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressLine1);

                LoanContactDetail LoanDetails = new LoanContactDetail
                {
                    Username = objUName,
                    email = LoanDetailsemail.email,
                    is_Foreign = getuserinfoPhoneNo.contactinfo.contactInfo.isInternationalAddress,
                    street = getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressStreet,
                    Address1 = Convert.ToString(getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressLine1),
                    Address2 = Convert.ToString(getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressLine2),
                    city = getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressCityName,
                    zipcode = getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressPostalCode,
                    state = getuserinfoPhoneNo.contactinfo.contactInfo.mailingAddressStateAbbreviation,
                    phone_primary_number = Convert.ToString(getuserinfoPhoneNo.contactinfo.contactInfo.primaryTelecomNumber.phoneNumber),
                    phone_primary_type = LoanDetailsemail.phone_primary_type,
                    phone_secondary_number = Convert.ToString(getuserinfoPhoneNo.contactinfo.contactInfo.secondaryTelecomNumber.phoneNumber),
                    phone_secondary_type = LoanDetailsemail.phone_secondary_type,
                    phone_other_1_number = LoanDetailsemail.phone_other_1_number,
                    phone_other_1_type = LoanDetailsemail.phone_other_1_type,
                    phone_other_2_number = LoanDetailsemail.phone_other_2_number,
                    phone_other_2_type = LoanDetailsemail.phone_other_2_type,
                    phone_other_3_number = LoanDetailsemail.phone_other_3_number,
                    phone_other_3_type = LoanDetailsemail.phone_other_3_type
                };

                var eventId = 5;
                var resourceName = "Profile+Information";
                var toEmail = "";
                var log = "Viewed+Contactdetails";
                var actionName = "VIEW";

                var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

                return new ResponseModel(LoanDetails);
            }
            catch (Exception EX)
            {

                return new ResponseModel(null, 1, EX.Message);

            }
        }
      

        public async Task<ResponseModel> GetLoanAsync(string mobileToken, string loanNumber)
        {
            // To do - Use DI

            Business_Services.Models.GenerateNewToken objgenerateToken = new GenerateNewToken();
           
                string lcToken = tokenServices.GetLctoken(mobileToken);

                var Decryptdata = objgenerateToken.Decrypt(mobileToken);

                dynamic ObjUserId = JsonConvert.DeserializeObject(Decryptdata);
                bool objisenrolled = ObjUserId.eStatement;



              Loan LoanStatus = new Loan();
            var response = await API_Connection.GetAsync(lcToken, "/api/Loan/GetCurrentLoanInfo/" + loanNumber);
            string returnedData = await response.Content.ReadAsStringAsync();
            Loan_GetCurrentLoanInfo loanInfo = JsonConvert.DeserializeObject<Loan_GetCurrentLoanInfo>(returnedData);

            //var responseUserInfo = await API_Connection.GetAsync(lcToken, "/api/User/GetUserInformation");
            //string returnedDataUserInfo = await responseUserInfo.Content.ReadAsStringAsync();
            //dynamic userInfo = JsonConvert.DeserializeObject(returnedDataUserInfo);

            var acctInforesponse = await API_Connection.GetAsync(lcToken, "/api/MyAccount/GetAccountInfo/" + loanNumber);
            string acctInfoData = await acctInforesponse.Content.ReadAsStringAsync();
            MyAccount_GetAccountInfo acctInfo = JsonConvert.DeserializeObject<MyAccount_GetAccountInfo>(acctInfoData);
            AutoDraft_GetAutoDraft autoDrftInfo = new AutoDraft_GetAutoDraft();
            bool temp_is_autodraft = false;

            try
            {
                var autoDrftresponse = await API_Connection.GetAsync(lcToken, "/api/AutoDraft/GetAutoDraft/" + loanNumber);
                string autoDrftData = await autoDrftresponse.Content.ReadAsStringAsync();

                temp_is_autodraft = true;
                autoDrftInfo = JsonConvert.DeserializeObject<AutoDraft_GetAutoDraft>(autoDrftData);
            }
            catch (Exception ex)
            {
                var Error = ex.Message;
                DateTime dateauto = new DateTime();
                dateauto = Convert.ToDateTime(loanInfo.dueDate);

                DateTime startDateauto = System.DateTime.Now;
                TimeSpan objTimeSpanauto = startDateauto - dateauto;
                double Daysauto = Convert.ToDouble(objTimeSpanauto.TotalDays);

                if (Daysauto > 60)
                {
                    LoanStatus.account_status = 1;
                }

                Loan AutoLoan = new Loan();
                AutoLoan.account_status = LoanStatus.account_status;
                AutoLoan.loan_number = loanNumber;
                AutoLoan.loan_principal_balance = Convert.ToDecimal(loanInfo.firstPB);
                AutoLoan.loan_total_amount = Convert.ToDecimal(loanInfo.netPresent);
                AutoLoan.loan_duedate = loanInfo.dueDate.Substring(5, 2) + "/" + loanInfo.dueDate.Substring(8, 2) + "/" + loanInfo.dueDate.Substring(2, 2);
                AutoLoan.loan_type = loanInfo.loanType;
                AutoLoan.is_enrolled = objisenrolled;
                AutoLoan.loan_interest_rate = loanInfo.intRate;
                AutoLoan.escrow_balance = Convert.ToDecimal(loanInfo.escrowBalance);
                AutoLoan.property_value = Convert.ToDecimal(loanInfo.propertyValue);
                AutoLoan.origination_date = Convert.ToString(loanInfo.loanDate);
                AutoLoan.original_loan_amount = Convert.ToDecimal(loanInfo.balOrigLoan);
                AutoLoan.maturity_date = loanInfo.maturityDate;
                AutoLoan.co_borrower_name = acctInfo.msg.coBorrower;
                AutoLoan.is_autodraft = false;
                AutoLoan.auto_draftdate = "";
                return new ResponseModel(AutoLoan);

                // Just to consume the error
            }

            DateTime date = new DateTime();
            date = Convert.ToDateTime(loanInfo.dueDate);

            DateTime startDate = System.DateTime.Now;
            TimeSpan objTimeSpan = startDate - date;
            double Days = Convert.ToDouble(objTimeSpan.TotalDays);

            if (Days > 60)
            {
                LoanStatus.account_status = 1;
            }

            var eventId = 7;
            var resourceName = "Account";
            var toEmail = "";
            var log = "Viewed+Loan+Details";
            var actionName = "VIEW";

            var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
            string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

            return new ResponseModel(new Loan
            {
                account_status = LoanStatus.account_status,
                loan_number = loanNumber,

                loan_principal_balance = Convert.ToDecimal(loanInfo.firstPB),
                loan_total_amount = Convert.ToDecimal(loanInfo.netPresent),
                loan_duedate = loanInfo.dueDate.Substring(5, 2) + "/" + loanInfo.dueDate.Substring(8, 2) + "/" + loanInfo.dueDate.Substring(2, 2),
                loan_type = loanInfo.loanType,
                is_enrolled = objisenrolled,
                loan_interest_rate = loanInfo.intRate,
                escrow_balance = Convert.ToDecimal(loanInfo.escrowBalance),
                property_value = Convert.ToDecimal(loanInfo.propertyValue),
                origination_date = Convert.ToString(loanInfo.loanDate),
                original_loan_amount = Convert.ToDecimal(loanInfo.balOrigLoan),
                maturity_date = loanInfo.maturityDate,
                co_borrower_name = acctInfo.msg.coBorrower,
                is_autodraft = autoDrftInfo.autoDraftInfo.isAutoDraftSetup,
                auto_draftdate = Convert.ToString((autoDrftInfo.nextDraftDate != "") ? DateTime.ParseExact(autoDrftInfo.nextDraftDate, "MM/dd/yyyy", CultureInfo.InvariantCulture) : new DateTime())
            });
        }

        public async Task<ResponseModel> PostLogoutForAsync(string MobileToken)
        {
            // To do - Use DI
            TokenServices tokenServices = new TokenServices();
            string lcToken = tokenServices.GetLctoken(MobileToken);
            try
            {
                var response = await API_Connection.GetAsync(lcToken, "/api/Auth/Logout");
                string returnedData = await response.Content.ReadAsStringAsync();
                return new ResponseModel();
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }
        }
        public async Task<ResponseModel> GetUpcomingPaymentForLoan(string loanNumber, string MobileToken)
        {
            // To do - Use DI

            string lcToken = tokenServices.GetLctoken(MobileToken);
            
            OnetimePayment_GetPaymentInfo loanInfo = new OnetimePayment_GetPaymentInfo();
            DateTime Schdate = new DateTime();
            Schdate = DateTime.Now;
            bool account_status = false;
            int savedLastBankDetails = 0;
            List<OneTimePayment_GetMockedPendingTransactions> pendingInfoPayment = new List<OneTimePayment_GetMockedPendingTransactions>();
            int paymentCounter = 0;
            string noOfPayments = string.Empty;
            decimal paymentAmt = 0.0M;
            AutoDraft_GetAutoDraft pendingInfoAutoDraft = new AutoDraft_GetAutoDraft();
            bool autodraft = false;
            try
            {
                var response = await API_Connection.GetAsync(lcToken, "/api/OnetimePayment/GetPaymentInfo/?loanNo=" + loanNumber + "&schDate=" + "");
                string returnedData = await response.Content.ReadAsStringAsync();
                account_status = true;
                loanInfo = JsonConvert.DeserializeObject<OnetimePayment_GetPaymentInfo>(returnedData);
                savedLastBankDetails = loanInfo.bankData.Count;

                var pendingresponse = await API_Connection.GetAsync(lcToken, "/api/OneTimePayment/GetMockedPendingTransactions/?loanNo=" + loanNumber + "&schDate=&");
                returnedData = await pendingresponse.Content.ReadAsStringAsync();
                pendingInfoPayment = JsonConvert.DeserializeObject<List<OneTimePayment_GetMockedPendingTransactions>>(returnedData);

                try
                {
                    var autoDraftResponse = await API_Connection.GetAsync(lcToken, "api/AutoDraft/GetAutoDraft/" + loanNumber + "? _");
                    returnedData = await autoDraftResponse.Content.ReadAsStringAsync();
                    pendingInfoAutoDraft = JsonConvert.DeserializeObject<AutoDraft_GetAutoDraft>(returnedData);

                    if (pendingInfoAutoDraft.autoDraftInfo.totalDftAmount != null)
                    {
                        autodraft = true;
                    }
                }
                catch (Exception ex)
                {
                    

                }
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                return new ResponseModel("Problem retrieving Data.Please contact Support at 1 - 800 - 274 - 6600.", 5, "Problem retrieving Data.Please contact Support at 1 - 800 - 274 - 6600.");

            }


            paymentCounter = pendingInfoPayment.Where(x => x.paymentType == 0).Sum(item => Convert.ToInt32(item.pmts));
            double dateDiff = (DateTime.Now - Convert.ToDateTime(loanInfo.payment.dueDate)).TotalDays;

            if (dateDiff > 15)
            {
                if (paymentCounter != 6)
                {
                    if (pendingInfoPayment.Count == 0)
                    {
                        noOfPayments = "2";
                    }
                    else
                    {
                        noOfPayments = loanInfo.payment.pmts; //"1";
                    }
                    paymentAmt = loanInfo.payment.pmtAmt * Convert.ToInt32(noOfPayments);
                }
                else
                {
                    noOfPayments = "2";
                    paymentAmt = loanInfo.payment.pmtAmt * Convert.ToInt32(noOfPayments);
                }

            }
            else
            {
                noOfPayments = paymentCounter != 6 ? loanInfo.payment.pmts : "0";
                paymentAmt = paymentCounter != 6 ? loanInfo.payment.pmtAmt : Convert.ToDecimal(0.00);
                paymentAmt = paymentAmt * Convert.ToInt32(noOfPayments);
            }

            Payment paymentData = new Payment
            {
                isAutoDraftAvailable = autodraft,
                due_date = loanInfo.payment.dueDate,
                loan_number = loanNumber,
                account_status = account_status,
                payment_amount = paymentAmt,
                original_mortgageAmt = loanInfo.payment.pmtAmt,
                bank_account = new BankAccount
                {

                    account_number = string.Empty,
                    account_nickname = "Choose Bank Account",
                    routing_number = string.Empty,
                    bank_name = string.Empty,
                    account_type = string.Empty,
                    legal_name = string.Empty

                },
                payment_date = loanInfo.payment.schDT,
                initial_schDate = loanInfo.payment.schDT,
                total_amount = paymentAmt
                + loanInfo.payment.addlFees
                                 + loanInfo.payment.nsfFeesDue + loanInfo.payment.otherFeesDue +
                                 loanInfo.payment.delqFee,
                number_of_payments = noOfPayments,
                additional_principal = 0,
                additional_escrow = 0,
                late_fees_due = loanInfo.payment.addlFees,
                nsf_fees_due = loanInfo.payment.nsfFeesDue,
                other_fees_due = loanInfo.payment.otherFeesDue,
                onetime_payment_fees = loanInfo.payment.delqFee,
                loanType = loanInfo.payment.loanType,
                userRowId = loanInfo.payment.userRowId,
                principal_balance = loanInfo.payment.principalBalance,
                payment_Type = "0",
                late_fees_due_input = loanInfo.payment.addlFees,
                nsf_fees_due_input = loanInfo.payment.nsfFeesDue,
                other_fees_due_input = loanInfo.payment.otherFeesDue

            };

            var eventId = 2;
            var resourceName = "Payment";
            var toEmail = "";
            var log = "Viewed+Payment";
            var actionName = "VIEW";

            var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
            string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();


            return new ResponseModel(paymentData);
        }


        public async Task<ResponseModel> GetMakePaymentPendingList(string MobileToken, string loanNumber)
        {
            // To do - Use DI

            string lcToken = tokenServices.GetLctoken(MobileToken);

            


            List<OneTimePayment_GetMockedPendingTransactions> pendingInfoPayment = new List<OneTimePayment_GetMockedPendingTransactions>();

            List<PendingPayment> pendingPayments = new List<PendingPayment>();

            try
            {
                var pendingresponse = await API_Connection.GetAsync(lcToken, "/api/OneTimePayment/GetMockedPendingTransactions/?loanNo=" + loanNumber + "&schDate=&");
                string returnedData = await pendingresponse.Content.ReadAsStringAsync();
                pendingInfoPayment = JsonConvert.DeserializeObject<List<OneTimePayment_GetMockedPendingTransactions>>(returnedData);

            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                return new ResponseModel("Problem retrieving Data.Please contact Support at 1 - 800 - 274 - 6600.", 5, "Problem retrieving Data.Please contact Support at 1 - 800 - 274 - 6600.");

            }

            foreach (OneTimePayment_GetMockedPendingTransactions a in pendingInfoPayment)
            {

                PendingPayment tempPayment = new PendingPayment()
                {
                    payment_date = a.schDT,
                    paymentCount = Convert.ToInt32(a.pmts),
                    date_created = a.dateCreated
                    
                };

                pendingPayments.Add(tempPayment);
            }
            //var eventId = 2;
            //var resourceName = "Payment";
            //var toEmail = "";
            //var log = "Viewed+Pending+Payment";
            //var actionName = "VIEW";

            //var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
            //string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

            return new ResponseModel(pendingPayments);
        }

        public async Task<ResponseModel> EditPaymentForLoanAsync(string mobileToken, string loanNumber, DateTime payment_date)
        {
            Debug.WriteLine("EditPaymentForLoanAsync has been called");
            // To do - Use DI

            string lcToken = tokenServices.GetLctoken(mobileToken);

            
            OnetimePayment_GetPaymentInfo loanInfo = new OnetimePayment_GetPaymentInfo();
            bool account_status = false;
            AutoDraft_GetAutoDraft pendingInfoAutoDraft = new AutoDraft_GetAutoDraft();
            bool autodraft = false;

            try
            {
                var response = await API_Connection.GetAsync(lcToken, "/api/OnetimePayment/GetPaymentInfo/?loanNo=" + loanNumber + "&schDate=" + payment_date.ToString("MM/dd/yyyy"));
                string returnedData = await response.Content.ReadAsStringAsync();
                //to do get account status
                account_status = true;
                loanInfo = JsonConvert.DeserializeObject<OnetimePayment_GetPaymentInfo>(returnedData);

                try
                {
                    var autoDraftResponse = await API_Connection.GetAsync(lcToken, "api/AutoDraft/GetAutoDraft/" + loanNumber + "? _");
                    returnedData = await autoDraftResponse.Content.ReadAsStringAsync();
                    pendingInfoAutoDraft = JsonConvert.DeserializeObject<AutoDraft_GetAutoDraft>(returnedData);

                    if (pendingInfoAutoDraft.autoDraftInfo.totalDftAmount != null)
                    {
                        autodraft = true;
                    }
                }
                catch (Exception ex)
                {
                } 
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }
            Payment paymentData = new Payment
            {
                isAutoDraftAvailable = autodraft,
                loan_number = loanNumber,
                account_status = account_status,
                payment_amount = loanInfo.payment.pmtAmt,
                original_mortgageAmt = loanInfo.payment.pmtAmt,
                bank_account = new BankAccount
                {
                    account_number = loanInfo.payment.accountNumber,
                    //account_nickname = loanInfo.payment.accountName,
                    routing_number = loanInfo.payment.routTran,
                    bank_name = loanInfo.payment.bankName,
                    account_type = (loanInfo.payment.accountType == "Savings") ? "Savings Account" : "Checking Account",
                    legal_name = loanInfo.payment.accountName,
                    isAuthorized = true
                },
                due_date = loanInfo.payment.dueDate,
                payment_date = loanInfo.payment.schDT,
                initial_schDate = loanInfo.payment.schDT,
                total_amount = loanInfo.payment.amtRecvd,
                number_of_payments = loanInfo.payment.pmts,
                additional_principal = loanInfo.payment.curtailment,
                additional_escrow = loanInfo.payment.addlEscrow,
                late_fees_due = loanInfo.payment.addlFees,
                nsf_fees_due = loanInfo.payment.nsfFeesDue,
                other_fees_due = loanInfo.payment.otherFeesDue,
                onetime_payment_fees = loanInfo.payment.delqFee,
                loanType = loanInfo.payment.loanType,
                userRowId = loanInfo.payment.userRowId,
                principal_balance = loanInfo.payment.principalBalance,
                payment_Type = loanInfo.payment.paymentType
            };

            if (loanInfo.payment.fees.Count != 0)
            {
                for (int i = 0; i <= loanInfo.payment.fees.Count - 1; i++)
                {
                    if (loanInfo.payment.fees[i].feeCode == "1")
                    {
                        paymentData.late_fees_due_input = loanInfo.payment.fees[i].feeAmt;
                    }
                    if (loanInfo.payment.fees[i].feeCode == "2")
                    {
                        paymentData.nsf_fees_due_input = loanInfo.payment.fees[i].feeAmt;
                    }
                    if (loanInfo.payment.fees[i].feeCode == "K")
                    {
                        paymentData.other_fees_due_input = loanInfo.payment.fees[i].feeAmt;
                    }
                }
            }

            var eventId = 2;
            var resourceName = "Edit+Payment";
            var toEmail = "";
            var log = "Viewed+Edit+Payment";
            var actionName = "VIEW";

            var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
            string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

            return new ResponseModel(paymentData);

        }


        public async Task<ResponseModel> GetPaymentFeeSchedule(string loanNumber, string MobileToken)
        {
            // To do - Use DI

            string lcToken = tokenServices.GetLctoken(MobileToken);
            try
            {

                

                var responseduedate = await API_Connection.GetAsync(lcToken, "/api/OnetimePayment/GetPaymentInfo/?loanNo=" + loanNumber + "&schDate=" + "");
                string returnedduedateData = await responseduedate.Content.ReadAsStringAsync();
                Calender_GetPaymentInfo calenderDuedate = JsonConvert.DeserializeObject<Calender_GetPaymentInfo>(returnedduedateData);

                var response = await API_Connection.GetAsync(lcToken, "api/Helper/GetPaymentFeeSchedule/?paymentFeeTypeId=" + 1 + "&loanNo=" + loanNumber + "&schDate=" + "");
                string returnedData = await response.Content.ReadAsStringAsync();
                Calender_GetPaymentFeeSchedule paymentInfo = JsonConvert.DeserializeObject<Calender_GetPaymentFeeSchedule>(returnedData);


                List<PaymentFeeSheduledate> feeList = new List<PaymentFeeSheduledate>();

                foreach (var duedate in paymentInfo.clientFeeCollection)
                {
                    feeList.Add(new PaymentFeeSheduledate
                    {
                        OverdueStartdays = duedate.daysOverdueStart,
                        OverdueEnddays = duedate.daysOverdueEnd,
                        FeeAmount = duedate.feeAmount

                    });
                }

                PaymentFeeShedule paymentData = new PaymentFeeShedule()
                {
                    dueDate = calenderDuedate.Payment.dueDate,
                    paymentFeesheduledate = feeList
                };

                var eventId = 2;
                var resourceName = "Payment";
                var toEmail = "";
                var log = "Viewed+Payment+Schedule";
                var actionName = "VIEW";

                var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

                return new ResponseModel(paymentData);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }
        }


        //Modified by BBSR Team on 10th Jan 2018
        public async Task<ResponseModel> GetPaymentHistoryForLoanAsync(string mobileToken, string loanNumber)
        {


            string lcToken = tokenServices.GetLctoken(mobileToken);
            try
            {

                

                var response = await API_Connection.GetAsync(lcToken, "/api/Loan/GetLoanActivity/" + loanNumber);
                string returnedData = await response.Content.ReadAsStringAsync();
                Activity_AccountActivity historyInfo = JsonConvert.DeserializeObject<Activity_AccountActivity>(returnedData);

                List<String> descriptionlist = new List<String>();
                foreach (FullMortageHistory a in historyInfo.fullMortageHistory)
                {
                    descriptionlist.Add(a.tranCodeDesc);

                }
                descriptionlist = descriptionlist.Distinct().ToList();

                var eventId = 2;
                var resourceName = "Payment";
                var toEmail = "";
                var log = "Viewed+Payment+History";
                var actionName = "VIEW";

                var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

                return new ResponseModel(descriptionlist);
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, Ex.Message);
            }

        }


        //Modified by BBSR Team on 10th Jan 2018 : The logic to extract the Activity details changed 
        public async Task<ResponseModel> GetPaymentDescriptionsForLoanAsync(string mobileToken, string loanNumber)
        {

            string lcToken = tokenServices.GetLctoken(mobileToken);
            try
            {

                

                var response = await API_Connection.GetAsync(lcToken, "/api/Loan/GetLoanActivity/" + loanNumber);
                string returnedData = await response.Content.ReadAsStringAsync();
                Activity_AccountActivity historyInfo = JsonConvert.DeserializeObject<Activity_AccountActivity>(returnedData);


                Business_Services.Models.LC_WebApi_Models.Activity_AccountActivity activityDetails = new Business_Services.Models.LC_WebApi_Models.Activity_AccountActivity();

                activityDetails.ActivityType = new List<Activity>();

                string strpayment_date;
                string strtotal_amount;
                string strpayment_description;
                string strdue_date;
                string strprincipal_amount;
                string strinterest_amount;
                string strescrow_amount;
                string strescrow_balance;
                string strprincipal_balance;
                string strfeeAmount;
                string strlateChargeAmount;
                string strmiscPaidAmount;
                string strsuspenseAmount;

                foreach (FullMortageHistory activity in historyInfo.fullMortageHistory)
                {

                    strpayment_date = activity.transactionAppliedDate;
                    strtotal_amount = Convert.ToString(activity.totalPaymentReceivedAmount);
                    strpayment_description = activity.tranCodeDesc;
                    strdue_date = activity.dueDate;
                    strprincipal_amount = Convert.ToString(activity.principalPmtAmount);
                    strinterest_amount = Convert.ToString(activity.interestPaidAmount);
                    strescrow_amount = Convert.ToString(activity.escrowPaidAmount);
                    strescrow_balance = Convert.ToString(activity.escrowBalance);
                    strprincipal_balance = Convert.ToString(activity.unpaidPrincipalBalance);
                    strfeeAmount = activity.feeAmount;
                    strlateChargeAmount = activity.lateChargeAmount;
                    strmiscPaidAmount = activity.miscPaidAmount;
                    strsuspenseAmount = activity.suspenseAmount;

                    activityDetails.ActivityType.Add(new Activity() { payment_date = strpayment_date, total_amount = strtotal_amount, payment_description = strpayment_description, due_date = strdue_date, principal_amount = strprincipal_amount, interest_amount = strinterest_amount, escrow_amount = strescrow_amount, escrow_balance = strescrow_balance, principal_balance = strprincipal_balance, feeAmount = strfeeAmount, lateChargeAmount = strlateChargeAmount, miscPaidAmount = strmiscPaidAmount, suspenseAmount = strsuspenseAmount });

                }

                var eventId = 2;
                var resourceName = "Payment";
                var toEmail = "";
                var log = "Viewed+Payment+Description";
                var actionName = "VIEW";

                var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

                return new ResponseModel(activityDetails);
            }
            catch (Exception ex)
            {
                return new ResponseModel(null, 1, ex.Message);
            }
        }
        public async Task<ResponseModel> GetPendingPaymentsAsync(string mobileToken, string loanNumber)
        {
            List<PendingPayment> pendingPayments = new List<PendingPayment>();
            PendingPayment tempPayment = new PendingPayment();
            AutoDraft_GetAutoDraft pendingInfoAutoDraft = new AutoDraft_GetAutoDraft();
            List<OneTimePayment_GetMockedPendingTransactions> pendingInfoPayment = new List<OneTimePayment_GetMockedPendingTransactions>();
            try
            {

                // To do - Use DI
                TokenServices tokenServices = new TokenServices();
                string lcToken = tokenServices.GetLctoken(mobileToken);
                

                string returnedData = null;
                var response = await API_Connection.GetAsync(lcToken, "/api/OneTimePayment/GetMockedPendingTransactions/?loanNo=" + loanNumber + "&schDate=&");
                returnedData = await response.Content.ReadAsStringAsync();
                pendingInfoPayment = JsonConvert.DeserializeObject<List<OneTimePayment_GetMockedPendingTransactions>>(returnedData);
                pendingInfoPayment = pendingInfoPayment.OrderBy(x => x.schDT).ToList();

                var eventId = 2;
                var resourceName = "Payment";
                var toEmail = "";
                var log = "Viewed+Pending+Payment";
                var actionName = "VIEW";

                var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

                try
                {
                    var autoDraftResponse = await API_Connection.GetAsync(lcToken, "api/AutoDraft/GetAutoDraft/" + loanNumber + "? _");
                    returnedData = await autoDraftResponse.Content.ReadAsStringAsync();
                    pendingInfoAutoDraft = JsonConvert.DeserializeObject<AutoDraft_GetAutoDraft>(returnedData);


                    if (pendingInfoAutoDraft.autoDraftInfo.totalDftAmount != null)
                    {
                        tempPayment = new PendingPayment()
                        {
                            payment_date = pendingInfoAutoDraft.nextDraftDate,
                            total_amount = pendingInfoAutoDraft.autoDraftInfo.totalDftAmount,
                            payment_description = "Auto Draft",
                            account_number = pendingInfoAutoDraft.autoDraftInfo.accountNumber
                        };

                        pendingPayments.Add(tempPayment);
                    }

                }
                catch (Exception ex)
                {
                    foreach (OneTimePayment_GetMockedPendingTransactions a in pendingInfoPayment)
                    {
                        tempPayment = new PendingPayment()
                        {
                            payment_date = a.schDT,
                            total_amount = a.amtRecvd,
                            payment_description = a.getPaymentType(a.paymentType),
                            account_number = a.accountNumber,
                            date_created = a.dateCreated,
                            paymentCount = Convert.ToInt32(a.pmts)
                        };

                        pendingPayments.Add(tempPayment);
                    }

                    

                    return new ResponseModel(pendingPayments);
                }

                foreach (OneTimePayment_GetMockedPendingTransactions a in pendingInfoPayment)
                {
                    tempPayment = new PendingPayment()
                    {
                        payment_date = a.schDT,
                        total_amount = a.amtRecvd,
                        payment_description = a.getPaymentType(a.paymentType),
                        account_number = a.accountNumber,
                        date_created = a.dateCreated,
                        paymentCount = Convert.ToInt32(a.pmts)
                    };

                    pendingPayments.Add(tempPayment);
                }


                return new ResponseModel(pendingPayments);
            }
            catch (Exception ex)
            {
                return new ResponseModel(pendingPayments, 1, ex.Message);
            }

        }

        public ResponseModel GetStatement(string loanNumber, DateTime statement_date)
        {
            return new ResponseModel(true);
        }

        public async Task<ResponseModel> PostModifyPaymentForLoanAsync(string MobileToken,
                                                Business_Services.Models.Payment ModifyPayment)
        {
            string lcToken = tokenServices.GetLctoken(MobileToken);

            try
            {
                Dictionary<string, string> someDict = new Dictionary<string, string>
                {
                    { "loanSource", "MAINFRAME" },
                    //{ "dateCreated", BankAccountDetails.bank_name },
                    //{ "routingNumber", BankAccountDetails.routing_number },
                    //{ "accountNumber", EncodedAcc },
                    //{ "legalName", BankAccountDetails.legal_name },
                    //{ "accountType", BankAccountDetails.account_type }

                };

                var content = new FormUrlEncodedContent(someDict);
                var response = await API_Connection.PostAsync(lcToken, "/api/OnetimePayment/InsertPaymentInfo/", content);

                return new ResponseModel(response);
            }
            catch (Exception ex)
            {
                return new ResponseModel(ModifyPayment, 1, ex.Message);
            }
        }

        public async Task<ResponseModel> PostManageNotificationForAsync(string MobileToken, Business_Services.Models.ManageNotification ModifyPayment)
        {
            string lcToken = tokenServices.GetLctoken(MobileToken);

            try
            {
                Dictionary<string, string> someDict = new Dictionary<string, string>
                {
                    { "loanSource", "MAINFRAME" },
                    //{ "dateCreated", BankAccountDetails.bank_name },
                    //{ "routingNumber", BankAccountDetails.routing_number },
                    //{ "accountNumber", EncodedAcc },
                    //{ "legalName", BankAccountDetails.legal_name },
                    //{ "accountType", BankAccountDetails.account_type }

                };

                var content = new FormUrlEncodedContent(someDict);
                var response = await API_Connection.PostAsync(lcToken, "/api/OnetimePayment/InsertPaymentInfo/", content);

                return new ResponseModel(response);
            }
            catch (Exception ex)
            {
                return new ResponseModel(ModifyPayment, 1, ex.Message);
            }
        }

        public async Task<ResponseModel> PostBankAccountAsync(string MobileToken, Business_Services.Models.BankAccount BankAccountDetails)
        {
            string lcToken = tokenServices.GetLctoken(MobileToken);
            try
            {
                var AccNumber = BankAccountDetails.account_number;
            byte[] mybyte = System.Text.Encoding.UTF8.GetBytes(AccNumber);
            var EncodedAcc = Convert.ToBase64String(mybyte);

            if (BankAccountDetails.account_type == "Checking Account")
            {

                BankAccountDetails.account_type = "C";
            }
            else if (BankAccountDetails.account_type == "Saving Account")
            {
                BankAccountDetails.account_type = "S";
            }
           
                Dictionary<string, string> someDict = new Dictionary<string, string>
                {
                    { "id", "" },
                    { "bankName", BankAccountDetails.bank_name },
                    { "routingNumber", BankAccountDetails.routing_number },
                    { "accountNumber", EncodedAcc },
                    { "legalName", BankAccountDetails.legal_name },
                    { "accountType", BankAccountDetails.account_type }

                };

                var content = new FormUrlEncodedContent(someDict);
                var response = await API_Connection.PostAsync(lcToken, "/api/BankAccountInformation/SaveBankDetails", content);


                var eventId = 5;
                var resourceName = "One-Time+Payment";
                var toEmail = "";
                var log = "Manage+Bank+Account+Page+-+AddBank";
                var actionName = "ADD";

                var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

                return new ResponseModel(response);
            }
            catch (Exception Ex)
            {              
                return new ResponseModel(null, 1, Ex.Message);
            }
        }

        public async Task<ResponseModel> InsertAutoDraftAsync(string MobileToken, AutodraftInsert autoDraft)
        {
            string lcToken = tokenServices.GetLctoken(MobileToken);

            try
            {
                Dictionary<string, string> someDict = new Dictionary<string, string>
                {
                    { "error[errorID]", "" },
                    { "error[errorMessage]", "" },
                    { "error[update]", "undefined" },
                    { "isNew", "true" },
                    { "accountName", "John+F+Smith" },
                     { "accountNumber", "0272707119" },
                     { "reAccountNumber", "0272707119" },
                     { "acctType", "CheckingAccount" },
                     { "actnCode", "A" },
                      { "addlPrin", "0.00" },
                      { "bankName", "BANK+OF+AMERICA,+N.A." },
                      { "dateTimeStamp", "0001-01-01T00:00:00" },
                      { "delayDays", "00" },
                      { "dftFeeAssess", "" },
                      { "effectDate", "02/01/2018" },
                      { "loanNo", "0000100271" },
                      { "totalDftAmount", "0000100271" },
                      { "transitNo", "121000358" },
                      { "paymentAmount", "2168.93" },
                      { "lblmsg", "" },
                      { "title", "Auto+Draft" },
                      { "effectDateConfirmations", "02/01/2018" },
                      { "dftFeeAssessPrint", "YES" },
                      { "fullAcctType", "Checking+Account" },
                      { "DraftDelayDaysPrint", "On+payment+due+date" },
                      { "FeePrint", "0" },
                      { "TotalDraftAmount", "2168.93" },
                      { "update", "" }
                };

                var content = new FormUrlEncodedContent(someDict);
                var response = await API_Connection.PostAsync(lcToken, "/api/AutoDraft/SaveAutoDraft/", content);

                return new ResponseModel(autoDraft);
            }
            catch (Exception Ex)
            { 
                return new ResponseModel(null, 1, Ex.Message);
            }
        }


        public async Task<ResponseModel> PostAutoDraftAsync(string MobileToken, AutodraftInsert autoDraft)
        {
            string lcToken = tokenServices.GetLctoken(MobileToken);

            try
            {
                Dictionary<string, string> someDict = new Dictionary<string, string>
                {
                    { "error[errorID]", "" },
                    { "error[errorMessage]", "" },
                    { "error[update]", "undefined" },
                    { "isNew", "true" },
                    { "accountName", "John+F+Smith" },
                    { "accountNumber", "0272707119" },
                    { "reAccountNumber", "0272707119" },
                    { "update", "true" }
                };

                var content = new FormUrlEncodedContent(someDict);
                var response = await API_Connection.PostAsync(lcToken, "/api/AutoDraft/SaveAutoDraft/", content);

                return new ResponseModel(autoDraft);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(System.DateTime.Now + ex.Message);
                return new ResponseModel(null, 1, "Error! Failed to set-up AutoDraft!");
            }
        }
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
        public async Task<ResponseModel> PostupdateemailForAsync(string MobileToken, UpdateEmail loanDetails)
        {
            // To do - Use DI
            Business_Services.Models.GenerateNewToken objgenerateToken = new GenerateNewToken();
            try
            {
                LogWriter("Email:", loanDetails.email);
                LogWriter("LoanNumber:", loanDetails.loanNo);
                var Decryptdata = objgenerateToken.Decrypt(MobileToken);

                dynamic ObjUserId = JsonConvert.DeserializeObject(Decryptdata);
                string objUId = ObjUserId.UserId;
                string objPWd = ObjUserId.Password;
                int objCId = ObjUserId.ClientId;
                string objusername = ObjUserId.UserName;

              
                if (ObjUserId.log == null) {

                    ObjUserId.log = "";
                }
                if (ObjUserId.resourcename == null) {
                    ObjUserId.resourcename = "";
                }
                string resourcename = ObjUserId.resourcename;
                string logview = ObjUserId.log;
                bool eStatemente = ObjUserId.eStatement;

                string lcToken = tokenServices.GetLctoken(MobileToken);

                byte[] email = System.Text.ASCIIEncoding.ASCII.GetBytes(loanDetails.email);
                string decodedStringemail = System.Convert.ToBase64String(email);

                byte[] emailnotify = System.Text.ASCIIEncoding.ASCII.GetBytes("Y");
                string decodedStringemailnotify = System.Convert.ToBase64String(emailnotify);

                byte[] loannotify = System.Text.ASCIIEncoding.ASCII.GetBytes(loanDetails.loanNo);
                string decodedStringloannotify = System.Convert.ToBase64String(loannotify);

                Dictionary<string, string> someDict = new Dictionary<string, string>();
                someDict.Add("email", decodedStringemail);
                someDict.Add("notifyEmail", decodedStringemailnotify);
                someDict.Add("loanNo", decodedStringloannotify);

                var content = new FormUrlEncodedContent(someDict);

                var response = await API_Connection.PostAsync(lcToken, "/api/MyAccount/SetUpdateEmail/", content);

                var responsePropertystateCD = await API_Connection.GetAsync(lcToken, "/api/Helper/GetStatePropertyCode/?loanNo="+loanDetails.loanNo);

                dynamic Message = await responsePropertystateCD.Content.ReadAsStringAsync();
                var PropcodeMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Message);
                string PropertyStatecode = PropcodeMessage;

                var responseClientURL = await API_Connection.GetAsync(lcToken, "/api/Helper/GetClientData/");
                dynamic URL = await responseClientURL.Content.ReadAsStringAsync();
                var ClientURLmessage = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(URL);
                string ClientPhone = ClientURLmessage.clientPhone;
                string ClientURL = ClientURLmessage.privateLabelURL;

                Dictionary<string, string> someDictsendmail = new Dictionary<string, string>();
                someDictsendmail.Add("emailData[0][key]", "loginName");
                someDictsendmail.Add("emailData[0][value]", objusername);
                someDictsendmail.Add("emailData[0][update]", "undefined");
                someDictsendmail.Add("emailData[1][key]", "LoanNo");
                someDictsendmail.Add("emailData[1][value]", loanDetails.loanNo);
                someDictsendmail.Add("emailData[1][update]", "undefined");
                someDictsendmail.Add("emailData[2][key]", "clientPhone");
                someDictsendmail.Add("emailData[2][value]", ClientPhone);
                someDictsendmail.Add("emailData[2][update]", "undefined");
                someDictsendmail.Add("emailData[3][key]", "Url");
                someDictsendmail.Add("emailData[3][value]", ClientURL);
                someDictsendmail.Add("emailData[3][update]", "undefined");
                someDictsendmail.Add("emailData[4][key]", "PROPERTY_STATE_CODE");
                someDictsendmail.Add("emailData[4][value]", PropertyStatecode);
                someDictsendmail.Add("emailData[4][update]", "undefined");
                someDictsendmail.Add("update", "undefined");
                var contentmail = new FormUrlEncodedContent(someDictsendmail);
                var UpdateEmailsend = "updateEmail";
                var UserID = "";
                var responseSendconfirmation = await API_Connection.PostAsync(lcToken, "/api/EmailNotification/SendEmailConfirmationForTemplate/?template=UpdateUserEmail&toEmail="+ decodedStringemail + "&pageName="+ UpdateEmailsend + "&userID="+UserID, contentmail);

                var eventId = 5;
                var resourceName = "Update+Email";
                var toEmail = "";
                var log = "Viewed+Update+Email+page";
                var actionName = "VIEW";

                var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

                var contentregeneratedToken = new FormUrlEncodedContent(new Dictionary<string, string> { { "userID", objUId }, { "password", objPWd } });
                var responseregeneratedToken = await API_Connection.PostAsync("/api/Auth/Authenticate", contentregeneratedToken);

                var Token = responseregeneratedToken.tokenValue;


                var MobileTokenNew = objgenerateToken.GenerateToken(objUId, objPWd, objCId, Token, objusername, resourcename,logview, eStatemente);
                loanDetails.Token = MobileTokenNew;

                return new ResponseModel(loanDetails);
            }
            catch (Exception Ex) {

                return new ResponseModel(null,1,Ex.Message);
            }
        }

        public async Task<ResponseModel> PostAddLoanForLoanAsync(string MobileToken, UpdateEmail loanDetails)
        {
            // To do - Use DI
            Business_Services.Models.GenerateNewToken objgenerateToken = new GenerateNewToken();
            try
            {
                var Decryptdata = objgenerateToken.Decrypt(MobileToken);

                dynamic ObjUserId = JsonConvert.DeserializeObject(Decryptdata);
                string objUId = ObjUserId.UserId;
                string objPWd = ObjUserId.Password;
                int objCId = ObjUserId.ClientId;
                string objusername = ObjUserId.UserName;
                string resourcename = ObjUserId.resourcename;
                string logview = ObjUserId.log;
                bool eStatemente = ObjUserId.eStatement;

                string lcToken = tokenServices.GetLctoken(MobileToken);

          
                Dictionary<string, string> someDict = new Dictionary<string, string>();
                someDict.Add("CurrentUserLoan[loanNo]", loanDetails.loanNo);
                someDict.Add("CurrentUserLoan[notifyEmail]", loanDetails.notifyEmail);
                someDict.Add("CurrentUserLoan[emailAddress]", loanDetails.email);
                someDict.Add("CurrentUserLoan[discVer]", loanDetails.discVer);
                someDict.Add("CurrentUserLoan[discAccept]", loanDetails.discAccept);

                var content = new FormUrlEncodedContent(someDict);

                var response = await API_Connection.PostAsync(lcToken, "/api/Investor/SaveLinkLoan/", content);
                dynamic Message = await response.message.Content.ReadAsStringAsync();

                var ErrorMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Message);
                string ErrMsg = ErrorMessage.msg;
                if (ErrMsg != "Success") {
                    loanDetails.issuccess = false;
                    loanDetails.Message= ErrMsg;
                }
                var contentregeneratedToken = new FormUrlEncodedContent(new Dictionary<string, string> { { "userID", objUId }, { "password", objPWd } });
                var responseregeneratedToken = await API_Connection.PostAsync("/api/Auth/Authenticate", contentregeneratedToken);

                var Token = responseregeneratedToken.tokenValue;


                var MobileTokenNew = objgenerateToken.GenerateToken(objUId, objPWd, objCId, Token,objusername,resourcename,logview, eStatemente);
                loanDetails.Token = MobileTokenNew;

                return new ResponseModel(loanDetails,0, ErrMsg);
            }
            catch (Exception ex)
            {
                return new ResponseModel(null, 1, ex.Message);

            }

        }

        public async Task<ResponseModel> PosteStatementForLoanAsync(string MobileToken, Business_Services.Models.User estatementdetails)
        {
            // To do - Use DI

            Business_Services.Models.GenerateNewToken objgenerateToken = new GenerateNewToken();
            try
            {
                var Decryptdata = objgenerateToken.Decrypt(MobileToken);

            dynamic ObjUserId = JsonConvert.DeserializeObject(Decryptdata);
            string objUId = ObjUserId.UserId;
            string objPWd = ObjUserId.Password;
            int objCId = ObjUserId.ClientId;

                string resourcename = ObjUserId.resourcename;
                string logview = ObjUserId.log;
                bool eStatementenroll = ObjUserId.eStatement;

                string objusername = ObjUserId.UserName;
               
                string lcToken = tokenServices.GetLctoken(MobileToken);

           
                var response = await API_Connection.GetAsync(lcToken, "/api/MyAccount/GetAccountInfo/" + estatementdetails.loanNumber);
                string returnedData = await response.Content.ReadAsStringAsync();
                MyAccount_GetAccountInfo getuserinfo = JsonConvert.DeserializeObject<MyAccount_GetAccountInfo>(returnedData);

                Dictionary<string, string> someDict = new Dictionary<string, string>();
                someDict.Add("email", getuserinfo.msg.emailAddress);
                someDict.Add("loanNo", estatementdetails.loanNumber);

                var content = new FormUrlEncodedContent(someDict);

                var responseEstatement = await API_Connection.PostAsync(lcToken, "/api/EStatement/EnrollStatement/", content);
                dynamic Message = await responseEstatement.message.Content.ReadAsStringAsync();
                var ResponseMsg = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Message);
                string resmsg = ResponseMsg;
                if (resmsg == "")
                {

                    eStatementenroll = true;
                }

                var contentregeneratedToken = new FormUrlEncodedContent(new Dictionary<string, string> { { "userID", objUId }, { "password", objPWd } });
                var responseregeneratedToken = await API_Connection.PostAsync("/api/Auth/Authenticate", contentregeneratedToken);

                var Token = responseregeneratedToken.tokenValue;


                //var MobileTokenNew = objgenerateToken.GenerateToken(objUId, objPWd, objCId, Token,resourcename,logview, eStatementenroll);
                var MobileTokenNew = objgenerateToken.GenerateToken(objUId, objPWd, objCId, Token,objusername, resourcename, logview,eStatementenroll);
                estatementdetails.Token = MobileTokenNew;
                return new ResponseModel(estatementdetails);
            }
            catch (Exception ex)
            {

                return new ResponseModel(null, 1, ex.Message);
            }



        }



        public async Task<ResponseModel> PostCancelStatementForLoanAsync(string MobileToken, Business_Services.Models.User Cstatementdetails)
        {
            // To do - Use DI

            Business_Services.Models.GenerateNewToken objgenerateToken = new GenerateNewToken();
            try
            {
                var Decryptdata = objgenerateToken.Decrypt(MobileToken);

            dynamic ObjUserId = JsonConvert.DeserializeObject(Decryptdata);
                string objUId = ObjUserId.UserId;
                string objPWd = ObjUserId.Password;
                int objCId = ObjUserId.ClientId;
                string objusername = ObjUserId.UserName;
                string resourcename = ObjUserId.resourcename;
                string logview = ObjUserId.log;
                bool eStatemente = ObjUserId.eStatement;
                string lcToken = tokenServices.GetLctoken(MobileToken);

           
                var response = await API_Connection.GetAsync(lcToken, "/api/MyAccount/GetAccountInfo/" + Cstatementdetails.loanNumber);
                string returnedData = await response.Content.ReadAsStringAsync();
                MyAccount_GetAccountInfo getuserinfo = JsonConvert.DeserializeObject<MyAccount_GetAccountInfo>(returnedData);

                Dictionary<string, string> someDict = new Dictionary<string, string>();
                someDict.Add("email", getuserinfo.msg.emailAddress);
                someDict.Add("loanNo", Cstatementdetails.loanNumber);

                var content = new FormUrlEncodedContent(someDict);

                var responseCstatement = await API_Connection.PostAsync(lcToken, "/api/EStatement/CancelStatement/", content);
                dynamic Message = await responseCstatement.message.Content.ReadAsStringAsync();
                var ResponseMsg = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Message);
                string resmsg = ResponseMsg;
                if (resmsg == "")
                {

                    eStatemente = false;
                }
                var contentregeneratedToken = new FormUrlEncodedContent(new Dictionary<string, string> { { "userID", objUId }, { "password", objPWd } });
                var responseregeneratedToken = await API_Connection.PostAsync("/api/Auth/Authenticate", contentregeneratedToken);

                var Token = responseregeneratedToken.tokenValue;


                var MobileTokenNew = objgenerateToken.GenerateToken(objUId, objPWd, objCId, Token,objusername,resourcename,logview, eStatemente);
                Cstatementdetails.Token = MobileTokenNew;
                return new ResponseModel(Cstatementdetails);
            }
            catch (Exception ex)
            {

                return new ResponseModel(null, 1, "Error cancelling e-Statement. Please contact Customer Service at 1-800-274-6600");
            }
        }

        //Modified By BBSR Team on 8th Jan 2018 : Cell Phone Disclosure Issue
        public async Task<ResponseModel> PostupdateprofileForAsync(string MobileToken, LoanContactDetail LoanContactDetail)
        {
            // To do - Use DI
            Business_Services.Models.GenerateNewToken objgenerateToken = new GenerateNewToken();
            string lcToken = tokenServices.GetLctoken(MobileToken);
            try
            {
                var responseuser = await API_Connection.GetAsync(lcToken, "api/Personal/GetBorrowerContactInfo/" + LoanContactDetail.LoanNumber);
                string returnedDatausername = await responseuser.Content.ReadAsStringAsync();
                personal_getborrowercontactInfo getusernameinfo = JsonConvert.DeserializeObject<personal_getborrowercontactInfo>(returnedDatausername);

                foreach (var imailingdetails in getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers)
                {

                    if (imailingdetails.sequenceNumber == 1)
                    {
                        LoanContactDetail.sequenceNumber1 = Convert.ToString(imailingdetails.sequenceNumber);
                        LoanContactDetail.phone_other_1_number = imailingdetails.phoneNumber;
                        LoanContactDetail.phone_other_1_type = imailingdetails.type;
                    }
                    else if (imailingdetails.sequenceNumber == 2)
                    {

                        LoanContactDetail.sequenceNumber2 = Convert.ToString(imailingdetails.sequenceNumber);
                        LoanContactDetail.phone_other_2_number = imailingdetails.phoneNumber;
                        LoanContactDetail.phone_other_2_type = imailingdetails.type;
                    }
                    else if (imailingdetails.sequenceNumber == 3)
                    {

                        LoanContactDetail.sequenceNumber3 = Convert.ToString(imailingdetails.sequenceNumber);
                        LoanContactDetail.phone_other_3_number = imailingdetails.phoneNumber;
                        LoanContactDetail.phone_other_3_type = imailingdetails.type;
                    }
                }

                Dictionary<string, string> someDict = new Dictionary<string, string>();
                someDict.Add("contactInfo[error][errorID]", "");
                someDict.Add("contactInfo[error][errorMessage]", "");
                someDict.Add("contactInfo[error][update]", "Undefind");
                someDict.Add("contactInfo[clientId]", getusernameinfo.contactinfo.contactInfo.clientId);
                someDict.Add("contactInfo[faxNumber]", "");
                someDict.Add("contactInfo[firstName]", getusernameinfo.contactinfo.contactInfo.firstName);
                someDict.Add("contactInfo[isInternationalAddress]", Convert.ToString(LoanContactDetail.is_Foreign));
                someDict.Add("contactInfo[lastOrOrganizationName]", getusernameinfo.contactinfo.contactInfo.lastOrOrganizationName);
                someDict.Add("contactInfo[loanNumber]", LoanContactDetail.LoanNumber);
                someDict.Add("contactInfo[mailingAddressCityName]", LoanContactDetail.city);
                someDict.Add("contactInfo[mailingAddressLine1]", LoanContactDetail.Address1);
                someDict.Add("contactInfo[mailingAddressLine2]", LoanContactDetail.Address2);
                someDict.Add("contactInfo[mailingAddressPostalCode]", LoanContactDetail.zipcode);
                someDict.Add("contactInfo[mailingAddressStateAbbreviation]", LoanContactDetail.state);
                someDict.Add("contactInfo[mailingAddressStreet]", LoanContactDetail.street);
                someDict.Add("contactInfo[middleName]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.middleName));
                someDict.Add("contactInfo[primaryTelecomNumber][error][errorID]", "");
                someDict.Add("contactInfo[primaryTelecomNumber][error][errorMessage]", "");
                someDict.Add("contactInfo[primaryTelecomNumber][error][update]", "Undefind");
                someDict.Add("contactInfo[primaryTelecomNumber][bestTime]", "");

                //Modified By BBSR Team on 8th Jan 2018
                if (getusernameinfo.contactinfo.contactInfo.primaryTelecomNumber.type == "C")
                {
                    someDict.Add("contactInfo[primaryTelecomNumber][consentRevokeIndicatorCode]", LoanContactDetail.consentRevokeIndicatorCode_primary);
                    someDict.Add("contactInfo[primaryTelecomNumber][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.primaryTelecomNumber.consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[primaryTelecomNumber][consentRevokeSourceCode]", "4");
                }
                else
                {
                    someDict.Add("contactInfo[primaryTelecomNumber][consentRevokeIndicatorCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.primaryTelecomNumber.consentRevokeIndicatorCode));
                    someDict.Add("contactInfo[primaryTelecomNumber][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.primaryTelecomNumber.consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[primaryTelecomNumber][consentRevokeSourceCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.primaryTelecomNumber.consentRevokeSourceCode));
                }
                someDict.Add("contactInfo[primaryTelecomNumber][phoneNumber]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.primaryTelecomNumber.phoneNumber));
                someDict.Add("contactInfo[primaryTelecomNumber][priorityCode]", getusernameinfo.contactinfo.contactInfo.primaryTelecomNumber.priorityCode);
                someDict.Add("contactInfo[primaryTelecomNumber][sequenceNumber]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.primaryTelecomNumber.sequenceNumber));
                someDict.Add("contactInfo[primaryTelecomNumber][sourceCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.primaryTelecomNumber.sourceCode));
                someDict.Add("contactInfo[primaryTelecomNumber][type]", getusernameinfo.contactinfo.contactInfo.primaryTelecomNumber.type);
                someDict.Add("contactInfo[primaryTelecomNumber][update]", "undefined");
                someDict.Add("contactInfo[secondaryTelecomNumber][error][errorID]", "");
                someDict.Add("contactInfo[secondaryTelecomNumber][error][errorMessage]", "");
                someDict.Add("contactInfo[secondaryTelecomNumber][error][update]", "undefined");
                someDict.Add("contactInfo[secondaryTelecomNumber][bestTime]", "");

                //Modified By BBSR Team on 8th Jan 2018
                if (getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.type == "C")
                {
                    someDict.Add("contactInfo[secondaryTelecomNumber][consentRevokeIndicatorCode]", LoanContactDetail.consentRevokeIndicatorCode_secondary);
                    someDict.Add("contactInfo[secondaryTelecomNumber][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[secondaryTelecomNumber][consentRevokeSourceCode]", "4");
                }
                else
                {
                    someDict.Add("contactInfo[secondaryTelecomNumber][consentRevokeIndicatorCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.consentRevokeIndicatorCode));
                    someDict.Add("contactInfo[secondaryTelecomNumber][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[secondaryTelecomNumber][consentRevokeSourceCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.consentRevokeSourceCode));
                }

                someDict.Add("contactInfo[secondaryTelecomNumber][phoneNumber]", getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.phoneNumber);
                someDict.Add("contactInfo[secondaryTelecomNumber][priorityCode]", getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.priorityCode);
                someDict.Add("contactInfo[secondaryTelecomNumber][sequenceNumber]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.sequenceNumber));
                someDict.Add("contactInfo[secondaryTelecomNumber][sourceCode]", getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.sourceCode);
                someDict.Add("contactInfo[secondaryTelecomNumber][type]", getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.type);
                someDict.Add("contactInfo[secondaryTelecomNumber][update]", "Undefind");
                someDict.Add("contactInfo[otherTelecomNumbers][0][error][errorID]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][0][error][errorMessage]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][0][error][update]", "undefined");
                someDict.Add("contactInfo[otherTelecomNumbers][0][bestTime]", "");

                //Modified By BBSR Team on 8th Jan 2018
                if (getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.type == "C")
                {
                    someDict.Add("contactInfo[otherTelecomNumbers][0][consentRevokeIndicatorCode]", LoanContactDetail.consentRevokeIndicatorCode_other1);
                    someDict.Add("contactInfo[otherTelecomNumbers][0][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[0].consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[otherTelecomNumbers][0][consentRevokeSourceCode]", "4");
                }
                else
                {
                    someDict.Add("contactInfo[otherTelecomNumbers][0][consentRevokeIndicatorCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[0].consentRevokeIndicatorCode));
                    someDict.Add("contactInfo[otherTelecomNumbers][0][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[0].consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[otherTelecomNumbers][0][consentRevokeSourceCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[0].consentRevokeSourceCode));
                }

                someDict.Add("contactInfo[otherTelecomNumbers][0][phoneNumber]", LoanContactDetail.phone_other_1_number);
                someDict.Add("contactInfo[otherTelecomNumbers][0][priorityCode]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][0][sequenceNumber]", LoanContactDetail.sequenceNumber1);
                someDict.Add("contactInfo[otherTelecomNumbers][0][sourceCode]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][0][type]", LoanContactDetail.phone_other_1_type);
                someDict.Add("contactInfo[otherTelecomNumbers][0][update]", "undefined");
                someDict.Add("contactInfo[otherTelecomNumbers][1][error][errorID]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][1][error][errorMessage]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][1][error][update]", "undefined");
                someDict.Add("contactInfo[otherTelecomNumbers][1][bestTime]", "");

                if (getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.type == "C")
                {
                    someDict.Add("contactInfo[otherTelecomNumbers][1][consentRevokeIndicatorCode]", LoanContactDetail.consentRevokeIndicatorCode_other2);
                    someDict.Add("contactInfo[otherTelecomNumbers][1][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[1].consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[otherTelecomNumbers][1][consentRevokeSourceCode]", "4");
                }
                else
                {
                    someDict.Add("contactInfo[otherTelecomNumbers][1][consentRevokeIndicatorCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[1].consentRevokeIndicatorCode));
                    someDict.Add("contactInfo[otherTelecomNumbers][1][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[1].consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[otherTelecomNumbers][1][consentRevokeSourceCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[1].consentRevokeSourceCode));
                }

                someDict.Add("contactInfo[otherTelecomNumbers][1][phoneNumber]", LoanContactDetail.phone_other_2_number);
                someDict.Add("contactInfo[otherTelecomNumbers][1][priorityCode]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][1][sequenceNumber]", LoanContactDetail.sequenceNumber2);
                someDict.Add("contactInfo[otherTelecomNumbers][1][sourceCode]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][1][type]", LoanContactDetail.phone_other_2_type);
                someDict.Add("contactInfo[otherTelecomNumbers][1][update]", "undefined");
                someDict.Add("contactInfo[otherTelecomNumbers][2][error][errorID]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][2][error][errorMessage]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][2][error][update]", "undefined");
                someDict.Add("contactInfo[otherTelecomNumbers][2][bestTime]", "undefined");

                if (getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.type == "C")
                {
                    someDict.Add("contactInfo[otherTelecomNumbers][2][consentRevokeIndicatorCode]", LoanContactDetail.consentRevokeIndicatorCode_other3);
                    someDict.Add("contactInfo[otherTelecomNumbers][2][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[2].consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[otherTelecomNumbers][2][consentRevokeSourceCode]", "4");
                }
                else
                {
                    someDict.Add("contactInfo[otherTelecomNumbers][2][consentRevokeIndicatorCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[2].consentRevokeIndicatorCode));
                    someDict.Add("contactInfo[otherTelecomNumbers][2][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[2].consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[otherTelecomNumbers][2][consentRevokeSourceCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[2].consentRevokeSourceCode));
                }

                someDict.Add("contactInfo[otherTelecomNumbers][2][phoneNumber]", LoanContactDetail.phone_other_3_number);
                someDict.Add("contactInfo[otherTelecomNumbers][2][priorityCode]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][2][sequenceNumber]", LoanContactDetail.sequenceNumber3);
                someDict.Add("contactInfo[otherTelecomNumbers][2][sourceCode]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][2][type]", LoanContactDetail.phone_other_3_type);
                someDict.Add("contactInfo[otherTelecomNumbers][2][update]", "undefined");
                someDict.Add("contactInfo[borrower]", getusernameinfo.contactinfo.contactInfo.borrower);
                someDict.Add("contactInfo[coBorrower]", getusernameinfo.contactinfo.contactInfo.coBorrower);
                someDict.Add("ccontactInfo[propertyAddressStreet]", getusernameinfo.contactinfo.contactInfo.propertyAddressStreet);
                someDict.Add("contactInfo[propertyAddressCityStateZip]", LoanContactDetail.state + "" + LoanContactDetail.zipcode);
                someDict.Add("contactInfo[update]", "Undefind");
                var content = new FormUrlEncodedContent(someDict);

                var response = await API_Connection.PostAsync(lcToken, "/api/Personal/UpdateBorrowerContactInfo/", content);

                var Decryptdata = objgenerateToken.Decrypt(MobileToken);

                dynamic ObjUserId = JsonConvert.DeserializeObject(Decryptdata);
                string objUId = ObjUserId.UserId;
                string objPWd = ObjUserId.Password;
                int objCId = ObjUserId.ClientId;
                string objusername = ObjUserId.UserName;
                string resourcename = ObjUserId.resourcename;
                string logview = ObjUserId.log;
                bool eStatemente = ObjUserId.eStatement;
                var contentregeneratedToken = new FormUrlEncodedContent(new Dictionary<string, string> { { "userID", objUId }, { "password", objPWd } });
                var responseregeneratedToken = await API_Connection.PostAsync("/api/Auth/Authenticate", contentregeneratedToken);

                var Token = responseregeneratedToken.tokenValue;


                var MobileTokenNew = objgenerateToken.GenerateToken(objUId, objPWd, objCId, Token,objusername,resourcename,logview,eStatemente);
                LoanContactDetail.Token = MobileTokenNew;
                return new ResponseModel(LoanContactDetail);

            }
            catch (Exception EX)
            {
                return new ResponseModel(null, 1, EX.Message);
            }
        }

        //Modified By BBSR Team on 6th Jan 2018 : Cell Phone Disclosure Issue
        public async Task<ResponseModel> PostupdatephoneForAsync(string MobileToken, LoanContactDetail LoanContactDetail)
        {
            // To do - Use DI
            Business_Services.Models.GenerateNewToken objgenerateToken = new GenerateNewToken();
            try
            {
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
           
                var responseuser = await API_Connection.GetAsync(lcToken, "api/Personal/GetBorrowerContactInfo/" + LoanContactDetail.LoanNumber);
                string returnedDatausername = await responseuser.Content.ReadAsStringAsync();
                personal_getborrowercontactInfo getusernameinfo = JsonConvert.DeserializeObject<personal_getborrowercontactInfo>(returnedDatausername);

                foreach (var iphonedetails in getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers)
                {

                    if (iphonedetails.sequenceNumber == 1)
                    {
                        LoanContactDetail.sequenceNumber1 = Convert.ToString(iphonedetails.sequenceNumber);
                    }
                    else if (iphonedetails.sequenceNumber == 2)
                    {

                        LoanContactDetail.sequenceNumber2 = Convert.ToString(iphonedetails.sequenceNumber);
                    }
                    else if (iphonedetails.sequenceNumber == 3)
                    {

                        LoanContactDetail.sequenceNumber3 = Convert.ToString(iphonedetails.sequenceNumber);
                    }
                }

                Dictionary<string, string> someDict = new Dictionary<string, string>();
                someDict.Add("contactInfo[error][errorID]", "");
                someDict.Add("contactInfo[error][errorMessage]", "");
                someDict.Add("contactInfo[error][update]", "Undefind");
                someDict.Add("contactInfo[clientId]", getusernameinfo.contactinfo.contactInfo.clientId);
                someDict.Add("contactInfo[faxNumber]", "");
                someDict.Add("contactInfo[firstName]", getusernameinfo.contactinfo.contactInfo.firstName);
                someDict.Add("contactInfo[isInternationalAddress]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.isInternationalAddress));
                someDict.Add("contactInfo[lastOrOrganizationName]", getusernameinfo.contactinfo.contactInfo.lastOrOrganizationName);
                someDict.Add("contactInfo[loanNumber]", LoanContactDetail.LoanNumber);
                someDict.Add("contactInfo[mailingAddressCityName]", getusernameinfo.contactinfo.contactInfo.mailingAddressCityName);
                someDict.Add("contactInfo[mailingAddressLine1]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.mailingAddressLine1));
                someDict.Add("contactInfo[mailingAddressLine2]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.mailingAddressLine2));
                someDict.Add("contactInfo[mailingAddressPostalCode]", getusernameinfo.contactinfo.contactInfo.mailingAddressPostalCode);
                someDict.Add("contactInfo[mailingAddressStateAbbreviation]", getusernameinfo.contactinfo.contactInfo.mailingAddressStateAbbreviation);
                someDict.Add("contactInfo[mailingAddressStreet]", getusernameinfo.contactinfo.contactInfo.mailingAddressStreet);
                someDict.Add("contactInfo[middleName]", "");
                someDict.Add("contactInfo[primaryTelecomNumber][error][errorID]", "");
                someDict.Add("contactInfo[primaryTelecomNumber][error][errorMessage]", "");
                someDict.Add("contactInfo[primaryTelecomNumber][error][update]", "Undefind");
                someDict.Add("contactInfo[primaryTelecomNumber][bestTime]", "");
                
                //Modified By BBSR Team on 6th Jan 2018
                if (LoanContactDetail.phone_primary_type == "C")
                {
                    someDict.Add("contactInfo[primaryTelecomNumber][consentRevokeIndicatorCode]", LoanContactDetail.consentRevokeIndicatorCode_primary);
                    someDict.Add("contactInfo[primaryTelecomNumber][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.primaryTelecomNumber.consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[primaryTelecomNumber][consentRevokeSourceCode]", "4");
                }
                else
                {
                    someDict.Add("contactInfo[primaryTelecomNumber][consentRevokeIndicatorCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.primaryTelecomNumber.consentRevokeIndicatorCode));
                    someDict.Add("contactInfo[primaryTelecomNumber][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.primaryTelecomNumber.consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[primaryTelecomNumber][consentRevokeSourceCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.primaryTelecomNumber.consentRevokeSourceCode));
                }

                someDict.Add("contactInfo[primaryTelecomNumber][phoneNumber]", LoanContactDetail.phone_primary_number);
                someDict.Add("contactInfo[primaryTelecomNumber][priorityCode]", getusernameinfo.contactinfo.contactInfo.primaryTelecomNumber.priorityCode);
                someDict.Add("contactInfo[primaryTelecomNumber][sequenceNumber]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.primaryTelecomNumber.sequenceNumber));
                someDict.Add("contactInfo[primaryTelecomNumber][sourceCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.primaryTelecomNumber.sourceCode));
                someDict.Add("contactInfo[primaryTelecomNumber][type]", LoanContactDetail.phone_primary_type);
                someDict.Add("contactInfo[primaryTelecomNumber][update]", "undefined");
                someDict.Add("contactInfo[secondaryTelecomNumber][error][errorID]", "");
                someDict.Add("contactInfo[secondaryTelecomNumber][error][errorMessage]", "");
                someDict.Add("contactInfo[secondaryTelecomNumber][error][update]", "undefined");
                someDict.Add("contactInfo[secondaryTelecomNumber][bestTime]", "");

                //Modified By BBSR Team on 6th Jan 2018
                if (LoanContactDetail.phone_secondary_type == "C")
                {
                    someDict.Add("contactInfo[secondaryTelecomNumber][consentRevokeIndicatorCode]", LoanContactDetail.consentRevokeIndicatorCode_secondary);
                    someDict.Add("contactInfo[secondaryTelecomNumber][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[secondaryTelecomNumber][consentRevokeSourceCode]", "4");
                }
                else
                {
                    someDict.Add("contactInfo[secondaryTelecomNumber][consentRevokeIndicatorCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.consentRevokeIndicatorCode));
                    someDict.Add("contactInfo[secondaryTelecomNumber][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[secondaryTelecomNumber][consentRevokeSourceCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.consentRevokeSourceCode));
                }

                someDict.Add("contactInfo[secondaryTelecomNumber][phoneNumber]", LoanContactDetail.phone_secondary_number);
                someDict.Add("contactInfo[secondaryTelecomNumber][priorityCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.priorityCode));
                someDict.Add("contactInfo[secondaryTelecomNumber][sequenceNumber]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.sequenceNumber));
                someDict.Add("contactInfo[secondaryTelecomNumber][sourceCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.secondaryTelecomNumber.sourceCode));
                someDict.Add("contactInfo[secondaryTelecomNumber][type]", LoanContactDetail.phone_secondary_type);
                someDict.Add("contactInfo[secondaryTelecomNumber][update]", "Undefind");
                someDict.Add("contactInfo[otherTelecomNumbers][0][error][errorID]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][0][error][errorMessage]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][0][error][update]", "undefined");
                someDict.Add("contactInfo[otherTelecomNumbers][0][bestTime]", "");

                //Modified By BBSR Team on 6th Jan 2018
                if (LoanContactDetail.phone_other_1_type == "C")
                {
                    someDict.Add("contactInfo[otherTelecomNumbers][0][consentRevokeIndicatorCode]", LoanContactDetail.consentRevokeIndicatorCode_other1);
                    someDict.Add("contactInfo[otherTelecomNumbers][0][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[0].consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[otherTelecomNumbers][0][consentRevokeSourceCode]", "4");
                }
                else
                {
                    someDict.Add("contactInfo[otherTelecomNumbers][0][consentRevokeIndicatorCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[0].consentRevokeIndicatorCode));
                    someDict.Add("contactInfo[otherTelecomNumbers][0][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[0].consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[otherTelecomNumbers][0][consentRevokeSourceCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[0].consentRevokeSourceCode));
                }

                someDict.Add("contactInfo[otherTelecomNumbers][0][phoneNumber]", LoanContactDetail.phone_other_1_number);
                someDict.Add("contactInfo[otherTelecomNumbers][0][priorityCode]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][0][sequenceNumber]", LoanContactDetail.sequenceNumber1);
                someDict.Add("contactInfo[otherTelecomNumbers][0][sourceCode]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][0][type]", LoanContactDetail.phone_other_1_type);
                someDict.Add("contactInfo[otherTelecomNumbers][0][update]", "undefined");
                someDict.Add("contactInfo[otherTelecomNumbers][1][error][errorID]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][1][error][errorMessage]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][1][error][update]", "undefined");
                someDict.Add("contactInfo[otherTelecomNumbers][1][bestTime]", "");

                if (LoanContactDetail.phone_other_2_type == "C")
                {
                    someDict.Add("contactInfo[otherTelecomNumbers][1][consentRevokeIndicatorCode]", LoanContactDetail.consentRevokeIndicatorCode_other2);
                    someDict.Add("contactInfo[otherTelecomNumbers][1][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[1].consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[otherTelecomNumbers][1][consentRevokeSourceCode]", "4");
                }
                else
                {
                    someDict.Add("contactInfo[otherTelecomNumbers][1][consentRevokeIndicatorCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[1].consentRevokeIndicatorCode));
                    someDict.Add("contactInfo[otherTelecomNumbers][1][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[1].consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[otherTelecomNumbers][1][consentRevokeSourceCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[1].consentRevokeSourceCode));
                }

                someDict.Add("contactInfo[otherTelecomNumbers][1][phoneNumber]", LoanContactDetail.phone_other_2_number);
                someDict.Add("contactInfo[otherTelecomNumbers][1][priorityCode]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][1][sequenceNumber]", LoanContactDetail.sequenceNumber2);
                someDict.Add("contactInfo[otherTelecomNumbers][1][sourceCode]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][1][type]", LoanContactDetail.phone_other_2_type);
                someDict.Add("contactInfo[otherTelecomNumbers][1][update]", "undefined");
                someDict.Add("contactInfo[otherTelecomNumbers][2][error][errorID]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][2][error][errorMessage]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][2][error][update]", "undefined");
                someDict.Add("contactInfo[otherTelecomNumbers][2][bestTime]", "undefined");

                 if (LoanContactDetail.phone_other_3_type == "C")
                {
                    someDict.Add("contactInfo[otherTelecomNumbers][2][consentRevokeIndicatorCode]", LoanContactDetail.consentRevokeIndicatorCode_other3);
                    someDict.Add("contactInfo[otherTelecomNumbers][2][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[2].consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[otherTelecomNumbers][2][consentRevokeSourceCode]", "4");
                }
                else
                {
                    someDict.Add("contactInfo[otherTelecomNumbers][2][consentRevokeIndicatorCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[2].consentRevokeIndicatorCode));
                    someDict.Add("contactInfo[otherTelecomNumbers][2][consentRevokeIndicatorDate]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[2].consentRevokeIndicatorDate));
                    someDict.Add("contactInfo[otherTelecomNumbers][2][consentRevokeSourceCode]", Convert.ToString(getusernameinfo.contactinfo.contactInfo.otherTelecomNumbers[2].consentRevokeSourceCode));
                }

                someDict.Add("contactInfo[otherTelecomNumbers][2][phoneNumber]", LoanContactDetail.phone_other_3_number);
                someDict.Add("contactInfo[otherTelecomNumbers][2][priorityCode]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][2][sequenceNumber]", LoanContactDetail.sequenceNumber3);
                someDict.Add("contactInfo[otherTelecomNumbers][2][sourceCode]", "");
                someDict.Add("contactInfo[otherTelecomNumbers][2][type]", LoanContactDetail.phone_other_3_type);
                someDict.Add("contactInfo[otherTelecomNumbers][2][update]", "undefined");
                someDict.Add("contactInfo[borrower]", getusernameinfo.contactinfo.contactInfo.borrower);
                someDict.Add("ccontactInfo[propertyAddressStreet]", getusernameinfo.contactinfo.contactInfo.propertyAddressStreet);
                someDict.Add("contactInfo[coBorrower]", getusernameinfo.contactinfo.contactInfo.coBorrower);
                someDict.Add("contactInfo[propertyAddressCityStateZip]", getusernameinfo.contactinfo.contactInfo.propertyAddressCityStateZip);
                someDict.Add("contactInfo[update]", "undefined");
                var content = new FormUrlEncodedContent(someDict);

                var response = await API_Connection.PostAsync(lcToken, "/api/Personal/UpdateBorrowerContactInfo/", content);

                var contentregeneratedToken = new FormUrlEncodedContent(new Dictionary<string, string> { { "userID", objUId }, { "password", objPWd } });
                var responseregeneratedToken = await API_Connection.PostAsync("/api/Auth/Authenticate", contentregeneratedToken);

                var Token = responseregeneratedToken.tokenValue;


                var MobileTokenNew = objgenerateToken.GenerateToken(objUId, objPWd, objCId, Token,objusername,resourcename,logview,eStatementenr);
                LoanContactDetail.Token = MobileTokenNew;

                return new ResponseModel(LoanContactDetail);
            }
            catch (Exception ex)
            {

                return new ResponseModel(null, 1, ex.Message);
            }
        }


        public async Task<ResponseModel> PostPaymentForLoanAsync(string MobileToken, Payment paymentdata)
        {

            string lcToken = tokenServices.GetLctoken(MobileToken);
            Dictionary<string, string> someDict = new Dictionary<string, string>();
            OnetimePayment_GetPaymentInfo loanInfo = new OnetimePayment_GetPaymentInfo();
            DateTime Schdate = new DateTime();
            Schdate = DateTime.Now;
            int savedLastBankDetails = 0;

            try
            {
                if (paymentdata.override_payment == true)
                {
                    var responseCancelPayment = await API_Connection.GetAsync(lcToken, "/api/OneTimePayment/CancelOnetimePayment/?loanNo=" +
                                 paymentdata.loan_number + "&schDate=" + paymentdata.initial_schDate +
                                 "&isRegularDelete=false&dateCreated=" + paymentdata.date_created + "&=");
                    string returnedData = await responseCancelPayment.Content.ReadAsStringAsync();

                }

                someDict.Add("loanSource", "MAINFRAME");
                someDict.Add("dateCreated", "0001-01-01T00:00:00");
                someDict.Add("isNew", "true");
                someDict.Add("IsAutoDraftSetup", paymentdata.isAutoDraftSetup.ToString());
                someDict.Add("isStateException", "false");
                someDict.Add("loanNo", paymentdata.loan_number);
                someDict.Add("schDT", paymentdata.payment_date);
                someDict.Add("intialSchDt", paymentdata.initial_schDate);
                someDict.Add("PaymentEffectiveDate", "0001-01-01T00: 00:00");
                //paymentdata.payment_date.ToString("yyyy -MM-ddTHH:mm:ss"));
                someDict.Add("pmtAmt", paymentdata.payment_amount.ToString());
                someDict.Add("addlFees", paymentdata.late_fees_due.ToString());
                someDict.Add("nSFFeesDue", paymentdata.nsf_fees_due.ToString());
                someDict.Add("IsOnetimePaymentAlreadySetUp", "false");
                someDict.Add("otherFeesDue", paymentdata.other_fees_due.ToString());
                someDict.Add("delqFee", paymentdata.onetime_payment_fees.ToString());
                someDict.Add("principalBalance", paymentdata.principal_balance.ToString());
                someDict.Add("outstandingLateChages", "0.00");
                someDict.Add("outstandingFeeDue", "0.00");

                someDict.Add("userRowId", paymentdata.userRowId);
                someDict.Add("saveBankInfoDtls", "false"); // Added By Avinash
                someDict.Add("isBankNameSelected", "true");  // Added By Avinash
                someDict.Add("bankInfoDetailsRowId", "0");
                someDict.Add("PaymentEffectiveDateFormatted", "01/01/1901");
                someDict.Add("PaymentEffectiveDateFormatted1", "01/01/1");


                someDict.Add("dueDate", paymentdata.due_date);
                someDict.Add("addlEscrow", paymentdata.additional_escrow.ToString());
                someDict.Add("curtailment", paymentdata.additional_principal.ToString());
                someDict.Add("amtRecvd", paymentdata.total_amount.ToString());

                someDict.Add("accountNumber", paymentdata.bank_account.account_number);
                someDict.Add("routTran", paymentdata.bank_account.routing_number);
                someDict.Add("accountName", paymentdata.bank_account.legal_name);
                someDict.Add("accountType", (paymentdata.bank_account.account_type == "Checking Account") ? "C" : "S");
                someDict.Add("bankName", paymentdata.bank_account.account_nickname);
                someDict.Add("pmts", paymentdata.number_of_payments);
                someDict.Add("loanType", paymentdata.loanType);
                someDict.Add("noOfDelinquentPaymentsDue", "0");
                someDict.Add("fullAcctType", paymentdata.bank_account.account_type);
                //someDict.Add("fees[0][feeCode]", "1");
                //someDict.Add("fees[0][feeAmt]", "121");
                someDict.Add("paymentsList[0][Text]", "1");
                someDict.Add("paymentsList[0][Value]", "1");
                someDict.Add("paymentsList[1][Text]", "2");
                someDict.Add("paymentsList[1][Value]", "2");
                someDict.Add("paymentsList[2][Text]", "3");
                someDict.Add("paymentsList[2][Value]", "3");
                someDict.Add("paymentsList[3][Text]", "4");
                someDict.Add("paymentsList[3][Value]", "4");
                someDict.Add("paymentsList[4][Text]", "5");
                someDict.Add("paymentsList[4][Value]", "5");
                someDict.Add("paymentsList[5][Text]", "6");
                someDict.Add("paymentsList[5][Value]", "6");
                someDict.Add("getPaymentsList", "[object Object]");
                someDict.Add("update", "");
                someDict.Add("isBankInfoVisible", "true");


                if (paymentdata.payment_Type == "FullPayment")
                {
                    // Post a regular payment
                    someDict.Add("paymentType", "0");
                    someDict.Add("PaymentTypeFull", "Payment");
                    someDict.Add("isPayment", "true");
                }
                else
                {
                    if (paymentdata.payment_Type == "Escrow Only")
                    {
                        someDict.Add("paymentType", "2");
                        someDict.Add("PaymentTypeFull", "Escrow only");
                        someDict.Add("isPayment", "true");
                    }

                    if (paymentdata.payment_Type == "Principal Only")
                    {
                        someDict.Add("paymentType", "1");
                        someDict.Add("PaymentTypeFull", "Principal only");
                        someDict.Add("isPayment", "true");
                    }

                    if (paymentdata.payment_Type == "Fees Only")
                    {
                        someDict.Add("paymentType", "3");
                        someDict.Add("PaymentTypeFull", "Fee only");
                        someDict.Add("isPayment", "false");
                    }

                    #region "Commented"
                    //if (paymentdata.additional_escrow > 0)
                    //{
                    //    someDict.Add("paymentType", "2");
                    //    someDict.Add("PaymentTypeFull", "Escrow only");
                    //    someDict.Add("isPayment", "false");
                    //    //someDict["addlEscrow"] = paymentdata.additional_escrow.ToString();
                    //    //someDict["curtailment"] = "0";
                    //    //someDict["addlFees"] = "0";
                    //    //someDict["nSFFeesDue"] = "0";
                    //    //someDict["otherFeesDue"] = "0";
                    //    //someDict["delqFee"] = "0";
                    //    //someDict["amtRecvd"] = paymentdata.additional_escrow.ToString();

                    //    var content = new FormUrlEncodedContent(someDict);
                    //    var response = await API_Connection.PostAsync(lcToken, "/api/OnetimePayment/InsertPaymentInfo/", content);
                    //}

                    //if (paymentdata.additional_principal > 0)
                    //{
                    //    // Post a principal-only payment
                    //    someDict.Add("paymentType", "1");
                    //    someDict.Add("PaymentTypeFull", "Principal only");
                    //    someDict.Add("isPayment", "false");
                    //    //someDict["isPayment"] = "false";
                    //    //someDict["addlEscrow"] = "0";
                    //    //someDict["curtailment"] = paymentdata.additional_principal.ToString();
                    //    //someDict["addlFees"] = "0";
                    //    //someDict["nSFFeesDue"] = "0";
                    //    //someDict["otherFeesDue"] = "0";
                    //    //someDict["delqFee"] = "0";
                    //    //someDict["amtRecvd"] = paymentdata.additional_principal.ToString();

                    //    var content = new FormUrlEncodedContent(someDict);
                    //    var response = await API_Connection.PostAsync(lcToken, "/api/OnetimePayment/InsertPaymentInfo/", content);
                    //}

                    //if (paymentdata.late_fees_due > 0 || paymentdata.nsf_fees_due > 0 || paymentdata.other_fees_due > 0)
                    //{
                    //    // Post a fee-only payment
                    //    someDict.Add("paymentType", "3");
                    //    someDict.Add("PaymentTypeFull", "Fee only");
                    //    someDict.Add("isPayment", "false");
                    //    //someDict["paymentType"] = "3";
                    //    //someDict["PaymentTypeFull"] = "Fee only";
                    //    //someDict["isPayment"] = "false";
                    //    //someDict["addlEscrow"] = "0";
                    //    //someDict["curtailment"] = "0";
                    //    //someDict["addlFees"] = paymentdata.late_fees_due.ToString();
                    //    //someDict["nSFFeesDue"] = paymentdata.nsf_fees_due.ToString();
                    //    //someDict["otherFeesDue"] = paymentdata.other_fees_due.ToString();
                    //    //someDict["delqFee"] = "0";
                    //    //someDict["amtRecvd"] = (paymentdata.late_fees_due + paymentdata.nsf_fees_due + paymentdata.other_fees_due).ToString();

                    //    var content = new FormUrlEncodedContent(someDict);
                    //    var response = await API_Connection.PostAsync(lcToken, "/api/OnetimePayment/InsertPaymentInfo/", content);
                    //}

                    #endregion
                }

                var content = new FormUrlEncodedContent(someDict);
                var response = await API_Connection.PostAsync(lcToken, "/api/OnetimePayment/InsertPaymentInfo/", content);

                return new ResponseModel(paymentdata);
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                return new ResponseModel("Problem saving Data.Please contact Support at 1 - 800 - 274 - 6600.", 5,
                        "Problem saving Data.Please contact Support at 1 - 800 - 274 - 6600.");

            }
        }

        public async Task<ResponseModel> UpdateAutoDraftAsync(string MobileToken, string loanNumber, AutoDraft autoDraft)
        {
            Debug.WriteLine("UpdateAutoDraftAsync() is invoked..");
            string lcToken = tokenServices.GetLctoken(MobileToken);

            try
            {
                Dictionary<string, string> someDict = new Dictionary<string, string>
                {
                    { "error[errorID]", "" },
                    { "error[errorMessage]", "" },
                    { "error[update]", "undefined" },
                    { "isNew", "true" },
                    { "accountName", autoDraft.bank_account.legal_name },
                    { "accountNumber", autoDraft.bank_account.account_number },
                    { "reAccountNumber", autoDraft.bank_account.account_number },
                    { "acctType", (autoDraft.bank_account.account_type == "Checking Account") ? "CheckingAccount" : "SavingsAccount" }, // To do - Check whether the string literal for Savings Account is correct
                    { "actnCode", "A" },
                    { "addlPrin", autoDraft.additional_principal.ToString() },
                    { "bankName", autoDraft.bank_account.bank_name },
                    { "dateTimeStamp", "0001-01-01T00:00:00" },
                    { "delayDays", autoDraft.draft_payment_on.ToString() },
                    { "dftFeeAssess", "" },
                    { "effectDate", autoDraft.autodraft_startdate.ToString("MM/dd/yyyy") },
                    { "loanNo", loanNumber },
                    { "totalDftAmount", "" },
                    { "transitNo", autoDraft.bank_account.routing_number },
                    { "paymentAmount", autoDraft.mortgage_amount.ToString() },
                    { "lblmsg", "" },
                    { "title", "Auto Draft" },
                    { "effectDateConfirmations", autoDraft.autodraft_startdate.ToString("MM/dd/yyyy") },
                    { "dftFeeAssessPrint", "YES" },
                    { "fullAcctType", autoDraft.bank_account.account_type },
                    { "DraftDelayDaysPrint", autoDraft.draft_payment_on.ToString() },
                    { "FeePrint", "0" },
                    { "TotalDraftAmount", autoDraft.total_amount.ToString() },
                    { "update", "" }
                };

                var content = new FormUrlEncodedContent(someDict);
                var response = await API_Connection.PostAsync(lcToken, "/api/AutoDraft/UpdateAutoDraft/" + loanNumber, content);

                return new ResponseModel(autoDraft);
            }
            catch (HttpRequestException Ex)
            {               
                return new ResponseModel(null, 1, Ex.Message);
            }
        }

       
       

        public async Task<ResponseModel> GetPayDetailsAsync(string mobileToken, string loanNumber)
        {
            // To do - Use DI

            string lcToken = tokenServices.GetLctoken(mobileToken);
            try
            {

                var response = await API_Connection.GetAsync(lcToken, "/api/Loan/GetCurrentLoanInfo/" + loanNumber);
                string returnedData = await response.Content.ReadAsStringAsync();
                Loan_GetCurrentLoanInfo loanInfo = JsonConvert.DeserializeObject<Loan_GetCurrentLoanInfo>(returnedData);

                var responseloanactivity = await API_Connection.GetAsync(lcToken, "/api/Loan/GetLoanActivity/" + loanNumber);
                string returnedloanactivityData = await responseloanactivity.Content.ReadAsStringAsync();
                LoanHistory_Activity loanactivityInfo = JsonConvert.DeserializeObject<LoanHistory_Activity>(returnedloanactivityData);

                var responseEscrow = await API_Connection.GetAsync(lcToken, "/api/Escrow/CallEscrow/?LoanNo=" + loanNumber);
                string returnedDataEscrow = await responseEscrow.Content.ReadAsStringAsync();
                Escrow_CallEscrow escrowInfo = JsonConvert.DeserializeObject<Escrow_CallEscrow>(returnedDataEscrow);

                var eventId = 2;
                var resourceName = "Payment";
                var toEmail = "";
                var log = "Viewed+Payment+Details";
                var actionName = "VIEW";

                var trackresponse = await API_Connection.GetAsync(lcToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourceName + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
                string trackreturnedData = await trackresponse.Content.ReadAsStringAsync();

                return new ResponseModel(NewMethod(loanInfo, loanactivityInfo ,escrowInfo));
            }
            catch (Exception Ex) {

                return new ResponseModel(null,1,Ex.Message);
            }
        }

        private static PaymentDetails NewMethod(Loan_GetCurrentLoanInfo loanInfo, LoanHistory_Activity loanactivityInfo, Escrow_CallEscrow escrowInfo)
        {


            List<Pendingloandetails> pendingpaymentList = new List<Pendingloandetails>();
            for (int i = 0; i < loanactivityInfo.fullMortageHistory.Count(); i++)
            {
                var payment = loanactivityInfo.fullMortageHistory[i];
                dynamic paymentdetail = Newtonsoft.Json.JsonConvert.SerializeObject(payment);
                Pendingloandetails objpaymentdetail = JsonConvert.DeserializeObject<Pendingloandetails>(paymentdetail);

                Pendingloandetails paymentdetails = new Pendingloandetails {


                    totalPaymentReceivedAmount = objpaymentdetail.totalPaymentReceivedAmount,
                    escrowPaidAmount = objpaymentdetail.escrowPaidAmount,
                    principalPmtAmount = objpaymentdetail.principalPmtAmount,
                    transactionAppliedDate = objpaymentdetail.transactionAppliedDate,
                    interestPaidAmount = objpaymentdetail.interestPaidAmount,
                    suspenseAmount = objpaymentdetail.suspenseAmount,
                    tranCodeDesc = objpaymentdetail.tranCodeDesc,
                    dueDate = objpaymentdetail.dueDate
                };

                pendingpaymentList.Add(paymentdetails);

            }

            List<Pendingloandetails> paymentList = new List<Pendingloandetails>();

            foreach (var paymentdesc in pendingpaymentList)
            {

                if (paymentdesc.tranCodeDesc == "Payment")
                {
                    Pendingloandetails paymentdetails = new Pendingloandetails
                    {
                        totalPaymentReceivedAmount = paymentdesc.totalPaymentReceivedAmount,
                        escrowPaidAmount = paymentdesc.escrowPaidAmount,
                        principalPmtAmount = paymentdesc.principalPmtAmount,
                        transactionAppliedDate = paymentdesc.transactionAppliedDate,
                        interestPaidAmount = paymentdesc.interestPaidAmount,
                        suspenseAmount = paymentdesc.suspenseAmount,
                        tranCodeDesc = paymentdesc.tranCodeDesc,
                        dueDate = paymentdesc.dueDate
                    };
                    paymentList.Add(paymentdetails);
                }
            }
            var transactiondate = paymentList.Max(x => x.dueDate);

            var Paymentdata = (from paymentdetails in paymentList
                               where paymentdetails.dueDate == transactiondate
                               select paymentdetails).FirstOrDefault();


            //var Paymentdata = (from paymentde in Paymentdatapending
            //                   where paymentdetails.dueDate == transactiondate
            //                   select paymentdetails).First();
            //var q = from n in pendingpaymentList
            //        //group n by n.transactionAppliedDate into g
            //        select new { totalPaymentReceivedAmount = , Date = g.Max(t => t.transactionAppliedDate) };

            PaymentDetails loan_duedatedate = new PaymentDetails();         

            loan_duedatedate.loan_duedate = Convert.ToDateTime(loanInfo.dueDate).ToString("MM/dd/yy");

            LastRegularPayment last_regular_payment = new LastRegularPayment();

            last_regular_payment.principal_amount = Convert.ToString(Paymentdata.principalPmtAmount);

            if (Convert.ToString(Paymentdata.principalPmtAmount) == "")
            {

                last_regular_payment.principal_amount = "0.00";
            }

            last_regular_payment.interest_amount = Convert.ToString(Paymentdata.interestPaidAmount);
            if (Convert.ToString(Paymentdata.interestPaidAmount) == "")
            {
                last_regular_payment.interest_amount = "0.00";
            }
            if (Convert.ToString(Paymentdata.transactionAppliedDate) == "")
            {

                last_regular_payment.last_payment_date = "";
            }
            else if (Convert.ToString(Paymentdata.transactionAppliedDate) != "")
            {
                last_regular_payment.last_payment_date = Convert.ToDateTime(Paymentdata.transactionAppliedDate).ToString("MM/dd/yy");
            }

            last_regular_payment.escrow_amount = Convert.ToString(Paymentdata.escrowPaidAmount);
            if (Convert.ToString(Paymentdata.escrowPaidAmount) == "")
            {

                last_regular_payment.escrow_amount = "0.00";
            }

            //if (loanInfo.lastEscrowPD == "")
            //{

            //    loanInfo.lastEscrowPD = "0.00";
            //}
            //if (loanInfo.lastPrinPD == "")
            //{
            //    loanInfo.lastPrinPD = "0.00";
            //}
            //if (loanInfo.lastIntPD == "")
            //{
            //    loanInfo.lastIntPD = "0.00";
            //}

            //var lastescrowlenth = Convert.ToDecimal(loanInfo.lastEscrowPD);
            //var lastPrinPDlenth = Convert.ToDecimal(loanInfo.lastPrinPD);
            //var lastintPDlenth = Convert.ToDecimal(loanInfo.lastIntPD);

            var Total_Amount = loanInfo.netPresent;
            last_regular_payment.total_amount = Convert.ToString(Total_Amount);
            loan_duedatedate.last_regular_payment = last_regular_payment;

            NextMonthlyPayment next_monthly_payment = new NextMonthlyPayment();

            next_monthly_payment.principal_interest_amount = loanInfo.firstPIPresent;
            next_monthly_payment.property_insurance_amount = loanInfo.hazPresent;
            //next_monthly_payment.Next_Payment_Amount = escrowInfo.pmtEscrow;
            next_monthly_payment.city_tax_amount = loanInfo.cityTaxAmount;
            next_monthly_payment.county_tax_amount = loanInfo.countyTaxAmount;
            next_monthly_payment.other_tax_amount = loanInfo.otherTax;
            next_monthly_payment.total_amount = loanInfo.netPresent;
            next_monthly_payment.monthly_mortgage_insurance_amount = loanInfo.miAmount;
            next_monthly_payment.overageshortage = loanInfo.overShortAmount;
            loan_duedatedate.next_monthly_payment = next_monthly_payment;
            return (loan_duedatedate);
        }


        public async Task<ResponseModel> UpdatePaymentAsync(string mobileToken, string loanNumber, DateTime payment_date, Payment payment)
        {
            // To do - Use DI

            string lcToken = tokenServices.GetLctoken(mobileToken);

            try
            {
                var deleteResponse = await API_Connection.GetAsync(lcToken, "/api/OneTimePayment/CancelOnetimePayment/?loanNo=" + loanNumber + "&schDate=" + payment_date.ToString("MM/dd/yyyy") + "&isRegularDelete=true&dateCreated=" + DateTime.Now);

                var insertResponse = await PostPaymentForLoanAsync(mobileToken, payment);

                return new ResponseModel(payment);
            }
            catch (Exception Ex)
            {              
                return new ResponseModel(null, 1, Ex.Message);
            }
        }

        public async Task<ResponseModel> ValidatePasswordAsync(string mobileToken, UserAuth userData)
        {
            try
            {
                TokenServices tokenServices = new TokenServices();
                string lcToken = tokenServices.GetLctoken(mobileToken);
                string returnedData = null;
                var response = await API_Connection.GetAsync(lcToken, "/api/BankAccountInformation/ValidatePassword?userID="
                                                                            + userData.user_id + "&password=" + userData.user_pwd);
                returnedData = await response.Content.ReadAsStringAsync();                

                if (returnedData.Contains("true"))
                {
                    return new ResponseModel(null, 0, "success");
                }
                else
                {
                    return new ResponseModel(returnedData, 0, "failure");
                }
            }
            catch (Exception Ex)
            {
                return new ResponseModel(null, 1, "Problem validating credentials. Please try  again.");
            }
        }

    }
}
