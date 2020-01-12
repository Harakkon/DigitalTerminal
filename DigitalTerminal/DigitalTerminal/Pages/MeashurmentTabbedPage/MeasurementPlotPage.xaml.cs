using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigitalTerminal.Pages.MeashurmentTabbedPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MeasurementPlotPage : ContentPage
	{
        
        public MeasurementPlotPage ()
		{
			InitializeComponent ();
            OxyPlotView.Model = CreatePlotModel();
            OxyPlotView.HorizontalOptions = LayoutOptions.Fill;
            OxyPlotView.VerticalOptions = LayoutOptions.Fill;
            var Controller = new PlotController();
        }
        static PlotModel plotModel;
        static LineSeries series1;
        static double max = 0;
        public static void AddDataToPlot(double x,double y)
        {
            series1.Points.Add(new DataPoint(x, y));
            plotModel.InvalidatePlot(true);
            double panStep = series1.XAxis.Transform(-1 + series1.XAxis.Offset);
            series1.XAxis.Pan(panStep);
            series1.XAxis.AbsoluteMaximum++;
            if (y > max)
            {
                max = y; series1.YAxis.AbsoluteMaximum = max + 10; series1.YAxis.Maximum = max + 10; series1.YAxis.Reset();
            }
        }
        static void Reset()
        {
            series1.Points.Clear();
            for (int i = 0; i <= 10; i++) { series1.Points.Add(new DataPoint(i, 0)); }
        }
        static PlotModel CreatePlotModel()
        {
            plotModel = new PlotModel {};
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, IsPanEnabled = true, IsZoomEnabled = true });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Maximum = 20, Minimum = 0, IsPanEnabled = true, IsZoomEnabled = true });

            series1 = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White
            };

            Reset();

            plotModel.Series.Add(series1);

            return plotModel;
        }   
    }
}