using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DataNoseScanner.Common;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DataNoseScanner
{
    public partial class App : Application, ILoginManager
    {
        private Settings settings = null;

        public App()
        {
            InitializeComponent();

            settings = new Settings();
            if (settings.SignedUp == true)
                MainPage = new NavigationPage(new MainPage(this));
            else
                MainPage = new NavigationPage(new LoginPage(this));
        }

        public void ShowMainPage()
        {
            MainPage = new NavigationPage(new MainPage(this));
        }

        public void SignOut()
        {
            settings.SignedUp = false;
            MainPage = new NavigationPage(new LoginPage(this));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        
    }
}
