using And.Ecommerce.Core.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace And.ECommerce.UI.WEB.Helpers
{
    public class MailHelper
    {
        static void EmailInfo(User user, out string displayName, out MailAddress from, out string fromPassword, out MailAddress to)
        {
            displayName = "Ecommerce";
            from = new MailAddress("moon.hasanova@gmail.com", displayName);
            fromPassword = "statikamonster";
            to = new MailAddress(user.Email);
        }
        
        public static void Send(User user, string subject, string body)
        {
            EmailInfo(user, out var displayName, out var from, out var fromPassword, out var to);

            try
            {
                using (var message = new MailMessage(from, to))
                {
                    message.Body = body;
                    message.Subject = subject;
                    message.IsBodyHtml = true;

                    try
                    {

                        using (var smtp = new SmtpClient())
                        {
                            smtp.Host = "smtp.gmail.com";
                            smtp.Port = 587;
                            smtp.EnableSsl = true;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential(from.Address, fromPassword);

                            smtp.Send(message);
                        }
                    }
                    finally
                    {
                        message.Dispose();
                    }
                }
            }
            catch (Exception) { }
        }

    }
}