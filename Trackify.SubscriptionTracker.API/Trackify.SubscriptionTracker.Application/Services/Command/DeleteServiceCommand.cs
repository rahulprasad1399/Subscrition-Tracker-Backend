using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.SubscriptionTracker.Application.Services.Command
{
    public class DeleteServiceCommand :IRequest<int>
    {
        public int Id { get; set; }
    }
    public class DeleteServiceCommandHandler
    {
    }
    public class DeleteServiceCommandValidator
    {
    }
}
