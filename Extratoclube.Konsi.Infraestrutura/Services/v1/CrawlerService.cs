using Extratoclube.Konsi.Domain.Contracts.v1;
using Extratoclube.Konsi.Domain.DTOs.v1;
using Extratoclube.Konsi.Domain.Helpers.v1;
using Extratoclube.Konsi.Domain.Options.v1;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System.Net;
using System.Reflection;

namespace Extratoclube.Konsi.Infraestrutura.Services.v1;

public class CrawlerService : ICrawlerService
{
    protected readonly IOptions<SeleniumOptions> _seleniumOptions;
    private readonly IWebDriver _driver;
    private readonly string _loginUrl;

    public CrawlerService(IOptions<SeleniumOptions> seleniumOptions)
    {
        _seleniumOptions = seleniumOptions;

        // Opções do Chrome
        ChromeOptions options = new ChromeOptions();
        //options.AddArgument("--headless"); // Executa em modo headless (sem interface gráfica)
              
        // Inicializa o WebDriver remoto com o Selenium Server
        //_driver = new RemoteWebDriver(new Uri("http://selenium:4444/wd/hub"), options);

        _driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), options);

        // Define o tempo de espera implícito do WebDriver para 10 segundos
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

        // URL de login
        _loginUrl = _seleniumOptions.Value.WebUrl;
    }

    // Implementação do método CrawlerAsync da interface ICrawlerService
    public async Task<CustomApiResponse> CrawlerAsync(RegistrationRequestDto dto)
    {
        try
        {
            // Valida o CPF
            if (!Validate.Cpf(dto.Document))
                throw new ApplicationException("CPF inválido");

            // Navega para a página de login
            NavigateTo();

            // Realiza o login
            Login(dto.Login, dto.Password);

            // Navega para a página do menu
            NavigateToMenuPage();

            // Obtém o extrato
            var result = await GetExtrato(dto.Document);

            // Retorna a resposta personalizada
            return new CustomApiResponse()
            {
                Result = result,
                Status = HttpStatusCode.OK,
            };
        }
        catch (Exception ex)
        {
            // Retorna uma resposta personalizada em caso de erro
            return new CustomApiResponse()
            {
                Status = HttpStatusCode.InternalServerError,
                Notifications = new List<Notification> { new Notification { Message = ex.Message } }
            };
        }
        finally
        {
            // Fecha o navegador
            Dispose();
        }

    }

    // Navega para a URL de login
    private void NavigateTo()
    {
        _driver.Navigate().GoToUrl(_loginUrl);
        var s = _driver.PageSource; // Página de origem do HTML
    }

    // Método para fazer login na aplicação usando as credenciais fornecidas
    private void Login(string login, string password)
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

        // Encontra o botão de fechar e clica nele para fechar qualquer mensagem de alerta ou popup
        var buttonCloseElement = _driver.FindElement(By.XPath("//app-modal-fila/ion-button"));
        buttonCloseElement.Click();
    }

    // Método para navegar até a página do menu
    private void NavigateToMenuPage()
    {
        Thread.Sleep(500);
        // Encontra o elemento do menu e clica nele
        var menuTabElement = _driver.FindElement(By.XPath("//ion-menu"));
        menuTabElement.Click();

        Thread.Sleep(500);
        //Encontra o elemento econtrar benefícios de um CPF
        var colapsibleFindBenefitElement = _driver.FindElement(By.XPath("//span[contains(text(), 'Encontrar Benefícios de um CPF')]"));
        new Actions(_driver).MoveToElement(colapsibleFindBenefitElement).Perform();
        colapsibleFindBenefitElement.Click();

    }

    // Método para obter o extrato do documento com o ID fornecido
    private Task<string> GetExtrato(string domcumentId)
    {
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
        var resultElement = _driver.FindElement(By.XPath("//ion-grid[@id='extratoonline']/ion-row[2]/ion-col/ion-card/ion-grid/ion-row[2]/ion-col/ion-card/ion-item/ion-label"));
        new Actions(_driver).MoveToElement(resultElement).Perform();
        return Task.FromResult(resultElement.Text);
    }

    // Método para liberar os recursos usados pelo driver do navegador
    private void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}
