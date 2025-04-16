using DemoFramewrok.Fixtures.Enum;
using Microsoft.Playwright;
using System;

namespace DemoFramewrok.Fixtures;

public class WebDriverFactory
{
    public static async Task<IBrowser> CreateDriver(string browserType, bool isHeadless, int slowMo)
    {
        WebDriverType webDriverType;
        System.Enum.TryParse(browserType, ignoreCase: true, out webDriverType);

        var driver = webDriverType switch
        {
            WebDriverType.Chrome or WebDriverType.Chromium =>  await InitChromeDriver(webDriverType, isHeadless, slowMo),
            WebDriverType.Edge => await InitEdgeDriver(isHeadless, slowMo),
            WebDriverType.Firefox => await InitFireFoxDriver(isHeadless, slowMo),
            WebDriverType.Safari => await InitWebKitDriver(isHeadless, slowMo),
            _ => await InitChromeDriver(webDriverType, isHeadless, slowMo),
        };
        return driver;
    }

    private static  async Task<IBrowser> InitChromeDriver(WebDriverType webDriverType, bool isHeadless, int slowMo)
    {
        var playwrightDriver = await Playwright.CreateAsync();
        var options = new BrowserTypeLaunchOptions {
            Channel = webDriverType.ToString().ToLower(),
            Headless = isHeadless,
            SlowMo = slowMo
        };
        var browser = await playwrightDriver.Chromium.LaunchAsync(options);

        return browser;
    }
    private static async Task<IBrowser> InitEdgeDriver(bool isHeadless, int slowMo)
    {
        var playwrightDriver = await Playwright.CreateAsync();
        var options = new BrowserTypeLaunchOptions
        {
            Channel = "msedge",
            Headless = isHeadless,
            SlowMo = slowMo
        };
        var browser = await playwrightDriver.Chromium.LaunchAsync(options);

        return browser;
    }
    private static async Task<IBrowser> InitFireFoxDriver(bool isHeadless, int slowMo)
    {
        var playwrightDriver = await Playwright.CreateAsync();
        var options = new BrowserTypeLaunchOptions
        {
            Headless = isHeadless,
            SlowMo = slowMo
        };
        var browser = await playwrightDriver.Firefox.LaunchAsync(options);

        return browser;
    }

    private static async Task<IBrowser> InitWebKitDriver(bool isHeadless, int slowMo)
    {
        var playwrightDriver = await Playwright.CreateAsync();
        var options = new BrowserTypeLaunchOptions
        {
            Headless = isHeadless,
            SlowMo = slowMo
        };
        var browser = await playwrightDriver.Webkit.LaunchAsync(options);
        return browser;
    }

}