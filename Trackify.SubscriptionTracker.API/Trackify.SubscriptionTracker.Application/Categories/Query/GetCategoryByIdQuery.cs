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

namespace Trackify.SubscriptionTracker.Application.Categories.Query
{
    public class GetCategoryByIdQuery : IRequest<Category>
    {
        public int Id { get; set; }
    }
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Category>
    {
        private readonly IGenericRepository<Category> _repository;

        public GetCategoryByIdQueryHandler(IGenericRepository<Category> repository)
        {
            _repository = repository;
        }
        public async Task<Category> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _repository.GetByIdAsync(request.Id);
            if(category == null)
                throw new NotFoundException(nameof(Category), request.Id);
            return category;
        }
    }
    public class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery> 
    {
        public GetCategoryByIdQueryValidator(IGenericRepository<Category> repository)
        {

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Missing category id");
        }
    }
}
