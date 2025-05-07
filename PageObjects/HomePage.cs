using DemoFramewrok.Enum;
using DemoFramewrok.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

namespace DemoFramewrok.PageObjects;

public class HomePage
{
    private readonly IPage _page;
    private readonly IConfiguration _config;

    public HomePage(IPage page, IConfiguration config)
    {
        _config = config;
        _page = page;
    }

   
    public async Task VerifyUserName(string userRole)
    {
        string? firstLastName = null;
        UserRoles userRoles;
        System.Enum.TryParse(userRole, ignoreCase: true, out userRoles);
        switch (userRoles)
        {
            case UserRoles.Staff:
                firstLastName = ApplicationOptions.GetConfig(_config).StaffUser.DisplayName;
                break;
            case UserRoles.Agency:
                firstLastName = ApplicationOptions.GetConfig(_config).AgencyUser.DisplayName;
                break;
            case UserRoles.Client :
                firstLastName = ApplicationOptions.GetConfig(_config).ClientUser.DisplayName;
                break;
            case UserRoles.Client1:
                firstLastName = ApplicationOptions.GetConfig(_config).ClientUser.DisplayName;
                break;
            case UserRoles.ClientFacilityTestUser:
                firstLastName = ApplicationOptions.GetConfig(_config).PerDiemClientFacilityUser.DisplayName;
                break;

        }
        Assert.Contains(firstLastName, await UserNameTx);
    }

    public async Task LogOut()
    {
        Console.WriteLine("****** LogOut method here");
        await ClickUserNameDropDown;
        await ClickSignOut;
        Console.WriteLine("****** LogOut method - Clicked logout");
    }
    public async Task OpenDropDown(int index)
    {
        while ((await IndivMenuBar(index).GetAttributeAsync("class")).Contains("dropdown") && !(await IndivMenuBar(index).GetAttributeAsync("class")).Contains("open"))
        {
            await IndivMenuBar(index).ClickAsync();
        }
    }
    public async Task NavigateToAllPages()
    {
        for (int i = 0; i < await MenuBarList.CountAsync(); i++)
        {
            await OpenDropDown(i);
            var NumberOfSubMenu = await EachSubMenu(i).CountAsync();
            if (NumberOfSubMenu > 0)
            {
                for (int j = 0; j < NumberOfSubMenu; j++)
                {
                    await EachSubMenu(i).Nth(j).HoverAsync();
                    var EachSubMenuOnRight = EachSubMenu(i).Nth(j).GetByRole(AriaRole.Listitem);

                    var NumberOfSubMenuListOnRight = await EachSubMenuOnRight.CountAsync();
                    if (NumberOfSubMenuListOnRight > 0)
                    {
                        for (int n = 0; n < NumberOfSubMenuListOnRight; n++)
                        {
                            // this for loop will loop through all right-side-SubMenus
                            var PerDiemSchedulerPopUp = await EachSubMenuOnRight.Nth(n).TextContentAsync();
                            var isPerDiemWindow = await IndivMenuBar(i).InnerTextAsync();
                            await EachSubMenuOnRight.Nth(n).ClickAsync();
                            if (PerDiemSchedulerPopUp.Contains("Calendar View - Staffing Pool") && isPerDiemWindow.Contains("PER DIEM"))
                            {
                                await SelectScheduleForViewCalendar();
                            }
                            // verify if page opened successfully
                            await VerifyPageContent();
                            await OpenDropDown(i);
                            await EachSubMenu(i).Nth(j).HoverAsync();
                        }
                    }
                    else
                    {
                        // this loop will loop through all SubMenus
                        var OracleExport = await EachSubMenu(i).Nth(j).TextContentAsync();
                        if (OracleExport.Contains("Oracle A/P Export") || OracleExport.Contains("A/P Management Hub"))
                        {
                            // Possible issue, taking more than 5 mins to load these two pages, so skip their validation
                            //await eachSubMenu.Nth(j).ClickAsync();
                        }
                        else
                        {
                            await EachSubMenu(i).Nth(j).ClickAsync();
                            await _page.WaitForURLAsync(ApplicationOptions.GetConfig(_config).BaseUrl + "/**");

                            // verify if page opened successfully
                            await VerifyPageContent();
                        }

                    }
                    await OpenDropDown(i);
                }
            }
            else
            {
                // this loop will loop through all Main Menus
                await IndivMenuBar(i).ClickAsync();
                await _page.WaitForURLAsync(ApplicationOptions.GetConfig(_config).BaseUrl + "/**");
                // verify if page opened successfully
                await VerifyPageContent();
            }
        }

    }
    public async Task SelectScheduleForViewCalendar()
    {
        Thread.Sleep(2000);
        await ScheduleBox.ClickAsync();
        await ScheduleBoxListResult.First.ClickAsync();
    }
    public async Task VerifyPageContent()
    {
        await _page.WaitForTimeoutAsync(2000);
        Assert.True(await PageContent.CountAsync() > 0 || await PageContentForAgGrid.CountAsync() > 0);
    }
    public async Task NavigateToPageSpecific(string pageName)
    {
        var pageFound = false;

        for (int i = 0; i < await MenuBarList.CountAsync(); i++)
        {
            await OpenDropDown(i);
            var NumberOfSubMenu = await EachSubMenu(i).CountAsync();
            if (NumberOfSubMenu > 0)
            {
                for (int j = 0; j < NumberOfSubMenu; j++)
                {
                    await EachSubMenu(i).Nth(j).HoverAsync();
                    var EachSubMenuOnRight = EachSubMenu(i).Nth(j).GetByRole(AriaRole.Listitem);

                    var NumberOfSubMenuListOnRight = await EachSubMenuOnRight.CountAsync();
                    if (NumberOfSubMenuListOnRight > 0)
                    {
                        for (int n = 0; n < NumberOfSubMenuListOnRight; n++)
                        {
                            // this for loop will loop through all right-side-SubMenus
                            var NameOfSubMenuRightTx = await EachSubMenuOnRight.Nth(n).TextContentAsync();

                            if (NameOfSubMenuRightTx.Contains(pageName))
                            {
                                pageFound = true;
                                await _page.WaitForLoadStateAsync();
                                await _page.WaitForURLAsync(ApplicationOptions.GetConfig(_config).BaseUrl + "/**");
                                return;
                            }
                            // verify if page opened successfully
                            await VerifyPageContent();
                            await OpenDropDown(i);
                            await EachSubMenu(i).Nth(j).HoverAsync();

                        }
                    }
                    else
                    {
                        // this loop will loop through all SubMenus
                        var NameOfSubMenuTx = await EachSubMenu(i).Nth(j).TextContentAsync();
                        if (NameOfSubMenuTx.Contains(pageName))
                        {
                            pageFound = true;
                            await EachSubMenu(i).Nth(j).ClickAsync();
                            await _page.WaitForURLAsync(ApplicationOptions.GetConfig(_config).BaseUrl + "/**");
                            await _page.WaitForLoadStateAsync();
                            // verify if page opened successfully
                            await VerifyPageContent();
                            return;
                        }

                    }
                    await OpenDropDown(i);
                }
            }
            else
            {
                // this loop will loop through all Main Menus
                var NameOfMenuTx = await IndivMenuBar(i).TextContentAsync();
                if (NameOfMenuTx.Contains(pageName))
                {
                    pageFound = true;
                    await IndivMenuBar(i).ClickAsync();
                    await _page.WaitForURLAsync(ApplicationOptions.GetConfig(_config).BaseUrl + "/**");
                    await _page.WaitForLoadStateAsync();
                    return;
                }
            }
        }

    }

