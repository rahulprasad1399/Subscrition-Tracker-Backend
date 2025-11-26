using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Infrastructure.Data.Reposetories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly IApplicationDbContext _context;
        public CategoryRepository(IApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> IsNameUniqueAsync(string categoryName)
        {
            return await _context.Categories.AnyAsync(x=>x.CategoryName== categoryName);
        }
    }
}
