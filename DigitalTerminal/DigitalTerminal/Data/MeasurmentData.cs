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
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace DigitalTerminal.Data
{
    class MeasurmentData
    {
        public static ObservableCollection<DataStorage> DataCollection = new ObservableCollection<DataStorage>();
        //public List<DataStorage> DataStore = new List<DataStorage>();
        //bool timerAlive = true;
        byte[] asciiBytes;
        public delegate void GetDataHandler();
        public event GetDataHandler DataArived;
        public string Data { get; set; }
        public async void StartUpdate() { await Pages.Connection.characteristic.StartUpdatesAsync(); }
        public async void StopUpdate() { await Pages.Connection.characteristic.StopUpdatesAsync(); }
        public void GetData()
        {
            //Обработчик события => изменение значения характеристики
            //Необходим для получения данных с устройства
            Pages.Connection.characteristic.ValueUpdated += (o, args) =>
            {
                var bytes = args.Characteristic.Value;
                Data=(System.Text.Encoding.ASCII.GetString(bytes));
                DataArived();
            };
        }
        public async void SendData(string str)
        {           
            if (str != "")
            {
                asciiBytes = Encoding.ASCII.GetBytes(str);
                await Pages.Connection.characteristic.WriteAsync(asciiBytes);
            }
        }
        
    }

}