    // This method is upgraded version of "NavigateToPageSpecific" method which will directly go to specific SubMen of Nav bar, will get ride of above method next PR
    public async Task NavigateToPageSpecificNew(string mainMenu, string pageName)
    {
        var pageFound = false;
        int i = 0;
        for (int n = 0; n < await MenuBarList.CountAsync(); n++)
        {
            if ((await MenuBarList.Nth(n).TextContentAsync()).Contains(mainMenu))
            {
                i = n;
                break;
            }
        }
        await OpenDropDown(i);
        var NumberOfSubMenu = await EachSubMenu(i).CountAsync();
        if (NumberOfSubMenu > 0)
        {
            for (int j = 0; j < NumberOfSubMenu; j++)
            {
                await EachSubMenu(i).Nth(j).HoverAsync();
                var EachSubMenuOnRight = EachSubMenu(i).Nth(j).GetByRole(AriaRole.Listitem);

                var NumberOfSubMenuListOnRight = await EachSubMenuOnRight.CountAsync();
                if (NumberOfSubMenuListOnRight > 0)
                {
                    for (int n = 0; n < NumberOfSubMenuListOnRight; n++)
                    {
                        // this for loop will loop through all right-side-SubMenus
                        var NameOfSubMenuRightTx = await EachSubMenuOnRight.Nth(n).TextContentAsync();

                        if (NameOfSubMenuRightTx.Contains(pageName))
                        {
                            pageFound = true;
                            await EachSubMenuOnRight.Nth(n).ClickAsync();
                            await _page.WaitForURLAsync(ApplicationOptions.GetConfig(_config).BaseUrl + "/**");
                            await _page.WaitForLoadStateAsync();
                            return;
                        }
                        // verify if page opened successfully
                        await VerifyPageContent();
                        await OpenDropDown(i);
                        await EachSubMenu(i).Nth(j).HoverAsync();

                    }
                }
                else
                {
                    // this loop will loop through all SubMenus
                    var NameOfSubMenuTx = await EachSubMenu(i).Nth(j).TextContentAsync();
                    if (NameOfSubMenuTx.Contains(pageName))
                    {
                        pageFound = true;
                        await EachSubMenu(i).Nth(j).ClickAsync();
                        await _page.WaitForURLAsync(ApplicationOptions.GetConfig(_config).BaseUrl + "/**");
                        await _page.WaitForLoadStateAsync();
                        // verify if page opened successfully
                        await VerifyPageContent();
                        return;
                    }

                }
                await OpenDropDown(i);
            }
        }
        else
        {
            // this loop will loop through all Main Menus
            var NameOfMenuTx = await IndivMenuBar(i).TextContentAsync();
            if (NameOfMenuTx.Contains(pageName))
            {
                pageFound = true;
                await IndivMenuBar(i).ClickAsync();
                await _page.WaitForURLAsync(ApplicationOptions.GetConfig(_config).BaseUrl + "/**");
                await _page.WaitForLoadStateAsync();
                return;
            }
        }


    }
}