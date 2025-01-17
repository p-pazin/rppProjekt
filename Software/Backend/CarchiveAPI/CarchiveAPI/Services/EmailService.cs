using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(emailSettings["SenderName"], emailSettings["SenderEmail"]));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["Port"]), false);
        await smtp.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["Password"]);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
