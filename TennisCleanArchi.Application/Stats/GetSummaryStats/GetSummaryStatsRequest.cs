using MediatR;
using TennisCleanArchi.Application.Stats.GetSummaryStats;

namespace TennisCleanArchi.Application.Stats.GetStats;

public class GetSummaryStatsRequest : IRequest<SummaryStatsDto>
{
}
