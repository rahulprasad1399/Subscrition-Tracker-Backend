using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trackify.SubscriptionTracker.Application.Categories.Command;
using Trackify.SubscriptionTracker.Application.Categories.Query;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            IEnumerable<Category> result =  await _mediator.Send(new GetCategoriesQuery());
            return Ok(result.ToList());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            GetCategoryByIdQuery command = new GetCategoryByIdQuery();
            command.Id= id;
            Category result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateCategoryCommand command)
        {
            int result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPut("{id}")]
        private async Task<IActionResult> Update([FromRoute] int id,UpdateCategoryCommand command)
        {
            command.Id = id;
            int result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            DeleteCategoryCommand command = new DeleteCategoryCommand();
            command.Id= id;
            int result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
