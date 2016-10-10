using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonRPC;

namespace BookItForMe
{
    class Program
    {

        static void Main(string[] args)
        {
            //START init variables
            var baseUrl = "http://user-api.simplybook.me";
            var companyLogin = "doctorpain";
            var userLogin = "admin";
            var userPassword = "pandaeyes";
            //END init variables

            //Get API token
            var token = GetToken(baseUrl, companyLogin, userLogin, userPassword);
            Console.WriteLine("Token: " + token);

            //Init API client
            var apiClient = new Client(baseUrl + "/admin/");
            apiClient.Headers.Add("X-Company-Login", companyLogin);
            apiClient.Headers.Add("X-User-Token", token);
            
            Request request = apiClient.NewRequest("getBookingsZapier");
            GenericResponse response = apiClient.Rpc(request);
            if (response.Result != null)
            {
                Console.WriteLine(response.Result.ToString());
            }
            else
            {
                throw new BadResponseException("ERROR No response from getEventList");
            }

            Console.ReadKey();
        }

        static string GetToken(string baseUrl, string companyLogin, string userLogin, string userPassword)
        {
            using (Client rpcClient = new Client(baseUrl + "/login"))
            {
                var tokenParams = new tokenParams
                {
                    companyLogin = companyLogin,
                    userLogin = userLogin,
                    userPassword = userPassword
                };
                
                Request request = rpcClient.NewRequest("getUserToken", JToken.FromObject(tokenParams));
                GenericResponse response = rpcClient.Rpc(request);

                if (response.Result != null)
                {
                    return response.Result.ToString();
                }

                
                throw new BadResponseException(string.Format("Error in response, code:{0} message:{1}",
                    response.Error.Code,
                    response.Error.Message));
            }
        }

        private class tokenParams
        {
            public string companyLogin { get; set; }
            public string userLogin { get; set; }
            public string userPassword { get; set; }
        }

        private class getBookingsParams
        {
            public Array[] param { get; set; }
        }

        [Serializable]
        public class BadResponseException : Exception
        {
            public BadResponseException() { }
            public BadResponseException(string message) : base(message) { }
            public BadResponseException(string message, Exception inner) : base(message, inner) { }
            protected BadResponseException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }
}
