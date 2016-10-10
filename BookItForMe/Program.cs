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
            var baseUrl = "http://user-api.simplybook.me";
            var ApiKey = "3533573756a1e9e08245db98a8b2752240649e0ccd0e331f85407c5788ba68ad";
            var Secret = "2742dffc3c60af8e42cfa4cce84ab731bcd2483f79adb87331746081448801b3";
            var token = GetToken(baseUrl, ApiKey);
            Console.WriteLine("Token: " + token);

            var apiClient = new Client(baseUrl + "/");
            Console.ReadKey();
        }

        static string GetToken(string baseUrl, string ApiKey)
        {
            using (Client rpcClient = new Client(baseUrl + "/login"))
            {
                var tokenParams = new tokenParams
                {
                    companyLogin = "doctorpain",
                    ApiKey = ApiKey
                };
                
                Request request = rpcClient.NewRequest("getToken", JToken.FromObject(tokenParams));
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
            public string ApiKey { get; set; }
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
