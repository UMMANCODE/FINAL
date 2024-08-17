using Final_Business.Helpers;
using Final_Business.Services.Interfaces;
using Final_Core.Entities;
using Final_Data;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace Final_Business.Services.Implementations;

public class EmailService(IConfiguration configuration, AppDbContext context, UserManager<AppUser> userManager)
  : IEmailService {
  // ReSharper disable once UnusedMember.Local
  public static string Me { get; } = "ummanmemmedov2005@gmail.com";

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
    var now = DateTime.UtcNow;
    var expirationThreshold = now.AddDays(7);

    // Query houses with discounts expiring within the next 7 days
    var houses = context.Houses
      .Include(h => h.HouseImages).Include(house => house.Discounts)
      .Where(h => h.Discounts.Any(d => d.ExpiryDate >= DateTime.Now && d.ExpiryDate <= expirationThreshold))
      .ToList();


    if (houses.Count <= 0) return;
    // Create email content
    const string emailSubject = "Last chance to get these houses discounted!";
    var data = houses.Select(house => new NotificationData {
      HouseName = house.Name,
      OldPrice = house.Price,
      NewPrice = house.Price - house.Discounts.First().Amount,
      Url = new Uri($"{configuration.GetSection("JWT:Audience").Value}api/Houses/user/{house.Id}").ToString()
    }).ToList();

    // Send email
    var subscribers = userManager.Users;
    foreach (var subscriber in subscribers) {
      var emailBody = EmailTemplates.GetDiscountInfoEmail(subscriber.Email!, data);
      Send(subscriber.Email!, emailSubject, emailBody);
    }
    //var emailBody = EmailTemplates.GetDiscountInfoEmail(Me, data);
    //Send(Me, emailSubject, emailBody);
  }
}