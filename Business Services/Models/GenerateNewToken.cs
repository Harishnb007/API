using Business_Services.Models.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business_Services.Models
{
  public class GenerateNewToken
    {
         
        // To do - move this to config file

      
        public string Decrypt(string cipherText)
        {
            string _Pwd = "This_is_just_a_token_text_for_dev";
            byte[] _Salt = new byte[] { 0x45, 0xF1, 0x61, 0x6e, 0x20, 0x00, 0x65, 0x64, 0x76, 0x65, 0x64, 0x03, 0x76 };
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_Pwd, _Salt);
            byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }

        public  byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
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

        public string GenerateToken(string userId, string password, int ClientId, string lcAuthToken,string Username,string resorucename,string logview,bool estatementenroll)
        {
            DateTime issuedOn = DateTime.Now;
            DateTime expiresOn = DateTime.Now.AddSeconds(Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));

            var tokendomain = new Token
            {
                //IssuedOn = issuedOn,
                UserId = userId,
                Lcauth = lcAuthToken,
                ExpiresOn = expiresOn,
                Password = password,
                ClientId = ClientId,
                UserName = Username,
                resourcename = resorucename,
                log = logview,
                eStatement = estatementenroll
            };

            return Encryptor.Encrypt(JsonConvert.SerializeObject(tokendomain));
        }
    }
}
