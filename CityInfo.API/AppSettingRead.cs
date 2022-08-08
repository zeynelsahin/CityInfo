namespace CityInfo.API;

public class AppSettingRead
{
    private string _toAdress;
    private string _fromAdress;
    public AppSettingRead(IConfiguration configuration)
    {
        _toAdress = configuration["MailSettings:toAdress"];
         _fromAdress = configuration["MailSettings:fromAdress"];
    }
}