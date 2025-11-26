using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.Users.Command
{
    public class DeleteUserCommand : IRequest<int>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, int>
    {
        private readonly IGenericRepository<User> _repository;
        public DeleteUserCommandHandler(IGenericRepository<User> repository)
        {
            _repository = repository;
        }
        public async Task<int> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync(request.Id);
        }
    }

    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator(IGenericRepository<User> repository)
        {
            RuleFor(x => x.Id).NotNull().WithMessage("Id is required")
        .GreaterThan(0).WithMessage("Provide a valid id");

            RuleFor(x => x.Id).MustAsync(async (id, cancellation) =>
            {
                var user = await repository.GetByIdAsync(id);
                return user != null;
            }).WithMessage("User not found");
        }

    }
}
