using Business_Services.Models.Helpers;
using Business_Services.Models.LC_WebApi_Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Business_Services.B2C_WebAPI
{
    class API_Connection
    {
       // public static Uri URL { get; set; }

        private static HttpClientHandler handler;
        private static HttpClient client;
        /*SIT URL*/
        private static Uri baseAddress = new Uri("https://lcuiqa.test.servicelinkfnf.com");
        /*UAT URL*/
         //private static Uri baseAddress = new Uri("https://lcui.test.servicelinkfnf.com");
        static API_Connection()
        {
          
            handler = new HttpClientHandler() { UseCookies = false };
            client = new HttpClient(handler) { BaseAddress = baseAddress };

            // To avoid certificate errors thrown due to the Fiddler interception
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        public static async Task<HttpResponseMessage> DeleteAsync(string tokenValue, string url)
        {

            Auth_GetToken tokens = await GetFormTokenAsync(tokenValue);

            var message = new HttpRequestMessage(HttpMethod.Delete, url);
            message.Headers.Add("Cookie", "locale=en-US; .lcauth=" + tokenValue);
            message.Headers.Add("cookieToken", tokens.cookieToken);
            message.Headers.Add("formToken", tokens.formToken);
            var result = await client.SendAsync(message);
            result.EnsureSuccessStatusCode();

            return result;
        }

        public static async Task<HttpResponseMessage> DeleteAsync(string tokenValue, string url, HttpContent content)
        {

            ResponseWithToken returnData = new ResponseWithToken();
            Auth_GetToken tokens = await GetFormTokenAsync(tokenValue);

            var message = new HttpRequestMessage(HttpMethod.Delete, url);
            message.Headers.Add("Cookie", "locale=en-US; .lcauth=" + tokenValue);
            message.Headers.Add("cookieToken", tokens.cookieToken);
            message.Headers.Add("formToken", tokens.formToken);

            message.Content = content;
            var result = await client.SendAsync(message);
            result.EnsureSuccessStatusCode();

            if (result.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> cookieValues))
            {
                string setCookieValue = HttpUtility.UrlDecode(cookieValues.FirstOrDefault());
                Regex regex = new Regex("lcauth=(.*?);");
                var v = regex.Match(setCookieValue);
                if (v != null)
                {
                    returnData.tokenValue = v.Groups[1].ToString();
                }
            }

            return returnData.message;
        }

        public static async Task<HttpResponseMessage> GetAsync(string tokenValue, string url)
        {

            var message = new HttpRequestMessage(HttpMethod.Get, url);
            message.Headers.Add("Cookie", "locale=en-US; .lcauth=" + tokenValue);
            var result = await client.SendAsync(message);
            result.EnsureSuccessStatusCode();

            return result;
        }
        public static async Task<HttpResponseMessage> GetAsync(string url)
        {
            var message = new HttpRequestMessage(HttpMethod.Get, url);
            message.Headers.Add("Cookie", "locale=en-US");
            var result = await client.SendAsync(message);
            result.EnsureSuccessStatusCode();

            return result;
        }

        public static async Task<ResponseWithToken> PostAsync(string url, HttpContent content)
        {

            ResponseWithToken returnData = new ResponseWithToken();

            var message = new HttpRequestMessage(HttpMethod.Post, url);
            message.Headers.Add("Cookie", "locale=en-US");
            message.Content = content;
            var result = await client.SendAsync(message);
            returnData.errorId = 0;
            returnData.tokenValue = "";
            returnData.errorMessage = "";
            returnData.changePassword = "N";
            returnData.authenticateResult = new Authenticate();

            if (result.IsSuccessStatusCode)
            {
                result.EnsureSuccessStatusCode();

                if (result.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> cookieValues))
                {
                    string setCookieValue = HttpUtility.UrlDecode(result.Headers.GetValues("Set-Cookie").FirstOrDefault());
                    Regex regex = new Regex("lcauth=(.*?);");
                    var v = regex.Match(setCookieValue);

                    var responseAsString = await result.Content.ReadAsStringAsync();
                    

                    if (responseAsString.Contains("loanPaid"))
                    {
                        Authenticate resultSet = JsonConvert.DeserializeObject<Authenticate>(responseAsString);
                        if (resultSet.objUserInfo.user.changePassword == "Y")
                        {
                            returnData.authenticateResult = resultSet;
                            returnData.changePassword = "Y";
                        }
                        else
                        {
                            returnData.tokenValue = v.Groups[1].ToString();
                        }
                    }
                    else
                    {
                        ErrorModel contentError = JsonConvert.DeserializeObject<ErrorModel>(responseAsString);
                        returnData.errorId = 1;
                        returnData.errorMessage = contentError.msg;
                    }
                    returnData.message = result;
                }
                else
                {
                    var responseAsString = await result.Content.ReadAsStringAsync();
                    ErrorModel resultSet = JsonConvert.DeserializeObject<ErrorModel>(responseAsString);
                    returnData.errorId = 1;
                    returnData.errorMessage = resultSet.msg;
                }
            }
            else
            {
                var responseAsString = await result.Content.ReadAsStringAsync();
                ErrorModel resultSet = JsonConvert.DeserializeObject<ErrorModel>(responseAsString);
                returnData.errorId = resultSet.errorID;
                returnData.errorMessage = resultSet.message;                
            }

            return returnData;
        }

        //Added by BBSR Team on 11th Jan 2018
        public static async Task<HttpResponseMessage> PostUserAsync(string url, HttpContent content)
        {

            ResponseWithToken returnData = new ResponseWithToken();
            Auth_GetToken tokens = await GetRegTokenAsync();

            var message = new HttpRequestMessage(HttpMethod.Post, url);
            message.Headers.Add("Cookie", "locale=en-US");
            message.Headers.Add("cookieToken", tokens.cookieToken);
            message.Headers.Add("formToken", tokens.formToken);
            message.Content = content;
            var result = await client.SendAsync(message);
            result.EnsureSuccessStatusCode();

            return result;
        }

        public static async Task<ResponseWithToken> PostUserRegisAsync(string url, HttpContent content)
        {
            ResponseWithToken returnData = new ResponseWithToken();
           Auth_GetToken tokens = await GetRegTokenAsync();

            var message = new HttpRequestMessage(HttpMethod.Post, url);
            message.Headers.Add("Cookie", "locale=en-US");
            message.Headers.Add("cookieToken", tokens.cookieToken);
            message.Headers.Add("formToken", tokens.formToken);
            message.Content = content;
            var result = await client.SendAsync(message);
            result.EnsureSuccessStatusCode();

            if (result.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> cookieValues))
            {
                string setCookieValue = HttpUtility.UrlDecode(cookieValues.FirstOrDefault());
                Regex regex = new Regex("lcauth=(.*?);");
                var v = regex.Match(setCookieValue);
                if (v != null)
                {
                    returnData.tokenValue = v.Groups[1].ToString();
                }
            }
           
         
            return returnData;
        }

        public static async Task<ResponseWithToken> PostAsync(string tokenValue, string url, HttpContent content)
        {

          
            Auth_GetToken tokens = await GetFormTokenAsync(tokenValue);
            
            var message = new HttpRequestMessage(HttpMethod.Post, url);
            message.Headers.Add("Cookie", "locale=en-US; .lcauth=" + tokenValue);
            message.Headers.Add("cookieToken", tokens.cookieToken);
            message.Headers.Add("formToken", tokens.formToken);
            message.Content = content;
            var result = await client.SendAsync(message);
            result.EnsureSuccessStatusCode();
            ResponseWithToken returnData = new ResponseWithToken();
            if (result.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> cookieValues))
            {
                string setCookieValue = HttpUtility.UrlDecode(cookieValues.FirstOrDefault());
                Regex regex = new Regex("lcauth=(.*?);");
                var v = regex.Match(setCookieValue);
                if (v != null)
                {
                    returnData.tokenValue = v.Groups[1].ToString(); 
                }
            }

            returnData.message = result;

            return returnData;
        }

        private static async Task<Auth_GetToken> GetFormTokenAsync(string tokenValue)
        {

            ResponseWithToken returnData = new ResponseWithToken();

            var message = new HttpRequestMessage(HttpMethod.Get, "/api/Auth/GetToken/");
            message.Headers.Add("Cookie", "locale=en-US; .lcauth=" + tokenValue);

            var response = await client.SendAsync(message);
            response.EnsureSuccessStatusCode();

            string returnedData = await response.Content.ReadAsStringAsync();
            Auth_GetToken tokenValues = JsonConvert.DeserializeObject<Auth_GetToken>(returnedData);

            return tokenValues;
        }

        private static async Task<Auth_GetToken> GetRegTokenAsync()
        {

            ResponseWithToken returnData = new ResponseWithToken();

            var message = new HttpRequestMessage(HttpMethod.Get, "/api/Register/GetToken/");
            message.Headers.Add("Cookie", "locale=en-US; .lcauth=");

            var response = await client.SendAsync(message);
            response.EnsureSuccessStatusCode();

            string returnedData = await response.Content.ReadAsStringAsync();
            Auth_GetToken tokenValues = JsonConvert.DeserializeObject<Auth_GetToken>(returnedData);

            return tokenValues;
        }
    }
}
