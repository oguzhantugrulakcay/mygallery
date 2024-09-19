namespace mygallery.Data;

public class AppConfig
{
    public AppConfig(){
        SmtpSettings=new SmtpSetting();
    }
    public string CompanyName { get; set; } ="";
    public string BaseUrl { get; set; } ="";
    public string SecretKey { get; set; }
    public string ConnectionName { get; set; }="";
    public class SmtpSetting{
        public string Server { get; set; }="";
        public int Port { get; set;}
        public string SenderName { get; set; }="";
        public string SenderEmail { get; set; }="";
        public string UserName { get; set; }="";
        public string Password { get; set; }="";
    }
    public SmtpSetting SmtpSettings { get; set; }
}