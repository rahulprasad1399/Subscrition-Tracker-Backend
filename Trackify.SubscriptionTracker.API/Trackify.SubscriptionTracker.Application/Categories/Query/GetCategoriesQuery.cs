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
    public class GetCategoriesQuery  : IRequest<IEnumerable<Category>>
    {
    }
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<Category>>
    {
        private readonly IGenericRepository<Category> _repository;

        public GetCategoriesQueryHandler(IGenericRepository<Category> repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
