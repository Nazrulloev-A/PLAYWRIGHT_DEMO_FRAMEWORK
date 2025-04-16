using DemoFramewrok.PageObjects;
using Reqnroll;

namespace DemoFramewrok.StepDefinitions;

[Binding]
public class LoginStepDefinitions
{
    private readonly BasePage _basePage;
    private readonly LoginPage _loginPage;
    private readonly HomePage _homePage;
    
    public LoginStepDefinitions(BasePage basePage)
    {
        _basePage = basePage;
        _loginPage = new LoginPage(_basePage.Page, _basePage.Config);
        _homePage = new HomePage(_basePage.Page, _basePage.Config);

    }


    [Given("user navigates to Test home page")]
    public async Task GivenUserNavigatesToTestHomePage()
    {
        await _basePage.Navigate();
    }

    [When("user logs in using username and password for Staff")]
    public async Task WhenUserLogsInUsingUsernameAndPasswordForStaff()
    {
        await _loginPage.UserLogin("Staff");
    }


    [When("user successfully logged out")]
    public async Task WhenUserSuccessfullyLoggedOut(string userRole)
    {
        await _loginPage.UserLogin(userRole);
    }
}