using System.Collections.Generic;

namespace Trackify.SubscriptionTracker.Application.DashBoard.Dtos
{
    public class DashboardDto
    {
        public InsightsDataDto InsightsData { get; set; }
        public SummaryCardDataDto SummaryCardData { get; set; }
        public NotificationDataDto NotficationData { get; set; }
        public UpcomingRenewalDataDto UpcomingRenewalData { get; set; }
    }

    // Upcoming Renewal
    public class UpcomingRenewalDataDto
    {
        public List<UpcomingRenewalDto> UpcomingRenewals { get; set; }
    }

    public class UpcomingRenewalDto
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int RenewalIn { get; set; }
    }

    // Notifications
    public class NotificationDataDto
    {
        public List<NotificationDetailsDto> Notifications { get; set; }
    }

    public class NotificationDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public bool IsRead { get; set; }
    }

    // Insights
    public class InsightsDataDto
    {
        public MonthlySpendsDto MonthlySpends { get; set; }
        public ActivityStatusDto ActiveStatus { get; set; }
        public CategoriesSummaryDto CategoriesSummary { get; set; }
    }

    public class MonthlySpendsDto
    {
        public List<decimal> MonthlySpends { get; set; }
    }

    public class ActivityStatusDto
    {
        public int Active { get; set; }
        public int Cancelled { get; set; }
        public int Paused { get; set; }
    }

    public class CategoriesSummaryDto
    {
        public List<CategoryWithSpendDto> CategoriesSummaryData { get; set; }
    }

    public class CategoryWithSpendDto
    {
        public string CategoryName { get; set; }
        public decimal Cost { get; set; }
    }

    // Summary Card Section
    public class SummaryCardDataDto
    {
        public decimal MonthlySpend { get; set; }
        public decimal YearlySpend { get; set; }
        public int ActiveNow { get; set; }
        public decimal AverageCostPerSub { get; set; }
    }
}
