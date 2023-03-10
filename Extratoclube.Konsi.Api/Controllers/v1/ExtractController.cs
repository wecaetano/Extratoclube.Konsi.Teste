using Extratoclube.Konsi.Domain.Contracts.v1;
using Extratoclube.Konsi.Domain.DTOs.v1;
using Extratoclube.Konsi.Domain.Resources.v1;
using Microsoft.AspNetCore.Mvc;

namespace Extratoclube.Konsi.Api.Controllers.v1;
[Route("api/v1/extract")]
[Produces("application/json")]
[ApiController]
public class ExtractController : BaseController
{
    private readonly ILogger<ExtractController> _logger;
    private readonly ICrawlerService _crawlerService;

    public ExtractController(
        ILogger<ExtractController> logger,
        ICrawlerService crawlerService) : base()
    {
        _logger = logger;
        _crawlerService = crawlerService;
    }

    [HttpPost("benefits")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetBenefitsAsync([FromBody] BenefitRequestDto dto)
    {
        _logger.LogInformation(string.Format(Message.EnpointStart, nameof(ExtractController)));

        var result = await _crawlerService.CrawlerAsync(dto);

        return ReturnFromNotifications<BenefitsResponseDto>(result);
    }
}
