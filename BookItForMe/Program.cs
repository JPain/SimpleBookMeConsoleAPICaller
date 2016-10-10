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
            var tokenKey = GetTokenKey(baseUrl, ApiKey);
            Console.ReadKey();
        }

        static string GetTokenKey(string baseUrl, string ApiKey)
        {
            using (Client rpcClient = new Client(baseUrl + "/login"))
            {
                JToken x = JToken.Parse("{companyLogin: 'doctorpain', apiKey: '3533573756a1e9e08245db98a8b2752240649e0ccd0e331f85407c5788ba68ad'}");
                Request request = rpcClient.NewRequest("getToken", x);
                GenericResponse response = rpcClient.Rpc(request);
                JToken result;
                if (response.Result != null)
                {
                    result = response.Result;
                }
                else
                {
                    Console.WriteLine(string.Format("Error in response, code:{0} message:{1}", 
                        response.Error.Code, 
                        response.Error.Message));
                    return "";
                }

                Console.WriteLine(result.ToString());
                    
            }
            return "abc123";
        }
    }
}
