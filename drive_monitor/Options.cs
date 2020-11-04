using System;
using System.Collections.Generic;
using System.Text;

using Mono.Options;

namespace drive_monitor
{
    class Options
    {
        public Options(string[] args)
        {
            Config config = new Config();
            bool show_help = false;
            bool console_only = false;
            bool email_alert = false;
            bool email_report = false;
            bool show_config = false;
            bool reset_alert = false;

            config.read_config();

            var options = new OptionSet()
                {
                    "Usage: drive_monitor [OPTIONS]+",
                    "Reports the remaining disk space of the local hosts fixed drives.",
                    "",
                    "Options:",
                    {
                        "server=","FQDN or IP address of the email server",
                        v => config.Email_server = v
                    },
                    {
                        "sender=","the {EMAIL} of sender",
                        v => config.Sender = v
                    },
                    {
                        "recipient=","the {EMAIL} of recipient",
                        v => config.Recipient = v
                    },
                    {
                        "threshold=","the amount of free space left (in {GB}) before a email alert is sent.",
                        v => config.Threshold = Convert.ToDouble(v)
                    },
                    {
                        "a|alert", "checks if a drive has crossed the alert threshold and sends an email to recipient. no email"
                            + " is sent if alert has been previously tripped.",
                        v => email_alert = v != null
                    },
                    {
                        "c|console", "displays the report only on the console; does not send an email",
                        v => console_only = v != null
                    },
                    {
                        "e|email", "emails the report to recipient",
                        v => email_report = v != null
                    },
                    {
                        "config", "displays drive_monitor.exe current configuration",
                        v => show_config = v != null
                    },
                    {
                        "reset", "resets drive_monitor.exe alert status",
                        v => reset_alert = v != null
                    },
                    {
                        "h|help", "show this message and exit",
                        v => show_help = v != null
                    },
                };

            List<string> extra;
            try
            {
                extra = options.Parse(args);

                if(!show_help)
                {
                    config.write_config();
                }
            }
            catch(OptionException e)
            {
                Console.WriteLine("Try `drive_monitor --help' for more information.");
            }

            if(show_help || args.Length == 0)
            {
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            Report report = new Report(config.Threshold);
            report.get_drives();

            if(console_only)
            {
                report.display_report();
            }

            if(email_report)
            {
                report.email_report(config.Email_server,config.Sender,config.Recipient);
            }

            if(email_alert && !config.Alert_tripped)
            {
                if(report.email_alert(config.Email_server,config.Sender,config.Recipient))
                {
                    config.alert_triggered();
                }
            }

            if(show_config)
            {
                Console.WriteLine(config.ToString());
            }

            if(reset_alert)
            {
                config.reset_alert();
            }
        }
    }
}
