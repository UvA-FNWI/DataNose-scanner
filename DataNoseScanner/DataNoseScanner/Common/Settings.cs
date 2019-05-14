using System;
using System.Collections.Generic;
using System.Text;

#if __ANDROID__
using Android.Content;
using Android.Preferences;
using Android.Runtime;
#endif

#if __IOS__
using Foundation;
#endif

namespace DataNoseScanner.Common
{
    public class Settings
    {
#if __ANDROID__
        public ISharedPreferences settings = null;
        private Context context = null;

        public Settings()//Context context)
        {
            context = Android.App.Application.Context;
            this.settings = context.GetSharedPreferences("DatanoseScanner", FileCreationMode.Private);
        }
#endif
#if __IOS__
        public Settings()
        {

        }
#endif

        public string server
        {
            get
            {
                string sServer = getString("server");
                if ((sServer == null) || (sServer == ""))
                    sServer = @"https://api-acc.datanose.nl/"; // @"https://api.datanose.nl/";
                return sServer;
            }
            set
            {
                putString("server", value);
            }
        }

        public string server_api
        {
            get
            {
                string sEntry = getString("server_api");
                if ((sEntry == null) || (sEntry == ""))
                    sEntry = @"scannerapp";
                return sEntry;
            }
            set
            {
                putString("server_api", value);
            }
        }


        #region genaral settings
        public string UserID
        {
            get
            {
                return getString("user_id");
            }
            set
            {
                putString("user_id", value);
            }
        }

        public string UserPass
        {
            get
            {
                return getString("user_pass");
            }
            set
            {
                putString("user_pass", value);
            }
        }

        public bool SignedUp
        {
            get
            {
                return getBool("user_signedup");
            }
            set
            {
                putBool("user_signedup", value);
            }
        }
        #endregion

        #region private cross platform functions
        private void setDateTime(string sName, DateTime value)
        {
            putInt(sName + "_year", value.Year);
            putInt(sName + "_month", value.Month);
            putInt(sName + "_day", value.Day);
            putInt(sName + "_hour", value.Hour);
            putInt(sName + "_min", value.Minute);
            putInt(sName + "_sec", value.Second);
        }

        private DateTime getDateTime(string sName)
        {
            int iYear = getInt(sName + "_year");
            int iMonth = getInt(sName + "_month");
            int iDay = getInt(sName + "_day");
            int iHour = getInt(sName + "_hour");
            int iMin = getInt(sName + "_min");
            int iSec = getInt(sName + "_sec");

            DateTime dtRet = DateTime.Now.AddHours(-1);
            try
            {
                dtRet = new DateTime(iYear, iMonth, iDay, iHour, iMin, iSec);
            }
            catch { }

            return dtRet;
        }

        #endregion

#if __ANDROID__

        #region private functions

        private void putString(string name, string value)
        {
            ISharedPreferencesEditor editor = settings.Edit();
            editor.PutString(name, value);
            editor.Apply();
        }

        private string getString(string name)
        {
            return settings.GetString(name, "");
        }

        private void putBool(string name, bool value)
        {
            ISharedPreferencesEditor editor = settings.Edit();
            editor.PutBoolean(name, value);
            editor.Apply();
        }

        private bool getBool(string name)
        {
            return settings.GetBoolean(name, false);
        }

        private void putInt(string name, int value)
        {
            ISharedPreferencesEditor editor = settings.Edit();
            editor.PutInt(name, value);
            editor.Apply();
        }

        private int getInt(string name)
        {
            return settings.GetInt(name, 0);
        }

        
        private void putIntArray(ref ISharedPreferencesEditor editor, string key, int[] vals)
        {
            string sTmp = "";
            for (int i = 0; i < vals.Length; i++)
                sTmp += vals[i].ToString() + " ";
            editor.PutString(key, sTmp);

            //List<string> lstItems = new List<string>();
            //for (int i = 0; i < vals.Length; i++)
            //    lstItems.Add(vals[i].ToString());
            //editor.PutStringSet(key, lstItems);
        }

