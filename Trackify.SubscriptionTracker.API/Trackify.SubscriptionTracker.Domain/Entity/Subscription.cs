using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.SubscriptionTracker.Domain.Entity
{
    public class Subscription
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int ServiceId { get; private set; }
        public int SubscriptionTypeId { get; private set; }
        public decimal Cost { get; private set; }
        public int BillingFrequency { get; private set; }
        [Required, MaxLength(25)]
        public string BillingPeriodUnit { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public DateTime RenewalDate { get; private set; }
        public ActiveStatus Status { get; private set; }

        public Subscription(int userId, int serviceId, int subscriptionTypeId, decimal cost, int billingFrequency, string billingPeriodUnit, DateTime purchaseDate, DateTime renewalDate, ActiveStatus status)
        {
            UserId = userId;
            ServiceId = serviceId;
            SubscriptionTypeId = subscriptionTypeId;
            Cost = cost;
            BillingFrequency = billingFrequency;
            BillingPeriodUnit = billingPeriodUnit;
            PurchaseDate = purchaseDate;
            RenewalDate = renewalDate;
            Status = status;
        }

        public void UpdateSubscription(int serviceId, int subscriptionTypeId, decimal cost, int billingFrequency,
            string billingPeriodUnit, DateTime purchaseDate, DateTime renewalDate, ActiveStatus activeStatus)
        {
            ServiceId = serviceId;
            SubscriptionTypeId = subscriptionTypeId;
            Cost = cost;
            BillingFrequency = billingFrequency;
            BillingPeriodUnit = billingPeriodUnit;
            PurchaseDate = purchaseDate;
            RenewalDate = renewalDate;
            Status = activeStatus;

        }

        public void UpdateStatus(ActiveStatus activeStatus)
        {
            Status = activeStatus;
        }

        public enum ActiveStatus
        {
            Active,
            Cancelled,
            Paused
        }

        private Subscription() { }
    }
}
