using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataNoseScanner
{
    public class DataNoseCodeResponse
    {
        public Action<double, double, double, double> HeightChanged;

        private double itemHeight1;
        public double ItemHeight1
        {
            get { return itemHeight1; }
            set
            {
                if (itemHeight1 != value)
                {
                    itemHeight1 = value;
                    if (HeightChanged != null)
                        HeightChanged(itemHeight1, itemHeight2, itemHeight3, itemHeight4);
                }
            }
        }

        private double itemHeight2;
        public double ItemHeight2
        {
            get { return itemHeight2; }
            set
            {
                if (itemHeight2 != value)
                {
                    itemHeight2 = value;
                    if (HeightChanged != null)
                        HeightChanged(itemHeight1, itemHeight2, itemHeight3, itemHeight4);
                }
            }
        }

        private double itemHeight3;
        public double ItemHeight3
        {
            get { return itemHeight3; }
            set
            {
                if (itemHeight3 != value)
                {
                    itemHeight3 = value;
                    if (HeightChanged != null)
                        HeightChanged(itemHeight1, itemHeight2, itemHeight3, itemHeight4);
                }
            }
        }

        private double itemHeight4;
        public double ItemHeight4
        {
            get { return itemHeight4; }
            set
            {
                if (itemHeight4 != value)
                {
                    itemHeight4 = value;
                    if (HeightChanged != null)
                        HeightChanged(itemHeight1, itemHeight2, itemHeight3, itemHeight4);
                }
            }
        }
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