        private int[] getIntArray(string key)
        {
            string sItems = settings.GetString(key, "");
            if (sItems == "")
                return new int[0];
            string[] lstItems = sItems.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            //var bla = settings.GetStringSet(key, null);
            //JavaSet<string> setItems = (JavaSet<string>)bla;
            //string[] lstItems = new string[bla.Count];
            //bla.CopyTo(lstItems, 0);
            int[] vals = new int[lstItems.Length];
            //int i = 0;
            //foreach (string item in lstItems)
            for (int i = 0; i < vals.Length; i++)
            {
                try
                {
                    vals[i] = Convert.ToInt32(lstItems[i]);
                }
                catch { }
                // i++;
            }
            return vals;
        }

        //private void pushStringToArray(string sArrayName, string sValue)
        //{

        //}

        private void putStringArray(string sArrayName, List<string> lstVal)
        {
            ISharedPreferencesEditor editor = settings.Edit();
            //editor.PutStringSet(sArrayName, lstVal);
            //editor.Apply();

            List<int> lstKeys = new List<int>();
            for (int i = 0; i < lstVal.Count;i++)
            {
                lstKeys.Add(i);
                editor.PutString(sArrayName + "_" + i.ToString(), lstVal[i]);
            }
            putIntArray(ref editor, sArrayName + "_keys", lstKeys.ToArray());

            editor.Apply();
        }

        private List<string> getStringArray(string sArrayName)
        {
            List<string> lstVal = new List<string>();

            int[] iKeys = getIntArray(sArrayName + "_keys");
            for (int i = 0; i < iKeys.Length;i++)
            {
                string sTmp = settings.GetString(sArrayName + "_" + iKeys[i].ToString(), "");
                lstVal.Add(sTmp);
            }

            //List<string> lstVal = (List<string>)settings.GetStringSet(sArrayName, new List<string>());
            return lstVal;
        }

        //private void removeStringFromArray(string sArrayName, int iIndex)
        //{

        //}

        #endregion

#endif

#if __IOS__

        #region private functions

        private void putString(string name, string value)
        {
            NSUserDefaults.StandardUserDefaults.SetString(value, name);
        }

        private string getString(string name)
        {
            return (string)NSUserDefaults.StandardUserDefaults.StringForKey(name);
        }

        private void putBool(string name, bool value)
        {
            NSUserDefaults.StandardUserDefaults.SetBool(value, name);
        }

        private bool getBool(string name)
        {
            return (bool)NSUserDefaults.StandardUserDefaults.BoolForKey(name);
        }

        private void putInt(string name, int value)
        {
            NSUserDefaults.StandardUserDefaults.SetInt(value, name);
        }

        private int getInt(string name)
        {
            return (int)NSUserDefaults.StandardUserDefaults.IntForKey(name);
        }

        private void putIntArray(string key, int[] vals)
        {
            string sTmp = "";
            for (int i = 0; i < vals.Length; i++)
                sTmp += vals[i].ToString() + " ";
            NSUserDefaults.StandardUserDefaults.SetString(sTmp, key);

            //List<string> lstItems = new List<string>();
            //for (int i = 0; i < vals.Length; i++)
            //    lstItems.Add(vals[i].ToString());
            //editor.PutStringSet(key, lstItems);
        }

        private int[] getIntArray(string key)
        {
            string sItems = NSUserDefaults.StandardUserDefaults.StringForKey(key);
            if ((sItems == "") || (sItems == null))
                return new int[0];
            string[] lstItems = sItems.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            //var bla = settings.GetStringSet(key, null);
            //JavaSet<string> setItems = (JavaSet<string>)bla;
            //string[] lstItems = new string[bla.Count];
            //bla.CopyTo(lstItems, 0);
            int[] vals = new int[lstItems.Length];
            //int i = 0;
            //foreach (string item in lstItems)
            for (int i = 0; i < vals.Length; i++)
            {
                try
                {
                    vals[i] = Convert.ToInt32(lstItems[i]);
                }
                catch { }
                // i++;
            }
            return vals;
        }

        private void putStringArray(string sArrayName, List<string> lstVal)
        {
            //TODO
        }

        private List<string> getStringArray(string sArrayName)
        {
            //TODO
            return new List<string>();
        }

        #endregion

#endif
    }

}