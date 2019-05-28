using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using DataNoseScanner.Common;

namespace DataNoseScanner
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private WifiHelper wifiHelper = null;
        private Settings settings = null;
        private ILoginManager loginManager = null;

        public LoginPage(ILoginManager loginmanager)
        {
            InitializeComponent();

            loginManager = loginmanager;

            wifiHelper = new WifiHelper();
            wifiHelper.CheckWIFI();

            settings = new Settings();
            User.Text = settings.UserID;
            //Password.Text = settings.UserPass;
            //if (settings.SignedUp == true)
            //    Task.Run(() => LoginSuccessfull());
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            string sUser = User.Text;
            string sPass = Password.Text;
            string api_url = settings.server + settings.server_api;

            DataNoseConnector DNC = new DataNoseConnector(api_url, new ScannerAccount() { User = sUser, Pass = sPass });
            DataNoseKeyResponse response = await DNC.tryKey();
            if ((response != null) && (response.status == "valid-key"))
            {
                settings.UserID = sUser;
                settings.UserPass = sPass;
                settings.SignedUp = true;
                LoginSuccessfull(response.message);
            }
            else
                LoginFailed();
        }

        private void LoginSuccessfull(string sMessage)
        {
            //DependencyService.Get<IToastMessage>().LongAlert(sMessage);
            LoginSuccessfull();
        }

        private void LoginSuccessfull()
        {
            loginManager.ShowMainPage();
            //await Navigation.PushAsync(new MainPage());
        }

        private void LoginFailed()
        {
            DependencyService.Get<IToastMessage>().LongAlert("Incorrect username or password");
        }

    }
}