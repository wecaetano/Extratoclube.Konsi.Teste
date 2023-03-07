using Extratoclube.Konsi.Domain.Contracts.v1;
using Extratoclube.Konsi.Domain.DTOs.v1;
using Extratoclube.Konsi.Domain.Helpers.v1;
using Extratoclube.Konsi.Domain.Options.v1;
using Extratoclube.Konsi.Domain.Resources.v1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System.Net;

namespace Extratoclube.Konsi.Infraestrutura.Services.v1;

public class CrawlerService : ICrawlerService
{
    protected readonly IOptions<SeleniumOptions> _seleniumOptions;
    private readonly IWebDriver _driver;
    private readonly string _loginUrl;
    private readonly ILogger<CrawlerService> _logger;

    public CrawlerService(
        IOptions<SeleniumOptions> seleniumOptions,
        ILogger<CrawlerService> logger)
    {
        _seleniumOptions = seleniumOptions;

        // Inicializa o WebDriver remoto com o Selenium Server
        _driver = new RemoteWebDriver(new Uri(seleniumOptions.Value.SeleinumUrl), new ChromeOptions());

        // Define o tempo de espera implícito do WebDriver para 10 segundos
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

        // URL de login
        _loginUrl = seleniumOptions.Value.WebUrl;
        _logger = logger;
    }

    // Implementação do método CrawlerAsync da interface ICrawlerService
    public async Task<CustomApiResponse> CrawlerAsync(RegistrationRequestDto dto)
    {
        try
        {
            _logger.LogInformation(string.Format(Message.ServiceStart, nameof(CrawlerService)));

            // Valida o CPF
            if (!Validate.Cpf(dto.Document))
                throw new ApplicationException("CPF inválido");

            // Navega para a página de login
            NavigateTo();

            // Realiza o login
            Login(dto.Login, dto.Password);

            // Navega para a página do acordion
            NavigateToAcordion();

            // Obtém o extrato
            var result = await GetExtrato(dto.Document);

            _logger.LogInformation(string.Format(Message.ServiceEnd, nameof(CrawlerService)));

            // Retorna a resposta personalizada
            return new CustomApiResponse()
            {
                Result = result,
                Status = HttpStatusCode.OK,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format(Message.Error, nameof(CrawlerService)));

            // Retorna uma resposta personalizada em caso de erro
            return new CustomApiResponse()
            {
                Status = HttpStatusCode.InternalServerError,
                Notifications = new List<Notification> { new Notification { Message = ex.Message } }
            };
        }
        finally
        {
            Dispose();
        }

    }

    // Navega para a URL de login
    public void NavigateTo()
    {
        _driver.Navigate().GoToUrl(_loginUrl);
    }

    // Método para fazer login na aplicação usando as credenciais fornecidas
    public void Login(string login, string password)
    {
        // Encontra o elemento de entrada do usuário e insere o nome de usuário fornecido
        var userElement = _driver.FindElement(By.Id("user"));
        userElement.Click();
        userElement.SendKeys(login);

        // Encontra o elemento de entrada de senha e insere a senha fornecida
        var passwordElement = _driver.FindElement(By.Id("pass"));
        passwordElement.Click();
        passwordElement.SendKeys(password);

        // Encontra o botão de login e clica nele
        var buttonElement = _driver.FindElement(By.Id("botao"));
        buttonElement.Click();

        Thread.Sleep(500);
        // Encontra o botão de fechar e clica nele para fechar qualquer mensagem de alerta ou popup
        var buttonCloseElement = _driver.FindElement(By.XPath("//app-modal-fila/ion-button"));
        buttonCloseElement.Click();
    }

    // Método para navegar até a página do menu
    public void NavigateToAcordion()
    {
        Thread.Sleep(500);
        // Encontra o elemento do menu e clica nele
        var menuTabElement = _driver.FindElement(By.XPath("//ion-menu"));
        menuTabElement.Click();

        Thread.Sleep(1000);
        //Encontra o elemento econtrar benefícios de um CPF
        var colapsibleFindBenefitElement = _driver.FindElement(By.XPath("//span[contains(text(), 'Encontrar Benefícios de um CPF')]"));
        new Actions(_driver).MoveToElement(colapsibleFindBenefitElement).Perform();
        colapsibleFindBenefitElement.Click();

    }

    // Método para obter o extrato do documento com o ID fornecido
    public Task<List<string>> GetExtrato(string domcumentId)
    {
        var result = new List<string>();

        // Encontra o elemento de entrada de documento e insere o ID do documento fornecido
        var inputDocumentElement = _driver.FindElement(By.XPath("//input[@name='ion-input-1']"));
        new Actions(_driver).MoveToElement(inputDocumentElement).Perform();
        inputDocumentElement.Click();
        inputDocumentElement.SendKeys(domcumentId);

        // Encontra o botão "Encontrar Benefício" e clica nele para buscar o documento
        var buttonBenefitFindElement = _driver.FindElement(By.XPath("//ion-card/ion-grid/ion-row[2]/ion-col/ion-card/ion-button"));
        new Actions(_driver).MoveToElement(buttonBenefitFindElement).Perform();
        buttonBenefitFindElement.Click();

        // Encontra o elemento com o resultado do extrato e retorna seu texto como uma tarefa
        var resultCardElement = _driver.FindElement(By.XPath("//*[@id=\"extratoonline\"]/ion-row[2]/ion-col/ion-card/ion-grid/ion-row[2]/ion-col/ion-card"));
        var resultElements = resultCardElement.FindElements(By.TagName("ion-item"));

        if (resultElements.Any())
        {
            foreach (var element in resultElements)
            {
                result.Add(element.Text);
            }
        }

        return Task.FromResult(result);
    }

    // Método para liberar os recursos usados pelo driver do navegador
    private void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}
