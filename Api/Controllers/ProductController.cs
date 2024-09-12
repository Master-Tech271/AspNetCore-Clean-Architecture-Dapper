using Api.Application.Common;
using Api.Application.Products.Command.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly ILogService _logger;
        public ProductController(ISender mediator, ILogService logService) : base(mediator)
        {
            _logger = logService;
        }

        [Authorize(Policy = "RequireUserRole")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.CompletedTask;

            return Ok("GET()");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        public async Task<IActionResult> AddAsync(CreateProductCommand request)
        {
            int productId = await _mediator.Send(request);

            return Ok(productId);
        }
    }
}
