using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;
using Trackify.SubscriptionTracker.Application.Exceptions;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;
using static Trackify.SubscriptionTracker.Domain.Entity.Subscription;

namespace Trackify.SubscriptionTracker.Application.SubscriptionCommand
{

    public class CreateSubscriptionCommand : IRequest<int>
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public int SubscriptionTypeId { get; set; }
        public decimal Cost { get; set; }
        public int BillingFrequency { get; set; }
        public string BillingPeriodUnit { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime RenewalDate { get; set; }
        public ActiveStatus ActiveStatus { get; set; }

    }

    public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, int>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Service> _serviceRepository;
        private readonly IGenericRepository<SubscriptionType> _subscriptionTypeRepository;
        private readonly IGenericRepository<Subscription> _genericRepository;

        public CreateSubscriptionCommandHandler(IGenericRepository<User> userRepository,
            IGenericRepository<Service> serviceRepository,
            IGenericRepository<SubscriptionType> subscriptionTypeRepository,
            IGenericRepository<Subscription> genericRepository)
        {
            _userRepository = userRepository;
            _serviceRepository = serviceRepository;
            _subscriptionTypeRepository = subscriptionTypeRepository;
            _genericRepository = genericRepository;
        }
        public async Task<int> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            if (!await _userRepository.ExistsAsync(request.UserId))
            {
                throw new NotFoundException(nameof(User),request.UserId);
            }

            if (!await _serviceRepository.ExistsAsync(request.ServiceId))
            {
                throw new NotFoundException(nameof(Service), request.ServiceId);
            }

            if (!await _subscriptionTypeRepository.ExistsAsync(request.SubscriptionTypeId))
            {
                throw new NotFoundException(nameof(SubscriptionType), request.SubscriptionTypeId);
            }

            Subscription subscription = new Subscription(request.UserId, request.ServiceId,
                request.SubscriptionTypeId, request.Cost, request.BillingFrequency, request.BillingPeriodUnit,
                request.PurchaseDate, request.RenewalDate, request.ActiveStatus);

            return await _genericRepository.AddAsync(subscription);
        }
    }

    public class CreateSubscriptionCommandValidator : AbstractValidator<CreateSubscriptionCommand>
    {
        public CreateSubscriptionCommandValidator()
        {
            RuleFor((x) => x.ServiceId)
                .NotNull().WithMessage("Service Id cannot be null");

            RuleFor((x) => x.SubscriptionTypeId)
                .NotNull().WithMessage("SubscriptionType Id cannot be null");

            RuleFor((x) => x.Cost)
                .NotNull().WithMessage("Cost cannot be null")
                .GreaterThanOrEqualTo(0).WithMessage("Cost cannot be negative");

            RuleFor((x) => x.BillingFrequency)
                .NotNull().WithMessage("BillingFrequency cannot be null")
                .GreaterThan(0).WithMessage("BillingFrequency must be greater than 0");

            RuleFor((x) => x.BillingPeriodUnit)
                .NotEmpty().WithMessage("BillingPeriodUnit cannot be null");

            RuleFor(x => x.PurchaseDate)
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Purchase date cannot be in the future.");

            RuleFor(x => x)
                .Must(x => x.RenewalDate > x.PurchaseDate).WithMessage("Renewal date must be greater than purchase date.");

            RuleFor(x => x.ActiveStatus)
                .IsInEnum().WithMessage("Invalid active status");
        }
    }
}


