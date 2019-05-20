using DataNoseScanner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DataNoseScanner
{
    public partial class MainPage : ContentPage
    {
        private DataNoseConnector DNC;
        private Settings settings = null;
        private ILoginManager loginManager = null;

        private ObservableCollection<DataNoseCodeResponse> Responses = new ObservableCollection<DataNoseCodeResponse>();

        public MainPage(ILoginManager loginmanager)
        {
            InitializeComponent();

            loginManager = loginmanager;

            settings = new Settings();
            string sUser = settings.UserID;
            string sPass = settings.UserPass;
            string api_url = settings.server + settings.server_api;
            DNC = new DataNoseConnector(api_url, new ScannerAccount() { User = sUser, Pass = sPass });

            Responses.Add(new DataNoseCodeResponse() { id = "",  student = "Tap the SCAN button", programme = "to begin", remarks = "", HeightChanged = CarouselItemSizeChanged });

            carouselInfo.ItemsSource = Responses;
        }

        private void CarouselItemSizeChanged(double h1, double h2, double h3, double h4)
        {
            Console.WriteLine("carousel height: " + h1.ToString() + " " + h2.ToString() + " " + h3.ToString() + " " + h4.ToString());
            carouselInfo.HeightRequest = h1 + h2 + h3 + h4 + 9 * 6;
        }

        private async void btnScan_Clicked(object sender, EventArgs e)
        {
            var scan = new ZXingScannerPage();
            scan.IsAnalyzing = true;
            
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    busyBG.IsVisible = true;
                    busyIndicator.IsRunning = true;
                    busyIndicator.IsVisible = true;

                    await Navigation.PopAsync();

                    DataNoseCodeResponse response = await DNC.tryCode(result.Text);
                    if (response != null)
                    {
                        response.HeightChanged = CarouselItemSizeChanged;
                        Responses.Add(response);
                        carouselInfo.Position = Responses.Count - 1;
                    }

                    busyBG.IsVisible = false;
                    busyIndicator.IsRunning = false;
                    busyIndicator.IsVisible = false;
                });
            };

            
        }

        private void btnSignout_Clicked(object sender, EventArgs e)
        {
            loginManager.SignOut();
        }

        private void btnDatanose_Clicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://datanose.nl"));
        }

    }

}
