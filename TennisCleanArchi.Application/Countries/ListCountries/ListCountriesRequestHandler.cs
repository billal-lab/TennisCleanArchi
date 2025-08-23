using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TennisCleanArchi.Application.Common;
using TennisCleanArchi.Application.Data;

namespace TennisCleanArchi.Application.Countries.ListCountries;

public class ListCountriesRequestHandler : IRequestHandler<ListCountriesRequest, PagedResult<CountryDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public ListCountriesRequestHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<CountryDto>> Handle(ListCountriesRequest request, CancellationToken cancellationToken)
    {
        var items = await _dbContext.Countries
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => c.Adapt<CountryDto>())
            .ToListAsync(cancellationToken);

        var totalItems = await _dbContext.Countries.CountAsync(cancellationToken);

        return new PagedResult<CountryDto>(request.PageNumber, request.PageSize, totalItems, items);
    }
}
