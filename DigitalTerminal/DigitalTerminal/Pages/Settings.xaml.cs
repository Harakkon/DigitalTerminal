using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebDav;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigitalTerminal.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Settings : ContentPage
	{
        public static Data.SettingParameters setPars;
        public static IWebDavClient _client = new WebDavClient();
        string documentsPath;
        string filePath;
        public Settings ()
		{
			InitializeComponent ();
            setPars = new Data.SettingParameters("kgeutestmail@ya.ru", "qwerty1234", "", "", 5, 5, "https://webdav.yandex.ru");

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            setPars.webLogin = EntryLogin.Text;
            setPars.webPass = EntryPasword.Text;
            setPars.timeBeep = Convert.ToInt32(EntryAlertValue.Text);
            setPars.timeWakeup = Convert.ToInt32(EntryTimeDelay.Text);
            setPars.webDavAdress = EntryWebDavAdress.Text;
        }

        private void LoginButton_Clicked_1(object sender, EventArgs e)
        {
            var clientParams = new WebDavClientParams
            {
                BaseAddress = new Uri(setPars.webDavAdress),
                Credentials = new NetworkCredential(setPars.webLogin, setPars.webPass)
            };
            _client = new WebDavClient(clientParams);
            documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            TryConnect();
        }

        public async void TryConnect()
        {
            try
            {
                await _client.Mkcol("Measurement Data");
                var result = await _client.Propfind("Measurement Data");
                if (result.IsSuccessful) await DisplayAlert("Оповещение", "Вход выполнен", "Ок");
                else await DisplayAlert("Оповещение", "Проверьте правильность ввода данных", "Ок");
            }
            catch { await DisplayAlert("Оповещение", "Не удалось соединится", "Ок"); }
        }
    }
}