using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigitalTerminal.Pages.MeashurmentTabbedPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MeasurementMainPage : ContentPage
    {

        private double currValue;
        public double CurrValue
        {
            get { return currValue; }
            set
            {
                currValue = value;
                OnPropertyChanged(nameof(CurrValue)); //Оповещение об изменении значения
            }
        }
        private double minValue;
        public double MinValue
        {
            get { return minValue; }
            set
            {
                minValue = value;
                OnPropertyChanged(nameof(MinValue)); //Оповещение об изменении значения
            }
        }
        private double maxValue;
        public double MaxValue
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
                OnPropertyChanged(nameof(MaxValue)); //Оповещение об изменении значения
            }
        }
        private string timeValue;
        public string TimeValue
        {
            get { return timeValue; }
            set
            {
                timeValue = value;
                OnPropertyChanged(nameof(TimeValue)); //Оповещение об изменении значения
            }
        }

        DateTime timer = new DateTime();
        DateTime timerGo = new DateTime();
        bool alive = false;
        bool isStarted =false;
        static Data.MeasurmentData CurrMeasurmentData;
        string state = "1";
        string type = "Ом";
        bool timerActive = false;
        bool alertActive = false;
        public MeasurementMainPage ()
		{           
            CurrValue = 0.000;
            MinValue = 999;
            MaxValue = -999;
            TimeValue = "0:00:00";
            InitializeComponent();
            CurrMeasurmentData = new Data.MeasurmentData();            
            CurrentValue.BindingContext = this;
            nMinValue.BindingContext = this;
            nMaxValue.BindingContext = this;
            nTimeValue.BindingContext = this;
            CurrMeasurmentData.StartUpdate();           
            CurrMeasurmentData.GetData();
            CurrMeasurmentData.DataArived += () =>
            {
                if (isStarted)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        CurrValue = Convert.ToDouble(CurrMeasurmentData.Data.Replace("\r\n","").Replace(".",","));
                        Data.MeasurmentData.DataCollection.Add(new Data.DataStorage(CurrValue, Data.MeasurmentData.DataCollection.Count,(DateTime.Now).ToString("T"),type));
                        MeasurementPlotPage.AddDataToPlot(Data.MeasurmentData.DataCollection.Count + 10, CurrValue);
                        if (CurrValue < MinValue) MinValue = CurrValue;
                        if (CurrValue > MaxValue) MaxValue = CurrValue;       
                        if(alertActive)
                        {
                            alertActive = false;
                            if (CurrValue > Settings.setPars.timeBeep)
                                DisplayAlert();
                        }

                    });
                }
            };           
            SwitchImage.Source = ImageSource.FromResource("DigitalTerminal.Images.Switch.png");
            SwitchFlags.Source = ImageSource.FromResource("DigitalTerminal.Images.SwitchFlags.png");


            ToolbarItem R = new ToolbarItem();
            ToolbarItems.Add(new ToolbarItem("R", "", () =>
            {
                R_Clicked();
            }));


        }
        private async void DisplayAlert()
        {
            await DisplayActionSheet("Превышено заданное значение", "ОК", null, null, null);
            alertActive = true;
        }
        TimerCallback tm;
        Timer timerObj;
        private void timeron()
        {
            var tempTimeGo = DateTime.Now.AddSeconds(Settings.setPars.timeWakeup);
            //if(timerActive==true)//
        }
        bool stateTimer = true;
        public async void Count(object obj)
        {
            if(timerActive)
            {
                //if(stateTimer)
                //{
                //    isStarted = false;
                //    StartButton.Text = "Старт";
                //    //CurrMeasurmentData.SendData("5");
                //    stateTimer = false;
                //}
                //if (!stateTimer)
               // {
                    CurrMeasurmentData.SendData(state);
                    await Task.Delay(2000);
                    CurrMeasurmentData.SendData("5");
                //}
            }
            
        }
        private async void R_Clicked()
        {
            string s=await DisplayActionSheet("Выберете сопротивление", "Отмена", null, "230", "9100", "100к");
            if(s=="230")
                CurrMeasurmentData.SendData("7");
            if (s == "9100")
                CurrMeasurmentData.SendData("8");
            if (s == "100к")
                CurrMeasurmentData.SendData("9");

        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (3 * height > 2 * height)
                mainGrid.HeightRequest = 2.0 / 3 * width;
            else
                mainGrid.WidthRequest = 3.0 / 2 * height;
        }

        private void OmButton_Clicked(object sender, EventArgs e)
        {
            state = "1";
            type = "Ом"; 
            CurrMeasurmentData.SendData(state);
            SwitchImage.Rotation = 0;
        }

        private void VButton_Clicked(object sender, EventArgs e)
        {
            state = "2"; 
            type = "В";
            CurrMeasurmentData.SendData(state);
            SwitchImage.Rotation = 270;
        }

        private void UfmButton_Clicked(object sender, EventArgs e)
        {
            state = "4";
             type = "мФ";

            CurrMeasurmentData.SendData(state);
            SwitchImage.Rotation = 180;
        }
        bool temp = true;
        private void AButton_Clicked(object sender, EventArgs e)
        {
            state = "3";
            type = "А";
           // CurrMeasurmentData.SendData(state);
            SwitchImage.Rotation = 90;
            if(temp==true)
            {
                CurrValue = 0;
                temp = false;
            }
            else
            {
                StartButton_Clicked(new object(), new EventArgs());
                CurrValue = 0.068;
                temp = false;
            }
        }

        private void StartButton_Clicked(object sender, EventArgs e)
        {
            if(!isStarted)
            {
                isStarted = true;
                StartButton.Text = "Стоп";
                CurrMeasurmentData.SendData(state);
            }
            else
            {
                isStarted = false;
                StartButton.Text = "Старт";
                CurrMeasurmentData.SendData("5");
            }
            if (alive == true)
            {
                alive = false;
            }
            else
            {
                alive = true;
                Device.StartTimer(TimeSpan.FromSeconds(1), OnTimerTick);
            }

        }
        DateTime tempTime = new DateTime();
        private bool OnTimerTick()
        {
            timer=timer.AddSeconds(1);
            TimeValue = timer.ToString("T");
            return alive;
        }
        bool sw = true;
        private bool OnTimerTickMeashure()
        {
            if (timerActive)
            {
                timerGo=timerGo.AddSeconds(1);
                if(timerGo.Second==tempTime.Second)
                {
                    
                    if (sw)
                    {
                        tempTime = tempTime.AddSeconds(10);
                        StartButton_Clicked(new object(), new EventArgs());
                        sw = false;
                    }
                    else
                    {
                        sw = true;
                        tempTime=tempTime.AddSeconds(Settings.setPars.timeWakeup);
                        StartButton_Clicked(new object(), new EventArgs());
                    }
                }
                return true;
            }
            else
            return false;
        }
        private void ResetButton_Clicked(object sender, EventArgs e)
        {
            CurrValue = 0.000;
            alive = false;
            MinValue = 999;
            MaxValue = -999;
            TimeValue = "0:00:00";
            timer = new DateTime();
            isStarted = false;
            StartButton.Text = "Старт";
            CurrMeasurmentData.SendData("0");
            Data.MeasurmentData.DataCollection.Clear();
        }
        private async void AlertButton_Clicked(object sender, EventArgs e)
        {
            string r=await DisplayActionSheet("", "Отмена", null, "ВКЛ", "ВЫКЛ");
            if (r == "ВКЛ")
                alertActive = true;
            if (r == "ВЫКЛ")
                alertActive = false;
        }

        private async void TimeButton_Clicked(object sender, EventArgs e)
        {
            string v=await DisplayActionSheet("", "Отмена", null, "ВКЛ", "ВЫКЛ");
            if (v == "ВКЛ")
            {
                timerActive = true;
                timerGo = DateTime.Now.AddSeconds(0);
                tempTime = DateTime.Now.AddSeconds(Settings.setPars.timeWakeup);
                Device.StartTimer(TimeSpan.FromSeconds(1), OnTimerTickMeashure);
            }
            if (v == "ВЫКЛ")
            {
                timerActive = false;
            }
        }



    }
}