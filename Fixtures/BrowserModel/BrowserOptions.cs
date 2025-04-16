using Microsoft.Extensions.Configuration;

namespace DemoFramewrok.Fixtures.BrowserModel;

public class BrowserOptions
{
    public const string SectionName = "BrowserOptions";
    public string BrowserType { get; set; }
    public bool Headless { get; set; }
    public int SlowMo { get; set; }

    public static BrowserOptions GetConfig(IConfiguration configuration)
    {
        var browserOptions = new BrowserOptions();
        configuration.GetSection(BrowserOptions.SectionName).Bind(browserOptions);
        return browserOptions;
    }
}
    
