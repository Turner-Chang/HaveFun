using System.Net;
using System.Net.Mail;

namespace HaveFun.Common
{
    public class SendEmail
    {
        // 收信人的email
        public string emailTo { get; set; }
        // 主旨
        public string subject { get; set; }
        //內容
        public string body { get; set; }

        public SendEmail() { }
        public SendEmail(string emailTo, string subject, string body)
        {
            this.emailTo = emailTo;
            this.subject = subject;
            this.body = body;
        }

        // 設定主機
        string smtpAddress = "smtp.gmail.com";
        // 設定Port
        int portNumber = 587;
        bool enableSSL = true;
        // 設定送信的google帳號密碼
        string emailForm = "tim101dotnet@gmail.com";
        string password = "iiwf mqsi sgla numo";

        public bool Send()
        {
            try
            {
                using (MailMessage message = new MailMessage())
                {
                    // 設定寄信的內容
                    message.From = new MailAddress(emailForm);
                    message.To.Add(emailTo);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    // 使用SmtpClient來寄信，參數為主機位址跟port號碼
                    using (SmtpClient client = new SmtpClient(smtpAddress, portNumber))
                    {
                        client.Credentials = new NetworkCredential(emailForm, password);
                        client.EnableSsl = enableSSL;
                        client.Send(message);
                    }
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
