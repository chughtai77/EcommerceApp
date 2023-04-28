using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Threading.Tasks;
public class EmailSender : IEmailSender
{
    private readonly IConfiguration _config;

    public EmailSender(IConfiguration config)
    {
        _config = config;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailToSend = new MimeMessage();
        emailToSend.From.Add(MailboxAddress.Parse("imran.chughtai77@gmail.com"));
        emailToSend.To.Add(MailboxAddress.Parse(email));
        emailToSend.Subject = subject;
        emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

        //send the email
        using (var emailClient = new SmtpClient())
        {
            emailClient.Connect("smtp.gmail.com",587,MailKit.Security.SecureSocketOptions.StartTls);
            emailClient.Authenticate("ctechh77@gmail.com", "nzzwsxawxcnvfdto");
            emailClient.Send(emailToSend);
            emailClient.Disconnect(true);
        }
        return Task.CompletedTask;
    }








    //public void Send(MailMessage message)
    //{
    //    throw new NotImplementedException();
    //}

    //public void Send(IEnumerable<MailMessage> messages)
    //{
    //    throw new NotImplementedException();
    //}



    //public async Task SendEmailAsync(string email, string subject, string message)
    //{
    //    var emailMessage = new MimeMessage();

    //    emailMessage.From.Add(new MailboxAddress(_config["EmailSettings:SenderName"], _config["EmailSettings:SenderEmail"]));
    //    emailMessage.To.Add(new MailboxAddress("", email));
    //    emailMessage.Subject = subject;
    //    emailMessage.Body = new TextPart("html") { Text = message };

    //    using (var client = new SmtpClient())
    //    {
    //        await client.ConnectAsync(_config["EmailSettings:SmtpServer"], int.Parse(_config["EmailSettings:SmtpPort"]), SecureSocketOptions.StartTls);
    //        await client.AuthenticateAsync(_config["EmailSettings:SmtpUsername"], _config["EmailSettings:SmtpPassword"]);
    //        await client.SendAsync(emailMessage);
    //        await client.DisconnectAsync(true);
    //    }
}

