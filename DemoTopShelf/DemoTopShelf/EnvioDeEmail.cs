using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace DemoTopShelf
{
    public class EnvioDeEmail : IDisposable
    {
        private readonly SmtpClient _smtpClient;
        private readonly MailMessage _message;

        public EnvioDeEmail(string sectionName = "default")
        {
            _smtpClient = new SmtpClient();
            CarregarSMTP(sectionName);
            _message = new MailMessage();
        }

        private void CarregarSMTP(string sectionName)
        {
            var section = ConfigurationManager.GetSection("mailSettings/" + sectionName) as SmtpSection;

            if (section == null)
                throw new System.Exception("Configurações de email não carregadas");


            if (section.Network == null)
                throw new System.Exception("Servidor de Email não configurado");


            _smtpClient.Host = section.Network.Host;
            _smtpClient.Port = section.Network.Port;
            _smtpClient.UseDefaultCredentials = section.Network.DefaultCredentials;

            _smtpClient.Credentials = new NetworkCredential(section.Network.UserName, section.Network.Password, section.Network.ClientDomain);
            _smtpClient.EnableSsl = section.Network.EnableSsl;

            _smtpClient.DeliveryMethod = section.DeliveryMethod;
            _smtpClient.Timeout = 60 * 1000;
        }


        public async void EnviarMensagemAsync(IList<string> destinatarios)
        {
            _message.IsBodyHtml = true;
            _message.From = (new MailAddress("notificacao.sis@gmail.com"));
            _message.Subject = $"Teste {DateTime.Now}";
            _message.Body = "<p>Testando! :)</p>";

            foreach (var dest in destinatarios)
            {
                _message.To.Clear();
                try
                {
                    if (EmailEhValido(dest))
                    {
                        _message.To.Add(new MailAddress(dest));
                        await _smtpClient.SendMailAsync(_message);
                    }
                    
                }
                catch
                {
                    //log here
                    return;
                }

            }
        }

        private bool EmailEhValido(string email)
        {
            return
                 Regex.IsMatch(
                     email,
                     @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                     RegexOptions.IgnoreCase);
        }

        public void Dispose()
        {
            _smtpClient.Dispose();
        }
    }
}