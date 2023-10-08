using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    internal class SmtpImap
    {
        private string _email;
        private string _password;

        private string _subject;
        private string _body;

        public SmtpImap(string email, string passwd, string subject, string body)
        {
            this._email = email;
            this._password = passwd;
            this._subject = subject;
            this._body = body;
        }

        public void SMTP()
        {
            var client = new SmtpClient("smtp.mail.ru", 587);
            client.EnableSsl = true;

            client.Credentials = new NetworkCredential(this._email, this._password);
            var message = new MailMessage
            {
                Subject = this._subject,
                Body = this._body,
            };

            message.From = new MailAddress(this._email, "Step computer Academy");
            message.To.Add(new MailAddress("natig_aliyev@itstep.org"));
            client.Send(message);
        }

        public void IMAP()
        {
            var imapClient = new ImapClient();
            imapClient.Connect("imap.mail.ru", 993, true);
            imapClient.Authenticate(this._email, this._password);
        }
    }
}
