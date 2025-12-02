using MediatR;
using Microsoft.EntityFrameworkCore;
using Trackify.SubscriptionTracker.Application.DashBoard.Dtos;
using Trackify.SubscriptionTracker.Infrastructure.Data;

namespace Trackify.SubscriptionTracker.Application.DashBoard.Query
{
    public class GetAllDashBoardDataQuery : IRequest<DashboardDto>
    {
        public int UserId { get; set; }
        public int Year { get; set; } = DateTime.UtcNow.Year;
    }

    public class GetAllDashBoardDataQueryHandler : IRequestHandler<GetAllDashBoardDataQuery, DashboardDto>
    {
        private readonly IApplicationDbContext _context;

        public GetAllDashBoardDataQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> Handle(GetAllDashBoardDataQuery request, CancellationToken cancellationToken)
        {
            
            var allSubs = await _context.Subscriptions
                .Include(s => s.Service)
                    .ThenInclude(svc => svc.Category)
                .Where(x => x.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            var monthlyData = allSubs
                .Where(s => s.PurchaseDate.Year == request.Year)
                .GroupBy(s => s.PurchaseDate.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(s => s.Cost) })
                .ToList();

            var monthlySpendsArray = new decimal[12];
            foreach (var m in monthlyData)
            {
                monthlySpendsArray[m.Month - 1] = m.Total;
            }

            var statusDto = new ActivityStatusDto
            {
                Active = allSubs.Count(x => x.Status == Domain.Entity.Subscription.ActiveStatus.Active),
                Paused = allSubs.Count(x => x.Status == Domain.Entity.Subscription.ActiveStatus.Paused),
                Cancelled = allSubs.Count(x => x.Status == Domain.Entity.Subscription.ActiveStatus.Cancelled)
            };

            var categoryDto = allSubs
                .GroupBy(x => x.Service.Category.CategoryName)
                .Select(g => new CategoryWithSpendDto
                {
                    CategoryName = g.Key,
                    Cost = g.Sum(x => x.Cost)
                })
                .ToList();

            var today = DateTime.UtcNow.Date;
            var renewalsDto = allSubs
                .Where(s => s.Status == Domain.Entity.Subscription.ActiveStatus.Active
                            && s.RenewalDate >= today
                            && s.RenewalDate <= today.AddDays(7)) 
                .OrderBy(s => s.RenewalDate)
                .Select(s => new UpcomingRenewalDto
                {
                    Id = s.Id,
                    ServiceId = s.ServiceId,
                    ServiceName = s.Service.ServiceName, 
                    RenewalIn = (s.RenewalDate.Date - today).Days
                })
                .ToList();

            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;

            var summaryDto = new SummaryCardDataDto
            {
                MonthlySpend = allSubs
                    .Where(s => s.PurchaseDate.Month == currentMonth && s.PurchaseDate.Year == currentYear)
                    .Sum(s => s.Cost),
                YearlySpend = allSubs
                    .Where(s => s.PurchaseDate.Year == currentYear)
                    .Sum(s => s.Cost),
                ActiveNow = statusDto.Active,
                AverageCostPerSub = allSubs.Any() ? allSubs.Average(s => s.Cost) : 0
            };

            return new DashboardDto
            {
                SummaryCardData = summaryDto,
                InsightsData = new InsightsDataDto
                {
                    MonthlySpends = new MonthlySpendsDto { MonthlySpends = monthlySpendsArray.ToList() },
                    ActiveStatus = statusDto,
                    CategoriesSummary = new CategoriesSummaryDto
                    {
                        CategoriesSummaryData = categoryDto
                    }
                },
                UpcomingRenewalData = new UpcomingRenewalDataDto
                {
                    UpcomingRenewals = renewalsDto
                },
                NotficationData = null 
            };
        }
    }
}