using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.Category.Command
{
    public class CreateCategoryCommand : IRequest<int>
    {
        public string CategoryName {  get; set; }
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
            return await _repository.AddAsync();
        }
    }
}
