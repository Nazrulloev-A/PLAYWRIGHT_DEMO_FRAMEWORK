using Microsoft.Extensions.Configuration;

namespace DemoFramewrok.Models;

public  class ApplicationOptions
{
    public const string Application = "Test";

    public string BaseUrl { get; set; }
    public string BrowserType { get; set; }
    public bool Headless { get; set; }
    public int SlowMo { get; set; }
    public ApplicationUser StaffUser { get; set; }
    
    public static ApplicationOptions GetConfig(IConfiguration configuration)
    {
        var appOptions = new ApplicationOptions();
        configuration.GetSection(Application).Bind(appOptions);

        return appOptions;
    }
}
