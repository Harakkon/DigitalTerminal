using System;

namespace DigitalTerminal.Data
{
    class DataStorage
    {
        public double value { get; set; }
        public int number { get; set; }
        public string time { get; set; }
        public string type { get; set; }
        public DataStorage(double value,int number,string time,string type)
        {
            this.value = value;
            this.number = number;
            this.time = time;
            this.type = type;
        }
    }
}
