﻿using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using RestSharp.Contrib;
using SendGrid;

namespace Imarley.Controle.Security.Identity
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            //return ConfigSendGridasync(message);
            return SendMail(message);
        }

        // Implementação do SendGrid
        private Task ConfigSendGridasync(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new MailAddress(ConfigurationManager.AppSettings["ContaDeEmail"], ConfigurationManager.AppSettings["SenhaEmail"]);
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential(ConfigurationManager.AppSettings["ContaDeEmail"], ConfigurationManager.AppSettings["SenhaEmail"]);

            // Criar um transport web para envio de e-mail
            var transportWeb = new Web(credentials);

            // Enviar o e-mail
            if (transportWeb != null)
            {
                var x = transportWeb.DeliverAsync(myMessage);
                return x;
            }
            else
            {
                return Task.FromResult(0);
            }
        }

        // Implementação de e-mail manual
        private Task SendMail(IdentityMessage message)
        {
            if (ConfigurationManager.AppSettings["Internet"] == "true")
            {
                var text = HttpUtility.HtmlEncode(message.Body);

                var msg = new MailMessage();
                msg.From = new MailAddress(ConfigurationManager.AppSettings["ContaDeEmail"], "Admin do Portal");
                msg.To.Add(new MailAddress(message.Destination));
                msg.Subject = message.Subject;
                msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Html));

                var smtpClient = new SmtpClient("smtp.rotamaxima.com.br", Convert.ToInt32(587));
                var credentials = new NetworkCredential(ConfigurationManager.AppSettings["ContaDeEmail"], ConfigurationManager.AppSettings["SenhaEmail"]);
                smtpClient.Credentials = credentials;
                smtpClient.EnableSsl = true;
                smtpClient.Send(msg);
            }

            return Task.FromResult(0);
        }
    }
}
