using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace drive_monitor
{
   class Report
    {
        public double Threshold { get; set; }
        public List<Hdd> All_drives { get; set; }
        public void get_drives()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach(DriveInfo d in drives)
            {
                if(d.DriveType.ToString() == "Fixed")
                {
                    Hdd hdd = new Hdd(d.Name,d.TotalSize,d.TotalFreeSpace);
                    All_drives.Add(hdd);
                }
            }
        }

        public Report(double _threshold)
        {
            Threshold = _threshold;
            All_drives = new List<Hdd>();
        }

        public void display_report()
        {
            foreach(Hdd d in All_drives)
            {
                Console.WriteLine("Drive name: {0}",d.Letter);
                Console.WriteLine("Drive total size: {0}",d.Total_size);
                Console.WriteLine("Drive free space: {0}",d.Free_space);
                Console.WriteLine("Drive free %: {0}",d.Percent_free.ToString("F2"));
                Console.WriteLine("-----------------------------------------");
            }
        }

        public void email_report(string _email_server,string _sender,string _recipient)
        {
            string computer = Environment.MachineName;
            string email_server = _email_server;
            string sender = _sender;
            string recipient = _recipient;
            string body = "";

            body += computer + "\n\n-----------------------------------------\n\n";
            foreach(Hdd d in All_drives)
            {
                body += "Drive name: " + d.Letter + "\n"
                        + "Drive total size: " + d.Total_size + "\n" 
                        + "Drive free space: " + d.Free_space + "\n"
                        + "Drive free %: " + d.Percent_free.ToString("F2") + "\n\n"
                        +"-----------------------------------------\n\n";
            }

            string subject = computer + " DISK USAGE REPORT";
            Email msg = new Email();
            msg.send_email(email_server,sender,recipient,body,subject);
        }

        public bool email_alert(string _email_server,string _sender,string _recipient)
        {
            bool has_alert = false;
            string computer = Environment.MachineName;
            string email_server = _email_server;
            string sender = _sender;
            string recipient = _recipient;
            string body = "";
            foreach(Hdd d in All_drives)
            {
                
                if(d.Percent_free <= Threshold)
                {
                    has_alert = true;

                    body += "Drive " + d.Letter 
                                + " on computer " + computer 
                                + " has only " + d.Percent_free.ToString("F2") 
                                + "% free disk space.\n\n"
                                +"-----------------------------------------\n\n";
                }
            }

            body += "\n\n!!!Drive Monitor needs to be reset!!!\n" +
                "To reset Drive Monitor logon to the local machine and run:\n" +
                "drive_monitor.exe --reset\n" +
                "To check Drive Monitor's status run:" +
                "drive_monitor.exe --config";

            if(has_alert)
            { 
                string subject = "WARNING: LOW DISK SPACE ON " + computer;
                Email msg = new Email();
                msg.send_email(email_server,sender,recipient,body,subject);
            }

            return has_alert; 
        }
    }
}
