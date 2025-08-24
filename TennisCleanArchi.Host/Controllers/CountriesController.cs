using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace TennisCleanArchi.Host.Controllers;
public class CountriesController : BaseController
{
    public CountriesController(IMediator mediator)
        : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get a paginated list of countries", Description = "Page size max is 100")]
    public async Task<IActionResult> GetCountries([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new Application.Countries.ListCountries.ListCountriesRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        var countries = await _mediator.Send(query);
        return Ok(countries);
    }
}
