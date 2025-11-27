using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.Services.Command
{
    public class CreateServiceCommand : IRequest<int>
    {
        public string ServiceName { get; set; }
        public int CategoryId { get; set; }
        public int CreatedByUserId { get; set; }
    }
    public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, int>
    {
        private readonly IGenericRepository<Service> _repository;

        public CreateServiceCommandHandler(IGenericRepository<Service> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            Service service = new Service(request.ServiceName,request.CategoryId,request.CreatedByUserId);
            await _repository.AddAsync(service, cancellationToken);
            return service.Id;
        }
    }

    public class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
    {
        private readonly IGenericRepository<Category> _categoryRepo;
        private readonly IGenericRepository<User> _userRepo;

        public CreateServiceCommandValidator(IGenericRepository<Category> categoryRepo,IGenericRepository<User> userRepo)
        {
            _categoryRepo = categoryRepo;
            _userRepo = userRepo;
            RuleFor(x => x.ServiceName)
                .NotEmpty().WithMessage("Service name is required")
                .MaximumLength(25).WithMessage("Service name must be less than 25 charecters");
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category Id is required")
                .MustAsync(async (categoryId, cancellation) =>
                {
                    return await _categoryRepo.ExistsAsync(categoryId, cancellation);
                }).WithMessage("Category doesnt exist");
            RuleFor(x => x.CreatedByUserId)
                .GreaterThan(0).WithMessage("User Id is required")
                .MustAsync(async (userId, cancellation) =>
                {
                    return await _userRepo.ExistsAsync(userId, cancellation);

                }).WithMessage("User doesn't exist");

        }
    }
}
