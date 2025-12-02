using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trackify.SubscriptionTracker.Application.Dtos;
using Trackify.SubscriptionTracker.Application.Services.Command;
using Trackify.SubscriptionTracker.Application.Services.Query;
using Trackify.SubscriptionTracker.Domain.Entity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Trackify.SubscriptionTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServiceController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in.");
            }

            GetServicesQuery getServicesQuery = new();
            getServicesQuery.userId = int.Parse(userId);
            IEnumerable<ServiceDto> services = await _mediator.Send(getServicesQuery);
            return Ok(services);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            GetServiceByIdQuery getServiceByIdQuery = new()
            {
                Id = id
            };
            ServiceDto service = await _mediator.Send(getServiceByIdQuery);
            return Ok(service);
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateServiceCommand command)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("User not logged in.");
            }
            command.CreatedByUserId = int.Parse(userIdString);
            int response = await _mediator.Send(command);
            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateServiceCommand command)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("User not logged in.");
            }

            command.CreatedByUserId = int.Parse(userIdString);
            command.Id = id;
            int response = await _mediator.Send(command);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            DeleteServiceCommand command = new()
            {
                Id = id
            };
            int response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
