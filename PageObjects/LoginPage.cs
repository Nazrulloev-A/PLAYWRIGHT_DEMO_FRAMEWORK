using DemoFramewrok.Enum;
using DemoFramewrok.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

namespace DemoFramewrok.PageObjects;

public class LoginPage
{
    private readonly BasePage _basePage;
    private readonly IPage _page;
    private readonly IConfiguration _config;

    public LoginPage(IPage page, IConfiguration config)
    {
        _page = page;
        _config = config;

    }
    
    private ILocator OrigUserNameBox => _page.Locator("//input[@id='ctl00_MainContent_username']");
    private ILocator OrigPasswordBox => _page.Locator("//input[@id='ctl00_MainContent_password']");
    private ILocator OrigSignInBtn => _page.Locator("button[@id='ctl00_MainContent_login_button']");
    
    private ILocator UserName => _page.Locator("id=username");
    private ILocator Password => _page.Locator("id=password");
    private ILocator Continue => _page.Locator("button[name=\"action\"]");
    private ILocator TermsAndConditionHeader => _page.Locator("//div[@class='panel-heading text-center']");
    private ILocator Accept => _page.Locator("id=accepted");
    private ILocator ContinueBtn => _page.Locator("//button[@type='submit'][@data-action-button-primary='true']");
    private ILocator NavBarList => _page.Locator("//ul[@class='nav navbar-nav']/li");
    private ILocator MessagesMenu => _page.Locator("//a[@id='messengerMainBtn']");
    private ILocator GlobalSearch => _page.Locator("//input[@id='term' and @type='search']");
    private ILocator PerDieamJobGrid => _page.Locator("//div[@id='grid-container']']");
    
    
    public async Task UserLogin(string userRole)
    {
        string userName = null;
        string password = null;
        UserRoles userRoles;
        System.Enum.TryParse(userRole, ignoreCase: true, out userRoles);
        switch (userRoles)
        {
            case UserRoles.Staff:
                userName = ApplicationOptions.GetConfig(_config).StaffUser.Username;
                password = ApplicationOptions.GetConfig(_config).StaffUser.Password;
                break;
            case UserRoles.Agency:
                userName = ApplicationOptions.GetConfig(_config).AgencyUser.Username;
                password = ApplicationOptions.GetConfig(_config).AgencyUser.Password;
                break;
            case UserRoles.Client:
                userName = ApplicationOptions.GetConfig(_config).ClientUser.Username;
                password = ApplicationOptions.GetConfig(_config).ClientUser.Password;
                break;
            case UserRoles.Client1:
                userName = ApplicationOptions.GetConfig(_config).ClientUser1.Username;
                password = ApplicationOptions.GetConfig(_config).ClientUser1.Password;
                break;
            case UserRoles.ClientFacilityTestUser:
                userName = ApplicationOptions.GetConfig(_config).PerDiemClientFacilityUser.Username;
                password = ApplicationOptions.GetConfig(_config).PerDiemClientFacilityUser.Password;
                break;
        }

        await Login(userName, password);
    }

    public async Task UserLoginAsApiUser()
    {
        var userName = ApplicationOptions.GetConfig(_config).ApiUser.Username;
        var password = ApplicationOptions.GetConfig(_config).ApiUser.Password;

        await Login(userName, password);
    }

    public async Task VerifyOnLoginPage()
    {
        Assert.Contains("Manage/UserProfil", _page.Url);
    }

    public async Task VerifyLimitedViewForAPI()
    {
        var isInvisible = await NavBarList.CountAsync() == 0;
        Assert.True(isInvisible);
    }

    public async Task ValidateElementsNotVisibleAsync()
    {
        // Assert that NavBarList is not visible
        var isNavBarListVisible = await NavBarList.IsVisibleAsync();
        if (isNavBarListVisible)
        {
            throw new Exception("NavBarList should not be visible, but it is.");
        }

        // Assert that MessagesMenu is not visible
        var isMessagesMenuVisible = await MessagesMenu.IsVisibleAsync();
        if (isMessagesMenuVisible)
        {
            throw new Exception("MessagesMenu should not be visible, but it is.");
        }

        // Assert that GlobalSearch is not visible
        var isGlobalSearchVisible = await GlobalSearch.IsVisibleAsync();
        if (isGlobalSearchVisible)
        {
            throw new Exception("GlobalSearch should not be visible, but it is.");
        }
    }

    public async Task ValidateUserPermissions(string page)
    {
        
        await _basePage.NavigateTo(page);
        
    }



    private async Task Login(string userName, string password)
    {
        if (await UserName.CountAsync() > 0)
        {
            await UserName.FillAsync(userName);
            await Password.FillAsync(password);
            await ContinueBtn.ClickAsync();
        }
        else
        {
            await OrigUserNameBox.FillAsync(userName);
            await OrigPasswordBox.FillAsync(password);
            await OrigSignInBtn.ClickAsync();
        }
        //Thread.Sleep(3000);
        if (!await TermsAndConditionHeader.IsHiddenAsync())
        {
            await Accept.ClickAsync();
            await ContinueBtn.ClickAsync();
        }
    }
    
    
    
    
    
    
    
    
    
    
}