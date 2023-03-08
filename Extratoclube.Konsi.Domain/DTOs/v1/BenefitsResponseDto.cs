using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extratoclube.Konsi.Domain.DTOs.v1;
public sealed record BenefitsResponseDto
{
    public IEnumerable<string> Benefits { get; set; }
}
