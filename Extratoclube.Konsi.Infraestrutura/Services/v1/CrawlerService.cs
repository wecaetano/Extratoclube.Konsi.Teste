using Extratoclube.Konsi.Domain.Contracts.v1;
using Extratoclube.Konsi.Domain.Options.v1;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Extratoclube.Konsi.Infraestrutura.Services.v1;
public class CrawlerService : ICrawlerService
{
    protected readonly IOptions<SeleniumOptions> _seleniumOptions;
    public CrawlerService(IOptions<SeleniumOptions> seleniumOptions)
    {
        _seleniumOptions = seleniumOptions;
    }

    public string CrawlerAsync()
    {
        ChromeOptions options = new();
        options.AddArgument("--start-maximized");

        var driverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        IWebDriver driver= new ChromeDriver("D:\\Git\\Crawler\\Extratoclube.Konsi\\Extratoclube.Konsi.Infraestrutura\\Drivers", options);

        driver.Manage().Timeouts().ImplicitWait = new TimeSpan(30);

        driver.Navigate().GoToUrl(_seleniumOptions.Value.WebUrl);

        var s = driver.FindElement(By.Id("user"));

        var a = "";

        return "";
    }
}
