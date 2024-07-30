using Final_Business.Services.Interfaces;
using Final_Data;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace Final_Business.Services.Implementations;

public class EmailService(IConfiguration configuration, AppDbContext context) : IEmailService {
  private const string Me = "ummanmemmedov2005@gmail.com";

  public void Send(string to, string subject, string body) {
    var email = new MimeMessage();

    email.From.Add(MailboxAddress.Parse(configuration.GetSection("EmailSettings:From").Value));
    email.To.Add(MailboxAddress.Parse(to));
    email.Subject = subject;
    email.Body = new TextPart(TextFormat.Html) { Text = body };

    using var smtp = new SmtpClient();
    smtp.Connect(configuration.GetSection("EmailSettings:Provider").Value, Convert.ToInt32(configuration.GetSection("EmailSettings:Port").Value), true);
    smtp.Authenticate(configuration.GetSection("EmailSettings:UserName").Value, configuration.GetSection("EmailSettings:Password").Value);
    smtp.Send(email);
    smtp.Disconnect(true);
  }

  public void SendNotificationEmail() {
    throw new NotImplementedException();
  }
}