using MediatR;
using Trackify.SubscriptionTracker.Application.Dtos;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Application.SubscriptionGetAllDto;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.SubscriptionQuery
{
    public class GetAllSubscriptionQuery : IRequest<SubscriptionResponseDto>
    {
        public int UserId { get; set; }
    }

    public class GetAllSubscriptionQueryHandler : IRequestHandler<GetAllSubscriptionQuery, SubscriptionResponseDto>
    {
        private readonly ISubscriptionGetAll _subscriptionGetAll;

        public GetAllSubscriptionQueryHandler(ISubscriptionGetAll subscriptionGetAll)
        {
            _subscriptionGetAll = subscriptionGetAll;
        }

        public async Task<SubscriptionResponseDto> Handle(GetAllSubscriptionQuery request, CancellationToken cancellationToken)
        {
            var subscriptions = await _subscriptionGetAll.SubscriptionGetAllAsync(request.UserId);

            var subscriptionGetDtos = subscriptions.Select(subscription => new SubscriptionGetDto
            {
                Id = subscription.Id,
                ServiceId = subscription.ServiceId,
                ServiceName = subscription.Service.ServiceName,
                SubscriptionTypeName = subscription.SubscriptionType.TypeName,
                CategoryName = subscription.Service.Category.CategoryName,
                Cost = subscription.Cost,
                PurchaseDate = subscription.PurchaseDate,
                RenewalDate = subscription.RenewalDate,
                Status = subscription.Status
            }).ToList();

            return new SubscriptionResponseDto
            {
                TotalItem = subscriptionGetDtos.Count,
                ActiveItem = subscriptionGetDtos.Where((sub) => sub.Status == Subscription.ActiveStatus.Active).Count(),
                MoneySpentMonthly = subscriptionGetDtos
                                    .Where(x => x.PurchaseDate.Month == DateTime.UtcNow.Month
                                     && x.PurchaseDate.Year == DateTime.UtcNow.Year)
                                    .Sum(x => x.Cost),
                Subscriptions = subscriptionGetDtos
            };
        }

    }
}