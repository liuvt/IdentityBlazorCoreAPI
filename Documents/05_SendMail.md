<h1>IdentityBlazorCoreAPI v1 👋</h1>

- Create account and get information email
- Add package: Mailkit
- API: Create Service/Interface, create model, create controller, register service
- UI: create component send mail

<h3>1. Create account and get information email</h3>

- Create account in: "https://ethereal.email/"
- Save information in file: emailconfig.json
```json
    {
    "Web-Site": "https://ethereal.email/",
    "AccountCredentials": {
        "Name": "Ken Armstrong",
        "Username": "ken.armstrong@ethereal.email",
        "Password": "mTe97TmrB4chhQGSeM"
    },
    "SMTPConfiguration": {
        "Host": "smtp.ethereal.email",
        "Port": "587",
        "Security": "STARTTLS",
        "Username": "ken.armstrong@ethereal.email",
        "Password": "mTe97TmrB4chhQGSeM"
    },
    "IMAPConfiguration": {
        "Host": "imap.ethereal.email",
        "Port": "993",
        "Security": "TLS",
        "Username": "ken.armstrong@ethereal.email",
        "Password": "mTe97TmrB4chhQGSeM"
    },
    "POP3 configuration": {
        "Host": "pop3.ethereal.email",
        "Port": "995",
        "Security": "TLS",
        "Username": "ken.armstrong@ethereal.email",
        "Password": "mTe97TmrB4chhQGSeM"
    }
}
```

<h3>2. Add package: MailKit</h3>

- CMD: _dotnet add package MailKit_
```cs
    <PackageReference Include="MailKit" Version="4.7.1.1" />
```

<h3>3. API</h3>

<h4>Create Service/Interface</h4>

- IEtherealEmailServer.cs
- EtherealEmailServer.cs
```cs
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
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
            email.Body = new TextPart(TextFormat.Html) {Text = model.Body};

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
```

<h4>Create Model</h4>

```cs
public class EtherealEmail
{
    public string From { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
```

<h4>Register Service/Interface</h4>

- In Program.cs class
```c#
builder.Services.AddScoped<IEtherealEmailServer, EtherealEmailServer>();
```

<h3>4. UI</h3>