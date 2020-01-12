using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Exceptions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigitalTerminal.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Terminal : ContentPage
	{
        DigitalTerminal.Data.MeasurmentData MeasurmentData;
        public Terminal ()
		{
			InitializeComponent ();
            SendButton.Source = ImageSource.FromResource("DigitalTerminal.Images.send-button.png");
            MeasurmentData = new DigitalTerminal.Data.MeasurmentData();
            MeasurmentData.GetData();
            MeasurmentData.DataArived += () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DataLabel.Text += "Device: " + MeasurmentData.Data;
                });
            };            
        }
        private void SendData_Clicked(object sender, EventArgs e)
        {
            DataLabel.Text += "Me: "+EntryChat.Text + "\r\n";
            if (EntryChat.Text != "")
            {
                MeasurmentData.SendData(EntryChat.Text);
            }
        }       
    }
}