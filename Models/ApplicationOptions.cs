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
    public ApplicationUser ClientUser { get; set; }
    public ApplicationUser ClientUser1 { get; set; }
    public ApplicationUser AgencyUser { get; set; }
    public ApplicationUser ApiUser { get; set; }
    public ApplicationUser PerDiemClientFacilityUser { get; set; }
    public string AuthType { get; set; }
    public string BaseApiUrl { get; set; }
    public string CreateCandidateApiRoute { get; set; }
    public string CreateSubmissionApiRoute { get; set; }
    public string GetSubmissionJobApiRoute { get; set; }
    public string ApiUserName { get; set; }
    public string ApiPassword { get; set; }
    public string CombinedAuthToken => $"{ApiUserName} {ApiPassword}";

    public static ApplicationOptions GetConfig(IConfiguration configuration)
    {
        var appOptions = new ApplicationOptions();
        configuration.GetSection(Application).Bind(appOptions);

        return appOptions;
    }
}
