using MediatR;
using System.Text.Json.Serialization;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.SubscriptionCommand
{
    public class DeleteSubscriptionCommand : IRequest<int>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }

    public class DeleteSubscriptionCommandHandler : IRequestHandler<DeleteSubscriptionCommand, int>
    {
        private readonly IGenericRepository<Subscription> _genericRepository;
        public DeleteSubscriptionCommandHandler(IGenericRepository<Subscription> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<int> Handle(DeleteSubscriptionCommand request, CancellationToken cancellationToken)
        {
            if(!await _genericRepository.ExistsAsync(request.Id))
            {
                throw new KeyNotFoundException("Subscription Id dosen't exist");
            }

            return await _genericRepository.DeleteAsync(request.Id);
        }
    }
}
