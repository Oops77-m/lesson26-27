using Lesson25.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;

namespace Lesson25.Helpers;

public class WebDriverFactory
{
    public static IWebDriver Create(BrowserOptions browserOptions)
    {
        return browserOptions.Browser switch
        {
            Browser.Chrome => CreateChrome(browserOptions),
            Browser.Edge => CreateEdge(browserOptions),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    
    
    private static IWebDriver CreateChrome(BrowserOptions browserOptions)
    {
        var options = new ChromeOptions();

        if (browserOptions.Headless)
        {
            options.AddArgument("--headless");
        }

        if (browserOptions.Maximize)
        {
            options.AddArgument("--start-maximized");
        }

        options.AddArgument("--guest");
        options.EnableDownloads = true;
        options.AddUserProfilePreference(
            "download.prompt_for_download", false);

        options.AddUserProfilePreference(
            "plugins.always_open_pdf_externally", true);

        if (browserOptions.Remote)
        {
            var seleniumUrl = Environment.GetEnvironmentVariable("JENKINS_URL") != null
                ? "http://selenium-chrome:4444"
                : "http://localhost:4444";

            return new RemoteWebDriver(new Uri(seleniumUrl), options);
        }

        var downloadPath = Path.Combine(
            TestContext.CurrentContext.WorkDirectory,
            "Downloads");

        Directory.CreateDirectory(downloadPath);

        options.AddUserProfilePreference("download.default_directory", downloadPath);
        options.AddUserProfilePreference("download.prompt_for_download", false);
        options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);

        return new ChromeDriver(options);
    }



    private static IWebDriver CreateEdge(BrowserOptions browserOptions)
    {
        var options = new EdgeOptions();
        if (browserOptions.Headless)
        {
            options.AddArgument("--headless");
        }

        if (browserOptions.Maximize)
        {
            options.AddArgument("--start-maximized");
        }

        return new EdgeDriver(options);
    }
}