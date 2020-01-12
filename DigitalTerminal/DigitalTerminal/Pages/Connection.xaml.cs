using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigitalTerminal.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Connection : ContentPage
    {
        //Управление BLE парраметрами
        IBluetoothLE ble;
        //Bluetooth адаптер
        IAdapter adapter;
        //Различимая коллекция для хранения списка найденных устройств
        ObservableCollection<IDevice> deviceList=new ObservableCollection<IDevice>();
        //Оттдельный экземпляр устройства, необходимо для соединения.
        IDevice device;
        //Характеристика для отправки и получения данных
        public static ICharacteristic characteristic;
        //Сервис для доступа к характеристике
        IService service;
        public static bool IsConnected=false;
        public Connection ()
		{
            InitializeComponent ();
            //Установка источника данных для привязки к UI
            listView.ItemsSource =  deviceList;
            //Обработчик события ItemSelected
            listView.ItemSelected += ListView_ItemSelected;
            //Базовые настройки для доступа к модулям мобильного устройства
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            //Установка кнопок
            ToolbarItem scan = new ToolbarItem();
            ToolbarItems.Add(new ToolbarItem("Scan", "IconBluetooth.png", () =>
            {
                ScanButton_Clicked();
            }));
            ToolbarItems.Add(new ToolbarItem("Connect", "IconConnect.png", () =>
            {
                Connect_Clicked();
            }));       
        }       
        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //Возврат выбранного устройства из списка
            foreach (var d in deviceList)
                if (e.SelectedItem.ToString() == d.Name)
                    device = d;
        }

        private async void ScanButton_Clicked()
        {
            deviceList.Clear();
            adapter.DeviceDiscovered += (s, a) => deviceList.Add(a.Device);
            //Обработчик события => найденно новое устройство
            await adapter.StartScanningForDevicesAsync();//Асинхронный метод для поиска новых устройств
        }
        private async void Connect_Clicked()
        {
            //Метод для подключения к устройству. Поскольку GUID сервисов
            //и харрактеристик для устройств на базе CC2541 одинаковые,
            //можно не выполнять их глобальный поиск по дискрипторам и свойствам                
            if (device != null)
            {
                try
                {
                    await adapter.ConnectToDeviceAsync(device);
                    service = await device.GetServiceAsync(Guid.Parse("0000ffe0-0000-1000-8000-00805f9b34fb"));
                    characteristic = await service.GetCharacteristicAsync(Guid.Parse("0000ffe1-0000-1000-8000-00805f9b34fb"));
                    await DisplayAlert(null, "Соединение успешно", "ОК");
                    IsConnected = true;                   
                }
                catch (DeviceConnectionException ex)
                {
                    await DisplayAlert("Ошибка!", "Не удалось соединится" + ex.Message, "ОК");
                }
            }
            else
            { await DisplayAlert("Ошибка!", "Выберете устройство для соединения" , "ОК"); }
        }
        

    }
}