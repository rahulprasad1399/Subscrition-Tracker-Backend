using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trackify.SubscriptionTracker.Application.SubscriptionTypes.Command;
using Trackify.SubscriptionTracker.Application.SubscriptionTypes.Query;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubscriptionTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            GetSubscriptionTypesQuery query = new GetSubscriptionTypesQuery();
            IEnumerable<SubscriptionType> response = await _mediator.Send(query);
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            GetSubscriptionTypeByIdQuery query = new GetSubscriptionTypeByIdQuery();
            query.Id = id;
            SubscriptionType response = await _mediator.Send(query);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateSubscriptionTypeCommand command)
        {
            int response = await _mediator.Send(command);
            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id,UpdateSubscriptionTypeCommand command)
        {
            command.Id = id;
            int response =await _mediator.Send(command);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            DeleteSubscriptionTypeCommand command = new DeleteSubscriptionTypeCommand();
            command.Id = id;
            int response =await _mediator.Send(command);
            return Ok(response);
        }
    }
}
