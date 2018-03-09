using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Services.Models;
using Newtonsoft.Json;
using Business_Services.Models.Helpers;
using System.Security.Cryptography;
using System.Net.Http;
using System.Net;
using System.Web;
using Business_Services.B2C_WebAPI;

namespace Business_Services
{
    public class TokenServices : ITokenServices
    {
        public TokenServices()
        {
            
        }

        public  string GenerateToken(string userId,string password,int ClientId, string lcAuthToken,string UserName,string resourcename,string log, bool eStatemente,string LoanNumber)
        {
            DateTime issuedOn = DateTime.Now;
            DateTime expiresOn = DateTime.Now.AddSeconds(Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));

          var Tracking =  trackinglog(lcAuthToken,log,resourcename);

            var tokendomain = new Token
            {
               //IssuedOn = issuedOn,
                UserId = userId,
                Lcauth = lcAuthToken,
                ExpiresOn = expiresOn,
                Password = password,
                ClientId = ClientId,
                UserName = UserName,
                resourcename =resourcename,
                log = log,
                eStatement=eStatemente,
                Loan_Number = LoanNumber
            };
           
            return Encryptor.Encrypt(JsonConvert.SerializeObject(tokendomain));
        }


        public async Task<string> trackinglog(string lcAuthToken, string log, string resourcename)
        {

            var eventId = 1;
            var toEmail = "";
            var actionName = "VIEW";

            var trackresponse = await API_Connection.GetAsync(lcAuthToken, "/api/Helper/AddTrackingInfo/?eventId=" + eventId + "&resourceName=" + resourcename + "&toEmail=" + toEmail + "&log=" + log + "&actionName=" + actionName);
            string returnedData = await trackresponse.Content.ReadAsStringAsync();
            return returnedData;
        }
        public async Task<ResponseWithToken> AuthenticateAsync(string userName, string password)
        {

            HttpContent content;
            ResponseWithToken response;
            Payment AuthUser = new Payment();
            AuthUser.loan_number = "";
            try
            {
                content = new FormUrlEncodedContent(new Dictionary<string, string> { { "userID", userName }, { "password", password }, { "ssn", "" } });
                response = await API_Connection.PostAsync("/api/Auth/Authenticate", content);

                return response;
            }
            catch (Exception Ex)
            {
                response = new ResponseWithToken();
                response.errorMessage = "Problem occurred trying to validate the user credentials. Please try again.";
                return response;
            }
        }

        public async Task<ResponseModel> TestAuthAsync()
        {
            try
            {
                
                string returnedData = "";
                string tokenValue = "";
                
                HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string> { { "userID", "0000100099" }, { "password", "Slloancare@1" } });
                ResponseWithToken response2 = await API_Connection.PostAsync("/api/Auth/Authenticate", content);
                    
                tokenValue = response2.tokenValue;

                var response3 = await API_Connection.GetAsync(tokenValue, "/api/MyAccount/GetAccountInfo/0000100099");
                returnedData = await response3.Content.ReadAsStringAsync();

                return new ResponseModel(returnedData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
        }
       
        public bool ValidateToken(string tokenData)
        {
            try
            {
                Token tokenObject = JsonConvert.DeserializeObject<Token>(Encryptor.Decrypt(tokenData));

                if (tokenObject != null && !(DateTime.Now > tokenObject.ExpiresOn))
                {
                    //tokenObject.ExpiresOn = tokenObject.ExpiresOn.AddSeconds(Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
                    return true;
                }
            }
            catch (Exception exp)  // Have used generic exception since both JsonConvert & Encryptor can throw errors
            {
              
                return false;
            }

            return false;
        }

        public string GetLctoken(string mobileToken)
        {
            Token tokenObject = JsonConvert.DeserializeObject<Token>(Encryptor.Decrypt(mobileToken));

            if (tokenObject !=  null)
            {
                return tokenObject.Lcauth;
            }

            return null;
        }
    }
}
