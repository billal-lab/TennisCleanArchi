using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TennisCleanArchi.Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;
    
    protected BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
