using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Services.Models;
using Business_Services.Models.Helpers;

namespace Business_Services
{
    public interface ITokenServices
    {
        /// <summary>
        /// Function to generate unique token with expiry, against the provided userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        // string GenerateToken(string userId, string password, int ClientId, string lcAuthToken, string UserName, string resourcename, string log, bool eStatementenroll);
        string GenerateToken(string userId, string password, int ClientId, string lcAuthToken, string UserName, string resourcename, string log, bool eStatemente, string LoanNumber);
        /// <summary>
        /// Function to validate token against expiry
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool ValidateToken(string token);

        Task<ResponseModel> TestAuthAsync();

        Task<ResponseWithToken> AuthenticateAsync(string userName, string password);

        string GetLctoken(string mobileToken);
    
    }
}
