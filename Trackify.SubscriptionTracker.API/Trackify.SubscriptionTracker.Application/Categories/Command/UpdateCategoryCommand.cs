using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Exceptions;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.Categories.Command
{
    public class UpdateCategoryCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, int>
    {
        private readonly IGenericRepository<Category> _repository;

        public UpdateCategoryCommandHandler(IGenericRepository<Category> repository)
        {
            _repository = repository;
        }
        public async Task<int> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repository.GetByIdAsync(request.Id);

            if (category == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            category.Update(request.CategoryName);

            return await _repository.SaveChangesAsync();

        }
    }
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        private readonly IGenericRepository<Category> _repository;
        public UpdateCategoryCommandValidator(IGenericRepository<Category> repository)
        {
            _repository= repository;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Missing category id");
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is missing");
        }
    }
}
