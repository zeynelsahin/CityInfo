namespace CityInfo.API.Services;

public class LocalMailService
{
    private readonly string _mailTo = "zeynelsahin@provisionpay.com";
    private readonly string _mailFrom = "deneme@provisionpay.com";

    internal void Send(string subject, string message)
    {
        Console.WriteLine($"Mail from{_mailFrom} to {_mailTo}, with {nameof(LocalMailService)}");
        Console.WriteLine($"Subject :{subject}");
        Console.WriteLine($"Message :{message}");
    }
    
}