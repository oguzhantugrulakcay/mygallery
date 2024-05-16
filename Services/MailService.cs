
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using mygallery.Context;
using mygallery.Data;
using Serilog.Core;

public interface IMailService{
    Task SendBuyRequestMail(BuyRequest request);
}

public class MailService:IMailService
{
    private readonly Logger _logger;
    private readonly AppConfig _appConfig;

    private readonly AppConfig.SmtpSetting smtpSetting;

    public MailService(Logger logger, IOptions<AppConfig> appConfig){
        _logger = logger;
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
    public async Task SendBuyRequestMail(BuyRequest request){
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(smtpSetting.SenderName,smtpSetting.SenderEmail));
        emailMessage.To.Add(new MailboxAddress("","ozgur@libertycars.com.tr"));
        emailMessage.Subject="Satın Alma Teklifi";
        var body=$"Merhaba<br/> Yeni satın alma isteği mevcut. Araç bilgileri:<br/>";
        var bodyBuilder=new BodyBuilder{HtmlBody=body};
        emailMessage.Body=bodyBuilder.ToMessageBody();
        await SendMail(emailMessage);
       
    }
}