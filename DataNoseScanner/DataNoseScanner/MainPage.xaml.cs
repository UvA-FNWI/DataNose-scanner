using DataNoseScanner.DataNose;
using DataNoseScanner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace DataNoseScanner
{
    public partial class MainPage : ContentPage
    {
        private DataNoseConnector DNC;
        private Settings settings = null;

        public MainPage()
        {
            InitializeComponent();

            settings = new Settings();
            string sUser = settings.UserID;
            string sPass = settings.UserPass;
            string api_url = settings.server + settings.server_api;
            DNC = new DataNoseConnector(api_url, sPass);
        }

        private async void btnScan_Clicked(object sender, EventArgs e)
        {
            var scan = new ZXingScannerPage();
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    mycode.Text = result.Text;

                    DataNoseCodeResponse response = await DNC.tryCode(result.Text);
                    if (response != null)
                    {
                        apiresult.Text = response.status;
                        lblStudent.Text = response.student;
                        lblProgramme.Text = response.programme;
                        lblRemarks.Text = response.remarks;
                    }
                });
            };
        }

    }
}
