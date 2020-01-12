using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalTerminal.Data
{
    public class SettingParameters
    {
        public string webLogin;
        public string webPass;
        public string UUIDs;
        public string UUIDc;
        public int timeBeep;
        public int timeWakeup;
        public string webDavAdress;
        public SettingParameters(string webLogin,string webPass,string UUIDs,string UUIDc,int timeBeep, int timeWakeup, string webDavAdress)
        {
            this.webLogin = webLogin;
            this.webPass = webPass;
            this.UUIDs = UUIDs;
            this.UUIDc = UUIDc;
            this.timeBeep = timeBeep;
            this.timeWakeup = timeWakeup;
            this.webDavAdress = webDavAdress;
        }

        
    }
}
