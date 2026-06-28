using System;

namespace Takwene.Application.DTOs.Tracks
{
    public record DistributionDto(
        int DspId,
        string DspName,
        string Status,
        DateTime SubmittedAt);
}
