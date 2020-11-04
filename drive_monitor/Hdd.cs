using System;
using System.Collections.Generic;
using System.Text;

namespace drive_monitor
{
    class Hdd
    { 
        public string Letter { get; set; }
        public double Total_size { get; set; }
        public double Free_space { get; set; }
        public double Percent_free { get; set; }

        public Hdd(string l,double ts,double fs)
        {
            Letter = l;
            Total_size = ts;
            Free_space = fs;
            Percent_free = (Free_space / Total_size) * 100;
        }
    }
}
