using System;
using System.Collections.Generic;
using System.Text;

#if __ANDROID__
using Android.Preferences;
using Android.Net.Wifi;
using Android.Net;
using Android.Content;
using Android.OS;
#endif

#if __IOS__

#endif

namespace DataNoseScanner.Common
{
    public class WifiHelper
    {
#if __ANDROID__

        private Context context = null;

        public WifiHelper()
        {
            this.context = Android.App.Application.Context;
        }

        public int CheckWIFI()
        {
            WifiManager wifi = (WifiManager)context.GetSystemService(Context.WifiService);
            if (wifi != null)
            {
                if (wifi.WifiState == Android.Net.WifiState.Disabled)
                    wifi.SetWifiEnabled(true);
                if (wifi.WifiState == Android.Net.WifiState.Enabling)
                    return 1;
                else if (wifi.WifiState != Android.Net.WifiState.Enabled)
                    return 0;
            }
            else
                return 0;

            ConnectivityManager conn = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
            if (conn != null)
            {
                NetworkInfo networkInfo = conn.ActiveNetworkInfo;
                if (networkInfo == null)
                    return 0;
                if (networkInfo.GetState() == NetworkInfo.State.Connecting)
                    return 1;
                else if (networkInfo.GetState() != NetworkInfo.State.Connected)
                    return 0;
            }
            else
                return 0;

            return 2;
        }
#endif


#if __IOS__
        public WifiHelper()
        {
            
        }

        public int CheckWIFI()
        {
            return 2;
        }

#endif
    }
}
