using DataNoseScanner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using System.Collections.ObjectModel;

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

            //global::Xamarin.Forms.Forms.Init(this, );
            //global::ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            settings = new Settings();
            string sUser = settings.UserID;
            string sPass = settings.UserPass;
            string api_url = settings.server + settings.server_api;
            DNC = new DataNoseConnector(api_url, new ScannerAccount() { User = sUser, Pass = sPass });

            Responses.Add(new DataNoseCodeResponse() { id = "1234",  student = "Tap the SCAN button to begin", programme = "bla", remarks = "remarks" });

            //names = new ObservableCollection<string> { "bla", "iets", "haha" };
            //names.Add(new PersonInfo() { Name = "bla", programme = "1" });
            //names.Add(new PersonInfo() { Name = "iets", programme = "2" });
            //for (int i = 0; i < 1000; i++)
            //    names.Add( "item " + i.ToString());
            carouselInfo.ItemsSource = Responses;
            
            
            
            
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
                    await Navigation.PopAsync();
                    //mycode.Text = result.Text;
                    //lblStudent.Text = "Processing...";
                    //lblProgramme.Text = "";
                    //lblRemarks.Text = "";
                    //names.Add(new PersonInfo() { Name = result.Text, programme = "scan" } );
                    //carouselInfo.Position = names.Count - 1;

                    DataNoseCodeResponse response = await DNC.tryCode(result.Text);
                    if (response != null)
                    {
                        Responses.Add(response);
                        carouselInfo.Position = Responses.Count - 1;
                        //apiresult.Text = response.status;
                        //lblStudent.Text = response.student;
                        //lblProgramme.Text = response.programme;
                        //lblRemarks.Text = response.remarks;
                    }
                    //else
                    //    lblStudent.Text = "Failed connecting with server";
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

    //public class PersonInfo
    //{
    //    public string Name { get; set; }
    //    public string programme { get; set; }
    //}
}
