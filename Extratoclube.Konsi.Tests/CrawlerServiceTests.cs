using Extratoclube.Konsi.Core.Services.v1;
using Extratoclube.Konsi.Domain.DTOs.v1;
using Extratoclube.Konsi.Domain.Options.v1;
using Extratoclube.Konsi.Tests.DTOs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Extratoclube.Konsi.Tests;

public class CrawlerServiceTests
{
    private readonly CrawlerService _crawlerService;

    public CrawlerServiceTests()
    {
        var seleniumOptions = Options.Create(new SeleniumOptions 
        { 
            WebUrl = "http://ionic-application.s3-website-sa-east-1.amazonaws.com/login",
            SeleinumUrl = "http://localhost:4444/wd/hub"
        });
        _crawlerService = new CrawlerService(seleniumOptions);
    }

    public static IEnumerable<object[]> GetBenefitFromDataGenerator()
    {
        yield return new object[] { new BenefitRequestTestDto { Document = "810.693.915-49", Login = "testekonsi", Password = "testekonsi", Results = new List<string> { "1649569847" } } };
        yield return new object[] { new BenefitRequestTestDto { Document = "927.100.938-04", Login = "testekonsi", Password = "testekonsi", Results = new List<string> { "6158750623", "1189485459" } } };
        yield return new object[] { new BenefitRequestTestDto { Document = "092.247.425-72", Login = "testekonsi", Password = "testekonsi", Results = new List<string> { "6200435549" } } };
        yield return new object[] { new BenefitRequestTestDto { Document = "527.100.175-04", Login = "testekonsi", Password = "testekonsi", Results = new List<string> { "6273566570" } } };
        yield return new object[] { new BenefitRequestTestDto { Document = "233.565.375-04", Login = "testekonsi", Password = "testekonsi", Results = new List<string> { "6165831595" } } };
        yield return new object[] { new BenefitRequestTestDto { Document = "452.603.114-34", Login = "testekonsi", Password = "testekonsi", Results = new List<string> { "1457941845" } } };
        yield return new object[] { new BenefitRequestTestDto { Document = "021.510.535-47", Login = "testekonsi", Password = "testekonsi", Results = new List<string> { "1698248560" } } };
        yield return new object[] { new BenefitRequestTestDto { Document = "654.262.075-34", Login = "testekonsi", Password = "testekonsi", Results = new List<string> { "1823882347" } } };
        yield return new object[] { new BenefitRequestTestDto { Document = "284.067.965-53", Login = "testekonsi", Password = "testekonsi", Results = new List<string> { "1837239603" } } };
        yield return new object[] { new BenefitRequestTestDto { Document = "098.907.775-68", Login = "testekonsi", Password = "testekonsi", Results = new List<string> { "1855516087" } } };
    }

    [Theory]
    [MemberData(nameof(GetBenefitFromDataGenerator))]
    public async void CrawlerService_Crawler_RetornaBenefits(BenefitRequestTestDto benefitDataTest)
    {
        //Arrange
        var benefitRequestDto = new BenefitRequestDto { Document = benefitDataTest.Document, Login = benefitDataTest.Login, Password = benefitDataTest.Password };

        //Act
        var benefits = await _crawlerService.CrawlerAsync(benefitRequestDto);

        //Assert
        Assert.True(benefitDataTest.Results.All(s => benefits.Result.Benefits.Contains(s)));
    }
}