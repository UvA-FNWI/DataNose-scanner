using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace QRCross.DataNose
{
    public class DataNoseConnector
    {
        //public const string API_URL = "https://api-acc.datanose.nl/ScannerApp?key="; // "https://api.datanose.nl/ScannerApp?key=";
        private string API_URL = "https://api-acc.datanose.nl/ScannerApp";
        private HttpClient client;
        private string sUser;
        private string sPass;

        public DataNoseConnector(string apiurl, string user, string pass)
        {
            client = new HttpClient();
            api_url = apiurl;
            setUser(user, pass);
        }

        public DataNoseConnector(string user, string pass)
        {
            client = new HttpClient();
            setUser(user, pass);
        }

        public string api_url
        {
            get { return API_URL; }
            set { API_URL = value; }
        }

        public void setUser(string user, string pass)
        {
            sUser = user;
            sPass = pass;
        }

        public async Task<DataNoseKeyResponse> tryKey()
        {
            string request = API_URL + "?key=" + sPass;

            HttpResponseMessage response = await client.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();
  
                try
                {
                    return new DataNoseKeyResponse(body);
                }
                catch { }
            }

            return null;
        }

        public async Task<DataNoseCodeResponse> tryCode(string code)
        {
            string request = API_URL + "?key=" + sPass + "&code" + code;

            HttpResponseMessage response = await client.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();

                try
                {
                    return new DataNoseCodeResponse(body);
                }
                catch { }
            }

            return null;
        }

    }
}
