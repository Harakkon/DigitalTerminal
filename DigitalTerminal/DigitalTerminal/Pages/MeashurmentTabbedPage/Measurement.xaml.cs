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
	public partial class Measurement : TabbedPage
	{
		public Measurement ()
		{
			InitializeComponent ();            
        }
	}
}