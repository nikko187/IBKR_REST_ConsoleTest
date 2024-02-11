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


namespace IBKR_Rest_Sample
{
    class Program
    {
        public static Uri BaseUri = new Uri(baseURL);
        public const string baseURL = "https://localhost:5000/v1/api";
        public const string routeSymbolSearch = "/iserver/secdef/search";
        public const string routeAuthStatus = "/iserver/auth/status";

        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello World!");
                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                var client = new HttpClient(handler);
                client.DefaultRequestHeaders.Add("User-Agent", "Console");

                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("symbol", "SPY"));
                postData.Add(new KeyValuePair<string, string>("name", "true"));
                postData.Add(new KeyValuePair<string, string>("secType", "STK"));
                FormUrlEncodedContent content = new FormUrlEncodedContent(postData);

                var request = new HttpRequestMessage(HttpMethod.Post, baseURL + routeSymbolSearch)
                {
                    Method = HttpMethod.Post,
                    Content = content
                };

                var response = await client.SendAsync(request);
                string result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(result);
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
}
