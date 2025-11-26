using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.Categories.Command
{
    public class DeleteCategoryCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, int>
    {
        private readonly Interface.IGenericRepository<Category> _repository;

        public DeleteCategoryCommandHandler(IGenericRepository<Category> repository)
        {
            _repository = repository;
        }
        public async Task<int> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync(request.Id);
        }
    }
    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        private readonly IGenericRepository<Category> _repository;
        public DeleteCategoryCommandValidator(IGenericRepository<Category> repository)
        {
            _repository = repository;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Missing category id");
        }
    }
}
