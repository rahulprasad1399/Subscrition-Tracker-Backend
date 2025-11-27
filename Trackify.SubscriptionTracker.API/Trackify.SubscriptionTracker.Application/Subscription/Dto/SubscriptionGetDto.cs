using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Trackify.SubscriptionTracker.Domain.Entity.Subscription;

namespace Trackify.SubscriptionTracker.Application.SubscriptionDto
{
    public class SubscriptionGetDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public int SubscriptionTypeId { get; set; }
        public decimal Cost { get; set; }
        public int BillingFrequency { get; set; }
        public string BillingPeriodUnit { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime RenewalDate { get; set; }
        public ActiveStatus Status { get; set; }
    }
}

