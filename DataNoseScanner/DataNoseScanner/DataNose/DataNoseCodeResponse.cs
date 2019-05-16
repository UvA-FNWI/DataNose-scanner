using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataNoseScanner
{
    public class DataNoseCodeResponse
    {
        public string status { get; set; }
        public string id { get; set; }
        public string student { get; set; }
        public string programme { get; set; }
        public string remarks { get; set; }

        public DataNoseCodeResponse()
        {

        }

        public DataNoseCodeResponse(string response)
        {
            JObject o = JObject.Parse(response);

            status = (string)o["status"];
            id = (string)o["id"];
            student = (string)o["student"];
            programme = (string)o["programme"];
            remarks = (string)o["remarks"];
        }

    }
}
