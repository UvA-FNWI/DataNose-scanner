using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using DataNoseScanner.Common;
using DataNoseScanner.DataNose;

namespace DataNoseScanner
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private WifiHelper wifiHelper = null;
        private Settings settings = null;

        public LoginPage()
        {
            InitializeComponent();

            wifiHelper = new WifiHelper();
            wifiHelper.CheckWIFI();

            settings = new Settings();
            if (settings.SignedUp == true)
                Task.Run(() => LoginSuccessfull());
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            string sUser = User.Text;
            string sPass = Password.Text;
            string api_url = settings.server + settings.server_api;

            DataNoseConnector DNC = new DataNoseConnector(api_url, sUser, sPass);
            DataNoseKeyResponse response = await DNC.tryKey();
            if ((response != null) && (response.status == "valid-key"))
            {
                settings.UserID = sUser;
                settings.UserPass = sPass;
                settings.SignedUp = true;
                await LoginSuccessfull(response.message);
            }
            else
                LoginFailed();
        }

        private async Task LoginSuccessfull(string sMessage)
        {
            //DependencyService.Get<IToastMessage>().LongAlert(sMessage);
            await LoginSuccessfull();
        }

        private async Task LoginSuccessfull()
        {
            await Navigation.PushAsync(new MainPage());
        }

        private void LoginFailed()
        {
            DependencyService.Get<IToastMessage>().LongAlert("Incorrect username or password");
        }

    }
}