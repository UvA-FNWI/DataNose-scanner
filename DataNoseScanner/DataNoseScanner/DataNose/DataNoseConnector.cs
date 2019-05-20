using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataNoseScanner
{
    public class DataNoseConnector
    {
        //public const string API_URL = "https://api-acc.datanose.nl/ScannerApp?key="; // "https://api.datanose.nl/ScannerApp?key=";
        private string API_URL = "https://api-acc.datanose.nl/scannerapp";
        private HttpClient client;
        private ScannerAccount Account;

        public DataNoseConnector(string apiurl, ScannerAccount account)
        {
            api_url = apiurl;
            client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(20);
            Account = account;
        }

        public DataNoseConnector(ScannerAccount account)
        {
            client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(20);
            Account = account;
        }

        public string api_url
        {
            get { return API_URL; }
            set { API_URL = value; }
        }

        public ScannerAccount account
        {
            set { Account = value; }
        }

        public async Task<DataNoseKeyResponse> tryKey()
        {
            string request = API_URL + "/signup";
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(Account);
#if __ANDROID__
            Android.Util.Log.Info("webapi", request);
            Android.Util.Log.Info("webapi", jsonString);
#endif 
            try
            {
                HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string body = await response.Content.ReadAsStringAsync();
#if __ANDROID__
                Android.Util.Log.Info("webapi", body);
#endif
                    return new DataNoseKeyResponse(body);
                }
                else
                {
#if __ANDROID__
                Android.Util.Log.Info("webapi", "post fail: " + response.StatusCode.ToString());
#endif
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        public async Task<DataNoseCodeResponse> tryCode(string code)
        {
            string request = API_URL + "/scan";
            ScannerScan scan = new ScannerScan() { Code = code, Account = Account };
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(scan);
#if __ANDROID__
            Android.Util.Log.Info("webapi", request);
            Android.Util.Log.Info("webapi", jsonString);
#endif 
            try
            {
                HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string body = await response.Content.ReadAsStringAsync();
#if __ANDROID__
                Android.Util.Log.Info("webapi", body);
#endif
                    return new DataNoseCodeResponse(body);
                }
                else
                {
#if __ANDROID__
                Android.Util.Log.Info("webapi", "post fail: " + response.StatusCode.ToString());
#endif
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

    }
}
