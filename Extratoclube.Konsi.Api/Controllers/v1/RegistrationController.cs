using Extratoclube.Konsi.Domain.Contracts.v1;
using Extratoclube.Konsi.Domain.DTOs.v1;
using Extratoclube.Konsi.Domain.Resources.v1;
using Microsoft.AspNetCore.Mvc;

namespace Extratoclube.Konsi.Api.Controllers.v1;
[Route("api/v1/registration")]
[Produces("application/json")]
[ApiController]
public class RegistrationController : BaseController
{
    private readonly ILogger<RegistrationController> _logger;
    private readonly ICrawlerService _crawlerService;

    public RegistrationController(
        ILogger<RegistrationController> logger,
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
        _logger.LogInformation(string.Format(Message.EnpointStart, nameof(RegistrationController)));

        var result = await _crawlerService.CrawlerAsync(dto);

        return ReturnFromNotifications<BenefitsResponseDto>(result);
    }
}
