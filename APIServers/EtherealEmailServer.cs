using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using IdentityBlazorCoreAPI.Data.Models;

namespace IdentityBlazorCoreAPI.APIServers.Contracts;

public class EtherealEmailServer : IEtherealEmailServer
{
    public void Send(EtherealEmail model)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("ken.armstrong@ethereal.email"));
            email.To.Add(MailboxAddress.Parse("ken.armstrong@ethereal.email"));
            email.Subject = model.Subject;
            // Lấy email của client, lưu vào body như chữ ký Signature
            var formatBody = new TextPart(TextFormat.Html) {Text = model.Body + $"<br/><br/><b>Signature</b>: {model.From}<br/><b>Phone</b>: {model.YourPhone}"};
            email.Body = formatBody;

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("ken.armstrong@ethereal.email", "mTe97TmrB4chhQGSeM");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}