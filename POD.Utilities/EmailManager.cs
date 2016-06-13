using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace POD.Utilities
{
    public static class EmailManager
    {
       // public static Task<bool> SendingTask;
        //public static void SendCompleted(object sender, AsyncCompletedEventArgs e)
        //{

        //    SendingTask.AsyncState =
        //}
        public static async Task<bool> SendEmailAsync(String Receiver, String Subject, String Body, String[] Attachments = null)
        {
            int smtpPort;
            string smtpHost = PODEnvironment.GetSetting("stmpServer");
            string smtpUsername = PODEnvironment.GetSetting("emailService:Login");
            string smtpPassword = PODEnvironment.GetSetting("emailService:Password");
            string fromDisplayName = PODEnvironment.GetSetting("emailService:DisplayName");
            if (!Int32.TryParse(PODEnvironment.GetSetting("emailService:From"), out smtpPort))
            {
                smtpPort = 587;
            }
            string From = PODEnvironment.GetSetting("emailService:From");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(From, fromDisplayName);
            mailMessage.To.Add(Receiver);
            mailMessage.Subject = Subject;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = true;
            if (Attachments != null)
            {
                foreach (String AttachmentPath in Attachments)
                {
                    if (File.Exists(AttachmentPath))
                    {
                        mailMessage.Attachments.Add(new Attachment(AttachmentPath));
                    }
                }
            }         
            SmtpClient emailClient = new SmtpClient(smtpHost, smtpPort);
            emailClient.EnableSsl = true;
            if (!String.IsNullOrEmpty(smtpUsername) && !String.IsNullOrEmpty(smtpPassword))
            {
                emailClient.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
            }

            TaskCompletionSource<bool> TaskCompletionSource = new TaskCompletionSource<bool>();      

            Task<bool> SendingTask = TaskCompletionSource.Task;

            emailClient.SendCompleted += (sender, e) => 
            {
                if (e.Error != null)
                {
                    TaskCompletionSource.SetException(new Exception("Can`t send email"));
                }
                if (e.Cancelled)
                {
                    TaskCompletionSource.SetCanceled();
                }
                else
                {
                    TaskCompletionSource.SetResult(true);
                }
            };

            String Token = "";
            emailClient.SendAsync(mailMessage, Token);
            return await SendingTask;
        }

        public static String GetTemplatePath(String TemplateName)
        {
            if (PODEnvironment.GetSetting("emailTemplate:" + TemplateName) != null)
            {
                string uriPath =  System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\Templates\\" + PODEnvironment.GetSetting("emailTemplate:" + TemplateName);
                return new Uri(uriPath).LocalPath;
            }
            else
            {
                return null;
            }

        }

        //public static  void SendEmail(String Receiver, String Subject, String TemplateName, object[] Data)
        //{
        //   Task task =  SendEmailFillBodyAsync(Receiver, Subject, TemplateName, Data);
        //}

        public static string FillBody(string TemplateName, object[] Data)
        {
            String TemplatePath = GetTemplatePath(TemplateName);
            String Body = "";
            if (!String.IsNullOrWhiteSpace(TemplatePath))
            {
                
                using (var sr = new StreamReader(TemplatePath))
                {
                    Body = sr.ReadToEnd();
                }
                Body = String.Format(Body, Data);
            }

            return Body;
        }

        public static async Task<bool> SendEmailFillBodyAsync(String Receiver, String Subject, String TemplateName, object[] Data, String[] Attachments = null)
        {
            string body = FillBody(TemplateName, Data);
            //String TemplatePath = GetTemplatePath(TemplateName);
            //if (!String.IsNullOrWhiteSpace(TemplatePath)) {                
            //    String Body = "";
            //    using (var sr = new StreamReader(TemplatePath))
            //    {
            //        Body = sr.ReadToEnd();
            //    }
            //    Body = String.Format(Body, Data);
                return await SendEmailAsync(Receiver, Subject, body, Attachments);
           
           // return false;
          
        }
    }
}
