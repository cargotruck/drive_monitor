using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;


namespace drive_monitor
{
    class Email
    {
        public void send_email(string _email_server,string sender,string recipient,string message,string subject = "")
        {
            string email_server = _email_server;
            MailMessage email = new MailMessage(sender,recipient);
            email.Subject = subject;
            email.Body = message;
            if(!String.IsNullOrEmpty(subject)) email.Subject = subject;
            SmtpClient client = new SmtpClient(email_server);
            client.UseDefaultCredentials = true;

            try
            {
                client.Send(email);
            }catch(Exception e){
                Console.WriteLine("Error sending email: " + e.Message);
            }
        }
    }
}
