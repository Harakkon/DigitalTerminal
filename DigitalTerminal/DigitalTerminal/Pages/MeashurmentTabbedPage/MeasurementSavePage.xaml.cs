using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigitalTerminal.Pages.MeashurmentTabbedPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MeasurementSavePage : ContentPage
	{
        string filePath, documentsPath;
        public MeasurementSavePage ()
		{
			InitializeComponent ();
            listViewData.ItemsSource = Data.MeasurmentData.DataCollection;
            documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            filePath = Path.Combine(documentsPath, "1.xml");
            ToolbarItem R = new ToolbarItem();
            ToolbarItems.Add(new ToolbarItem("S", "", () =>
            {
                S_Clicked();
            }));
        }
        private void S_Clicked()
        {
            CreateXML();
            CreateAsync();
        }

        public async void CreateAsync()
        {
            //await _client.Mkcol("mydir111111");
            string DataTimeNow= DateTime.Now.ToString();
            await Settings._client.PutFile("Measurement Data/"+DataTimeNow+".xml", File.OpenRead(filePath));
        }
        public void CreateXML()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.CloseOutput = false;           
            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                // writer.WriteStartDocument();
                writer.WriteStartElement("bookRecords");

                foreach (var c in Data.MeasurmentData.DataCollection)
                {
                    writer.WriteStartElement("record");
                    writer.WriteElementString("Data", c.value.ToString());
                    writer.WriteElementString("Time", c.time.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                //  writer.WriteEndDocument();
                writer.Flush();
            }
        }
    }
}