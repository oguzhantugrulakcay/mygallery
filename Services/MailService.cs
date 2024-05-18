
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using mygallery.Data;

public interface IMailService{
    Task SendBuyRequestMail(string ModelName,string BrandName,int Year);
}

public class MailService:IMailService
{
    private readonly AppConfig _appConfig;

    private readonly AppConfig.SmtpSetting smtpSetting;

    public MailService( IOptions<AppConfig> appConfig){
        _appConfig = appConfig.Value;
        smtpSetting = appConfig.Value.SmtpSettings;
    }

    public async Task SendMail(MimeMessage emailMessage){
        using (var client = new SmtpClient()){
                    await client.ConnectAsync(smtpSetting.Server, smtpSetting.Port,MailKit.Security.SecureSocketOptions.StartTls);

                    await client.AuthenticateAsync(smtpSetting.UserName, smtpSetting.Password);

                    await client.SendAsync(emailMessage);

                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                }
    }
//TODO: test send mail
    public async Task SendBuyRequestMail(string ModelName,string BrandName,int Year){
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(smtpSetting.SenderName,smtpSetting.SenderEmail));
        emailMessage.To.Add(new MailboxAddress("","ozgur@libertycars.com.tr"));
        emailMessage.Subject="Satın Alma Teklifi";
        var templatePath = Path.Combine(AppContext.BaseDirectory, "templates", "SendBuyRequestMail.html");
        var templateContent = await File.ReadAllTextAsync(templatePath);

        var body = templateContent.Replace("@@Brand", BrandName)
                                  .Replace("@@Model", ModelName)
                                  .Replace("@@Year", Year.ToString());

        var bodyBuilder=new BodyBuilder{HtmlBody=body};
        emailMessage.Body=bodyBuilder.ToMessageBody();
        await SendMail(emailMessage);
       
    }
}