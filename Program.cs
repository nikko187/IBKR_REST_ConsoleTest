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
using WebSocketSharp;
using WebSocketSharp.Net.WebSockets;
using System.Net.Mail;

namespace IBKR_REST_ConsoleTest
{
    public class Program
    {
        public static Uri BaseUri = new Uri(baseURL);
        public static Uri socketUri = new Uri(streamingURL);
        public const string baseURL = "https://localhost:5000/v1/api";
        public const string streamingURL = "wss://localhost:5000/v1/api/ws";
        public const string routeSymbolSearch = "/iserver/secdef/search";   //POST
        public const string routeAuthStatus = "/iserver/auth/status";
        public const string routeReauth = "/iserver/reauthenticate";        //POST
        public const string routeSnapshot = "/md/snapshot";
        public const string routeTickle = "/tickle";                        //POST
        public const string routeSSO = "/sso/validate";                     //GET

        public static string conID = "";

        static async Task Main(string[] args)
        {         
            string symbolName = "";
            string session = "";
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
            try
            {
                Console.WriteLine("Hello World!");
                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                var client = new HttpClient(handler);
                client.DefaultRequestHeaders.Add("User-Agent", "Console");

                // ### RE-AUTH FOR GOOD MEASURE ### //
                List<KeyValuePair<string, string>> getSSO = new List<KeyValuePair<string, string>>();
                getSSO.Add(new KeyValuePair<string, string>("", ""));
                FormUrlEncodedContent contentSSO = new FormUrlEncodedContent(getSSO);

                var requestAuth = new HttpRequestMessage(HttpMethod.Get, baseURL + routeSSO)
                {
                    Method = HttpMethod.Get,
                    Content = contentSSO
                };
                var responseAuth = await client.SendAsync(requestAuth);

                if (responseAuth.IsSuccessStatusCode)
                {
                    var resultAuth = responseAuth.Content.ReadAsStringAsync().Result;

                    var jsonAuth = JsonConvert.DeserializeObject<SSOValidate>(resultAuth);
                    Console.WriteLine(jsonAuth.RESULT);
                }
                else
                {
                    Console.WriteLine("ERROR: " + responseAuth.StatusCode);
                }

                // ####### TICKLE SERVER FOR AUTHENTICATION AND SESSION ###### //
                List<KeyValuePair<string, string>> postTickle = new List<KeyValuePair<string, string>>();
                postTickle.Add(new KeyValuePair<string, string>("", ""));
                FormUrlEncodedContent contentTickle = new FormUrlEncodedContent(postTickle);

                var request2 = new HttpRequestMessage(HttpMethod.Post, baseURL + routeTickle)
                {
                    Method = HttpMethod.Post,
                };
                var response2 = await client.SendAsync(request2);

                if (response2.IsSuccessStatusCode)
                {
                    var result2 = response2.Content.ReadAsStringAsync().Result;

                    var jsonTickle = JsonConvert.DeserializeObject<Tickle>(result2);
                    Console.WriteLine(jsonTickle.session);
                    session = jsonTickle.session;
                }
                else
                {
                    Console.WriteLine("ERROR: " + response2.StatusCode);
                }


                // ####### GET CONTRACT INFO ####### //
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("symbol", "TWLO"));
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
                    //Console.WriteLine(result);

                    var postResponse = JsonConvert.DeserializeObject<List<SecDef>>(result);
                    conID = postResponse[0].conid;
                    symbolName = postResponse[0].companyName;
                    Console.WriteLine("\n" + conID + "\n" + symbolName);

                }
                else
                {
                    Console.WriteLine("ERROR: " + response.StatusCode);
                }


                // ########## WEBSOCKET DATA STREAM ########## //  SCOPED
                List<KeyValuePair<string, string>> wsSession = new List<KeyValuePair<string, string>>();
                wsSession.Add(new KeyValuePair<string, string>("session", session));
                FormUrlEncodedContent contentWS = new FormUrlEncodedContent(wsSession);

                var wsStream = new WebsocketStream
                {
                    fields = "[31]"
                };
                string smdJson = JsonConvert.SerializeObject(wsStream);

                WebSocket ws = new WebSocket(streamingURL);
                
                ws.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.None;

                ws.Connect();
                ws.Send("smd+" + conID + "+{\"fields\":[\"31\"]}");
                ws.OnOpen += Ws_OnOpen;
                ws.OnMessage += Ws_OnMessage;
                ws.OnError += Ws_OnError;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            Console.ReadLine();
        }

        private static void Ws_OnOpen(object sender, EventArgs e)
        {
            Console.WriteLine(" ## CONNECTION OPEN ## " + e);
            string data = "smd+" + conID + "+{\"fields\":[\"31\"]}";

        }

        private static void Ws_OnMessage(object sender, WebSocketSharp.MessageEventArgs e)
        {
            Console.WriteLine("DATA: " + e.Data);
        }
        private static void Ws_OnError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            Console.WriteLine("WS_ERROR: " + e.Message);
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
