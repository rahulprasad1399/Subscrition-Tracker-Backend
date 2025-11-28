using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Trackify.SubscriptionTracker.Domain.Entity.Subscription;

namespace Trackify.SubscriptionTracker.Application.SubscriptionGetByIdDtos
{
    public class SubscriptionGetByIdDto
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int subscriptionTypeId { get; set; }
        public string SubscriptionTypeName { get; set; }
        public int categoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Cost { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime RenewalDate { get; set; }
        public ActiveStatus Status { get; set; }
        public int BillingFrequency { get; set; }
        public string BillingPeriodUnit { get; set; }
    }
}
