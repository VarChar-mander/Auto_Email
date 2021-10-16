using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml.Linq;
using System.Net.Mail;
using System.ComponentModel;
using System.Windows.Forms;
namespace Auto_Email
{
    class Program
    {
        static String toAttach = "";
        static void Main(string[] args)
        {
            try
            {
                if (args.Length > 2)
                    toAttach = args[2]; 
                SendEmail(args[0], args[1]);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            
        }
        private static void SendEmail(String subject, string content)
        {
            MailMessage m = new MailMessage();
            String sender = @ConfigurationManager.AppSettings.Get("fromAddress").ToString();
            String password = @ConfigurationManager.AppSettings.Get("password").ToString();
            m.From = new MailAddress(sender);
            m.To.Add(@ConfigurationManager.AppSettings.Get("toAddress").ToString());
            if (@ConfigurationManager.AppSettings.Get("ccAddress").ToString().Length > 0)
                m.CC.Add(new MailAddress(@ConfigurationManager.AppSettings.Get("ccAddress").ToString()));
            m.Subject = subject;
            m.Body = content;
            if (toAttach.Length > 0)
                try
                {
                    m.Attachments.Add(new Attachment(toAttach));
                }
                catch (Exception e)
                {
                    m.Body += "\n\n\n File failed to attach. Error message: \n" + e.ToString();
                }
            SmtpClient sc = new SmtpClient(@ConfigurationManager.AppSettings.Get("smtpClient").ToString());
            if (@ConfigurationManager.AppSettings.Get("port").ToString().Length > 0)
                sc.Port = System.Convert.ToInt32(@ConfigurationManager.AppSettings.Get("port"));
            if (password.Length > 0)
            {
                sc.Credentials = new System.Net.NetworkCredential(sender, password);
                sc.EnableSsl = true;
            }
            sc.Send(m);
        }
    }
}
