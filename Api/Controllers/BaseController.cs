using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly ISender _mediator;

        public BaseController(ISender mediator)
        {
            _mediator = mediator;
        }

    }
}
