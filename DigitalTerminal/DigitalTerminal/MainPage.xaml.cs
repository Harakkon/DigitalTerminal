using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DigitalTerminal
{
    public partial class MainPage : MasterDetailPage
    {
        NavigationPage SettingsPage,MeasurmentPage,ConnectionPage,AboutPage;
        public MainPage()
        {
            InitializeComponent();
            ImageMaster.Source = ImageSource.FromResource("DigitalTerminal.Images.IconPhone.png");            
            SettingsPage = new NavigationPage(new Pages.Settings());
            ConnectionPage = new NavigationPage(new Pages.Connection());
            Detail = ConnectionPage;
            IsPresented = true;
        }

        private void Measurment_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new Pages.MeashurmentTabbedPage.Measurement());
            IsPresented = false;          
        }
        private void Terminal_Cliced(object sender, EventArgs e)
        {
            if (Pages.Connection.IsConnected)
            {
                Detail =  new NavigationPage(new Pages.Terminal());
                IsPresented = false;
            }
            else DisplayAlertNotConnected();
        }
        private void Connection_Clicked(object sender, EventArgs e)
        {
                Detail = ConnectionPage;
                IsPresented = false;
        }
        private void Settings_Clicked(object sender, EventArgs e)
        {
                Detail = SettingsPage;
                IsPresented = false;
                DisplayAlertNotConnected();
        }
        private void About_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new About());
            IsPresented = false;
        }
        private async void DisplayAlertNotConnected() { await DisplayAlert(null, "Сначала подключитесь к устройству", "ОК"); }
    }   
}
