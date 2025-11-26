using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;
using Trackify.SubscriptionTracker.Infrastructure.Data.Reposetories;

namespace Trackify.SubscriptionTracker.Application.Categories.Command
{
    public class CreateCategoryCommand : IRequest<int>
    {
        public string CategoryName { get; set; }
    }
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly IGenericRepository<Category> _repository;

        public CreateCategoryCommandHandler(IGenericRepository<Category> repository)
        {
            _repository = repository;
        }
        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category(request.CategoryName);
            return await _repository.AddAsync(category);
        }
    }
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        private readonly ICategoryRepository _repository;
        public CreateCategoryCommandValidator(ICategoryRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required")
                .MaximumLength(25).WithMessage("Category name must be less than 25 charecters");
            RuleFor(x => x.CategoryName)
                .MustAsync(async (name, cancellation) =>
                {
                    var existing = await _repository.IsNameUniqueAsync(name);
                    return !existing;
                }).WithMessage("Category already exists");

        }
    }

}
