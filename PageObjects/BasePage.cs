using DemoFramewrok.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

namespace DemoFramewrok.PageObjects;

public class BasePage
{
    public string PageUrl { get; }
    public IPage Page { get; set; }
    public IBrowser Browser { get; set; }
    public IConfiguration Config { get; set; }
    public IBrowserContext BrowserContext { get; set; }

    public async Task Navigate()
    {
        Console.WriteLine("****** Navigation method here");
        await Page.GotoAsync(ApplicationOptions.GetConfig(Config).BaseUrl);
        Console.WriteLine("****** Navigation method - Navigated to base url");
    }
    public async Task NavigateTo(string pageName)
    {
        await Page.RunAndWaitForNavigationAsync(async () =>
        {
            await Page.GotoAsync(ApplicationOptions.GetConfig(Config).BaseUrl + "/" + pageName);
        });
    }

}
    
