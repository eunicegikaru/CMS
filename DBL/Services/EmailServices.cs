using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

public class EmailServices
{
    private readonly IConfiguration _config;

    public EmailServices(IConfiguration config)
    {
        _config = config;

    }

    public void SendOtp(string toEmail, string otp)
    {
        var smtp = _config.GetSection("Smtp");

        var client = new SmtpClient(smtp["Host"], int.Parse(smtp["Port"]))
        {
            Credentials = new NetworkCredential(smtp["Username"], smtp["Password"]),
            EnableSsl = true
        };

        var mail = new MailMessage(
            smtp["Username"],
            toEmail,
            "Your OTP Code",
            $"Your OTP is: {otp}"
        );

        client.Send(mail);
    }
}
