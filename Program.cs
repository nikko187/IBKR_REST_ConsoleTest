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
using IBKR_REST_ConsoleTest;


namespace IBKR_Rest_Sample
{
    public class Program
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
                string result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(result);
                var jsonDS = JsonConvert.DeserializeObject(result);

                
                Console.WriteLine(jsonDS);
                
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class SecDef
    {
        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public string conid { get; set; }
        public string companyHeader { get; set; }
        public string companyName { get; set; }
        public string symbol { get; set; }
        public string description { get; set; }
        public string restricted { get; set; }
        public object fop { get; set; }
        public string opt { get; set; }
        public string war { get; set; }
        public Section[] sections { get; set; }
        public string secType { get; set; }
        public Issuer[] issuers { get; set; }
        public int bondid { get; set; }
    }

    public class Section
    {
        public string secType { get; set; }
        public string exchange { get; set; }
        public string months { get; set; }
        public string conid { get; set; }
    }

    public class Issuer
    {
        public string id { get; set; }
        public string name { get; set; }
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
