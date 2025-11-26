using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return await _repository.GetByIdAsync(request.Id);
        }
    }
    public class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery> 
    {
        private readonly IGenericRepository<Category> _repository;

        public GetCategoryByIdQueryValidator(IGenericRepository<Category> repository)
        {
            _repository = repository;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Missing category id");
            RuleFor(x => x.Id)
                .MustAsync(async (id, cancellation) =>
                {
                    var existing = await _repository.GetByIdAsync(id);
                    return existing != null;
                }).WithMessage("Category does not exist");
        }
    }
}
