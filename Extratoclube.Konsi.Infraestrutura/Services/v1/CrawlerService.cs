using Extratoclube.Konsi.Domain.Contracts.v1;
using Extratoclube.Konsi.Domain.DTOs.v1;
using Extratoclube.Konsi.Domain.Helpers.v1;
using Extratoclube.Konsi.Domain.Options.v1;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Extratoclube.Konsi.Infraestrutura.Services.v1;
public class CrawlerService : ICrawlerService
{
    protected readonly IOptions<SeleniumOptions> _seleniumOptions;
    private readonly IWebDriver _driver;
    private readonly string _loginUrl;
    public CrawlerService(IOptions<SeleniumOptions> seleniumOptions)
    {
        _seleniumOptions = seleniumOptions;

        ChromeOptions options = new ChromeOptions();
        options.AddArgument("--start-maximized");
                
        var driverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        //_driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), options);

        _driver = new ChromeDriver("D:\\Git\\Extratoclube.Konsi.Teste\\src\\Extratoclube.Konsi.Infraestrutura\\Drivers", options);

        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

        _loginUrl = _seleniumOptions.Value.WebUrl;

    }

    public async Task<CustomApiResponse> CrawlerAsync(RegistrationRequestDto dto)
    {
        try
        {
            if (!Validate.Cpf(dto.Document))
                throw new ApplicationException("CPf inávlido");

            NavigateTo();

            Login(dto.Login, dto.Password);

            NavigateToMenuPage();

            var result = await GetExtrato(dto.Document);

            return new CustomApiResponse()
            {
                Result = result,
                Status = HttpStatusCode.OK,
            };
        }
        catch (Exception ex)
        {
            return new CustomApiResponse()
            {
                Status = HttpStatusCode.InternalServerError,
                Notifications = new List<Notification> { new Notification { Message = ex.Message } }
            };
        }
        finally { Dispose(); }
        
    }

    private void NavigateTo()
    {
        _driver.Navigate().GoToUrl(_loginUrl);
    }

    private void Login(string login, string password)
    {
        var userElement = _driver.FindElement(By.Id("user"));
        userElement.Click();
        userElement.SendKeys(login);       

        var passwordElement = _driver.FindElement(By.Id("pass"));
        passwordElement.Click();
        passwordElement.SendKeys(password);

        var buttonElement = _driver.FindElement(By.Id("botao"));
        buttonElement.Click();
        
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

        var buttonCloseElement = _driver.FindElement(By.XPath("//ion-modal[@id='ion-overlay-1']/div[2]/app-modal-fila/ion-button"));
        buttonCloseElement.Click();
    }

    private void NavigateToMenuPage()
    {
        var menuTabElement = _driver.FindElement(By.XPath("//ion-menu"));
        menuTabElement.Click();

        var colapsibleFindBenefitElement = _driver.FindElement(By.XPath("//ion-button[9]"));
        colapsibleFindBenefitElement.Click();
    }

    private Task<string> GetExtrato(string domcumentId)
    {
        var inputDocumentElement = _driver.FindElement(By.XPath("//input[@name='ion-input-1']"));

        inputDocumentElement.Click();
        inputDocumentElement.SendKeys(domcumentId);
        
        var buttonBenefitFindElement = _driver.FindElement(By.XPath("//ion-card/ion-grid/ion-row[2]/ion-col/ion-card/ion-button"));

        buttonBenefitFindElement.Click();

        var resultElement = _driver.FindElement(By.XPath("//ion-grid[@id='extratoonline']/ion-row[2]/ion-col/ion-card/ion-grid/ion-row[2]/ion-col/ion-card/ion-item/ion-label"));

        return Task.FromResult(resultElement.Text);
    }
    private void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}
