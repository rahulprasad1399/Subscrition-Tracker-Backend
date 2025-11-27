using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.SubscriptionTypes.Command
{
    public class CreateSubscriptionTypeCommand : IRequest<int>
    {
        public string TypeName { get; set; }
        [JsonIgnore]
        public int CreatedByUserId { get; set; }
    }

    public class CreateSubscriptionTypeCommandHandler : IRequestHandler<CreateSubscriptionTypeCommand, int>
    {
        private readonly IGenericRepository<SubscriptionType> _repository;

        public CreateSubscriptionTypeCommandHandler(IGenericRepository<SubscriptionType> repository)
        {
            _repository = repository;
        }
        public async Task<int> Handle(CreateSubscriptionTypeCommand request, CancellationToken cancellationToken)
        {
            SubscriptionType subscriptionType = new(request.TypeName,request.CreatedByUserId);
            await _repository.AddAsync(subscriptionType,cancellationToken);
            return subscriptionType.Id;
        }
    }
    public class CreateSubscriptionTypeCommandValidator : AbstractValidator<CreateSubscriptionTypeCommand> 
    {
        private readonly IGenericRepository<User> _repository;

        public CreateSubscriptionTypeCommandValidator(IGenericRepository<User> repository)
        {
            _repository = repository;

            RuleFor(x => x.TypeName)
                .NotEmpty().WithMessage("Subscrption type name is required")
                .MaximumLength(25).WithMessage("Subscrption type name must be less than 25 charecters");
            RuleFor(x => x.CreatedByUserId)
                .GreaterThan(0).WithMessage("User Id is required")
                .MustAsync(async (userId, cancellation) =>
                {
                    return await _repository.ExistsAsync(userId, cancellation);
                }).WithMessage("User doesn't exist");
        }
    }
}
