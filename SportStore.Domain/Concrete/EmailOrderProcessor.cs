using SportStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportStore.Domain.Entities;
using System.Net.Mail;
using System.Net;

namespace SportStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "tongtng@qq.com";
        public string MailFromAdress = "1585358813@qq.com";
        public bool UseSsl = true;
        public string UserName = "name";
        public string Password = "ps";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = "@E:";
    }
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;
        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }
        public void ProcessOrder(Cart cart, ShoppingDetails shoppingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.UserName, emailSettings.Password);
                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }
                StringBuilder body = new StringBuilder().AppendLine("A new order has been submitted")
                    .AppendLine("---").AppendLine("Items:");
                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0}x{1}(subtotal:{2:c})",line.Quantity,line.Product.Name,subtotal);
                }
                body.AppendFormat("Total order value:{2:c}", cart.ComputeToVaule())
                    .AppendLine("---")
                    .AppendLine("Ship to:")
                    .AppendLine(shoppingDetails.Name).AppendLine(shoppingDetails.Line1)
                    .AppendLine(shoppingDetails.Line2 ?? "")
                    .AppendLine(shoppingDetails.Line3 ?? "")
                    .AppendLine(shoppingDetails.State)
                    .AppendLine(shoppingDetails.Country).AppendLine(shoppingDetails.Zip)
                    .AppendLine("---")
                    .AppendFormat("Gift warp:{0}", shoppingDetails.GiftWrap ? "Yes" : "No");
                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAdress,
                    emailSettings.MailToAddress,
                    "New order submit",body.ToString());
                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smtpClient.Send(mailMessage);
            }
        }
    }
}
