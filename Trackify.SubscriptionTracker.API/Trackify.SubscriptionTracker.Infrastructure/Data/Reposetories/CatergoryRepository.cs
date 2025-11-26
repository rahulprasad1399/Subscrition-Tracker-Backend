using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.SubscriptionTracker.Infrastructure.Data.Reposetories
{
    public class CatergoryRepository : ICatergoryRepository
    {
        private readonly IApplicationDbContext _context;
        public CatergoryRepository(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AnyAsync(string categoryName)
        {
            return await _context.Categories.AnyAsync(x=>x.CategoryName== categoryName);
        }
    }
}
