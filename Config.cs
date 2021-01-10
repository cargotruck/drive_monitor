using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace drive_monitor
{
    class Config
    {
        private string config_path = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\options.config";

        private string email_server = "mail.yourdomain.com";
        public string Email_server
        {
            get
            { 
                return email_server;
            }
            set
            {
                if(!String.IsNullOrEmpty(value))
                {
                    email_server = value;
                }
            }
        }

        private string sender = "alerts@yourdomain.com";
        public string Sender
        {
            get
            { 
                return sender;
            }
            set
            {
                if(!String.IsNullOrEmpty(value))
                {
                    sender = value;
                }
            }
        }

        private string recipient = "itadmin@yourdomain.com";
        public string Recipient
        {
            get
            { 
                return recipient;
            }
            set
            {
                if(!String.IsNullOrEmpty(value))
                {
                    recipient = value;
                }
            }
        }

        private double threshold = 20;
        public double Threshold
        {
            get
            { 
                return threshold;
            }
            set
            {
                if(value > 0)
                {
                    threshold = value;
                }
            }
        }

        public bool Alert_tripped { get; set; }

        public void reset_alert()
        {
            Alert_tripped = false;
            write_config();
        }

        public void alert_triggered()
        {
            Alert_tripped = true;
            write_config();
        }

        public void read_config()
        {
            if(File.Exists(config_path))
            {
                using(StreamReader sr = new StreamReader(config_path))
                {
                    string line = "";
                    while((line = sr.ReadLine()) != null)
                    {
                        string[] val = line.Split();
                        if(val[0] == "email_server:")
                        {
                            Email_server = val[1];
                        }

                        if(val[0] == "sender:")
                        {
                            Sender = val[1];
                        }

                        if(val[0] == "recipient:")
                        {
                            Recipient = val[1];
                        }
                        
                        if(val[0] == "threshold:")
                        {
                            Threshold = Convert.ToDouble(val[1]);
                        }

                        if(val[0] == "alert_tripped:")
                        {
                            Alert_tripped = Convert.ToBoolean(val[1]);
                        }
                    }
                    sr.Close();
                }
            }
        }

        public void write_config()
        {
            StreamWriter sw = new StreamWriter(config_path);
            sw.WriteLine(ToString());
            sw.Close();
        }

        public override string ToString()
        {
            return "email_server: " + email_server + "\n" 
                    + "sender: " + sender + "\n"
                    + "recipient: " + recipient + "\n"
                    + "threshold: " + threshold + "\n"
                    + "alert_tripped: " + Alert_tripped;
        }
    }
}
