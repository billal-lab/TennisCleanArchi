using MediatR;
using TennisCleanArchi.Application.Common;

namespace TennisCleanArchi.Application.Countries.ListCountries;

public class ListCountriesRequest : IRequest<PagedResult<CountryDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
