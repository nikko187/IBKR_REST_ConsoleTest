using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Security;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using System.Reflection.Metadata;


namespace IBKR_REST_ConsoleTest
{
    public class Program
    {
        public static Uri BaseUri = new Uri(baseURL);
        public const string baseURL = "https://localhost:5000/v1/api";
        public const string streamingURL = "wss://localhost:5000/v1/api/ws";
        public const string routeSymbolSearch = "/iserver/secdef/search";
        public const string routeAuthStatus = "/iserver/auth/status";
        public const string routeSnapshot = "/md/snapshot";
        public const string routeTickle = "/tickle";
        

        static async Task Main(string[] args)
        {
            int conID = 0;
            try
            {
                Console.WriteLine("Hello World!");
                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                var client = new HttpClient(handler);
                client.DefaultRequestHeaders.Add("User-Agent", "Console");

                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("symbol", "AAPL"));
                postData.Add(new KeyValuePair<string, string>("name", "true"));
                postData.Add(new KeyValuePair<string, string>("secType", "STK"));
                FormUrlEncodedContent content = new FormUrlEncodedContent(postData);

                var request = new HttpRequestMessage(HttpMethod.Post, baseURL + routeSymbolSearch)
                {
                    Method = HttpMethod.Post,
                    Content = content
                };

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(result);

                    var postResponse = JsonConvert.DeserializeObject<List<SecDef>>(result);
                    conID = postResponse[0].conid;
                    Console.WriteLine("\n" + conID);    // How do I get the "conid" from the response??

                }
                else
                {
                    Console.WriteLine("ERROR: " + response.StatusCode);
                }

                List<KeyValuePair<string, string>> postTickle = new List<KeyValuePair<string, string>>();
                postTickle.Add(new KeyValuePair<string, string>("", ""));
                FormUrlEncodedContent contentTickle = new FormUrlEncodedContent(postTickle);

                var request2 = new HttpRequestMessage(HttpMethod.Post, baseURL + routeTickle)
                {
                    Method = HttpMethod.Post,
                };
                var response2 = await client.SendAsync(request2);

                if (response.IsSuccessStatusCode)
                {
                    var result2 = response2.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(result2);
                }
                else
                {
                    Console.WriteLine("ERROR: " + response2.StatusCode);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
    }

        /*private static void AttemptA()
        {
            //var baseURL = "https://localhost:5000/v1/api";
            //HttpClientHandler handler = new HttpClientHandler();
            //handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Console");
            //var response = client.GetAsync(baseURL + "/iserver/auth/status");
            //string result = response.Content.ReadAsStringAsync().Result;

            
            string auth = "";
            var request = (HttpWebRequest)WebRequest("https://localhost:5000/v1/api/iserver/auth/status");

            // Allows you to bypass "AuthenticationException: The remote certificate is invalid according to the validation procedure." error. Don't to do this in production code...
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };

            
            request.Method = "POST";
            //request.Headers["Authorization"] = auth;
            request.Accept = "application/json";

            var response = (HttpWebResponse)request.GetResponse();

            Console.WriteLine(response.StatusCode);
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Console.WriteLine(responseString);
            
        }*/

    
}
