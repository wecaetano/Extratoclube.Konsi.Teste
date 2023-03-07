using Extratoclube.Konsi.Domain.Contracts.v1;
using Extratoclube.Konsi.Domain.Options.v1;
using Extratoclube.Konsi.Core.Services.v1;

namespace Extratoclube.Konsi.Api;

public static class Bootstraper
{
    public static IServiceCollection AddApplicationBootstrapper(this IServiceCollection services)
    {
        services.AddScoped<ICrawlerService, CrawlerService>();
        return services;
    }

    public static void ConfigureSeleniumOptions(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider()
        .GetRequiredService<IConfiguration>();

        var sectionSelenium = configuration.GetSection("Selenium");

        services.Configure<SeleniumOptions>(s =>
        {
            s.WebUrl = sectionSelenium.GetSection("WebUrl").Value;
            s.SeleinumUrl = sectionSelenium.GetSection("SeleinumUrl").Value;
        });
    }
}
