using Extratoclube.Konsi.Domain.DTOs.v1;

namespace Extratoclube.Konsi.Domain.Contracts.v1;

public interface ICrawlerService
{
    Task<CustomApiResponse<BenefitsResponseDto>> CrawlerAsync(BenefitRequestDto dto);
}