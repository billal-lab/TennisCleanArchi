using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TennisCleanArchi.Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public CountriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
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
