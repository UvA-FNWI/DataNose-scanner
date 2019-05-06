using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataNoseScanner.DataNose
{
    public class DataNoseCodeResponse
    {
        public string status;
        public string id;
        public string student;
        public string programme;
        public string remarks;

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
