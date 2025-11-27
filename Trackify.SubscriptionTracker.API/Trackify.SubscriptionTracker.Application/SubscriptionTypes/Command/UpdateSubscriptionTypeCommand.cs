using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Exceptions;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.SubscriptionTypes.Command
{
    public class UpdateSubscriptionTypeCommand : IRequest<int>
    {
        [JsonIgnore]
        public int Id {  get; set; }
        public string TypeName { get; set; }
        public int CreatedByUserId { get; set; }
    }
    public class UpdateSubscriptionTypeCommandHandler : IRequestHandler<UpdateSubscriptionTypeCommand, int>
    {
        private readonly IGenericRepository<SubscriptionType> _repository;

        public UpdateSubscriptionTypeCommandHandler(IGenericRepository<SubscriptionType> repository)
        {
            _repository = repository;
        }
        public async Task<int> Handle(UpdateSubscriptionTypeCommand request, CancellationToken cancellationToken)
        {
            SubscriptionType subscriptionType = await _repository.GetByIdAsync(request.Id);
            if (subscriptionType ==null)
            {
                throw new NotFoundException(nameof(SubscriptionType),request.Id);
            }
            if (subscriptionType.CreatedByUserId != request.CreatedByUserId)
            {
                throw new ForbiddenAccessException("You are not authorized to modify this subscription.");
            }

            subscriptionType.UpdateTypeName(request.TypeName);
            return await _repository.SaveChangesAsync();    

        }
    }
    public class UpdateSubscriptionTypeCommandValidator : AbstractValidator<UpdateSubscriptionTypeCommand>
    {
        public UpdateSubscriptionTypeCommandValidator()
        {
            RuleFor(x => x.TypeName)
                .NotEmpty().WithMessage("Subscrption type name is required")
                .MaximumLength(25).WithMessage("Subscrption type name must be less than 25 charecters");
            RuleFor(x => x.CreatedByUserId)
                .GreaterThan(0).WithMessage("User Id is required");
        }
    }
}
