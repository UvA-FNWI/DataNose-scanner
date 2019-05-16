using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataNoseScanner
{
    public class DataNoseKeyResponse
    {
        public string status;
        public string message;

        public DataNoseKeyResponse(string response)
        {
            JObject o = JObject.Parse(response);

            status = (string)o["status"];
            message = (string)o["message"];
        }
    }
}
