namespace CityInfo.API.Services;

public class CloudMailService : IMailService
{
    private readonly string _mailTo = "cloud@provisionpay.com";
    private readonly string _mailFrom = "cloud@provisionpay.com";

    public CloudMailService(IConfiguration configuration)
    {
        _mailTo = configuration["MailSettings:mailToAddress"];
        _mailFrom = configuration["MailSettings:mailFromAddress"];
    }

    public void Send(string subject, string message)
    {
        Console.WriteLine($"Mail from{_mailFrom} to {_mailTo}, with  {nameof(CloudMailService)}");
        Console.WriteLine($"Subject :{subject}");
        Console.WriteLine($"Message :{message}");
    }
}