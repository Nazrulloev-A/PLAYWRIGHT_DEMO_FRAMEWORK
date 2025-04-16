using Microsoft.Extensions.Configuration;

namespace DemoFramewrok.Models;

public class ApplicationOptionsFixture
{ 
    public ApplicationOptionsFixture(IConfiguration configuration)
    {
        Options = ApplicationOptions.GetConfig(configuration);
    }

    public ApplicationOptions Options { get; private set; }
}

    
