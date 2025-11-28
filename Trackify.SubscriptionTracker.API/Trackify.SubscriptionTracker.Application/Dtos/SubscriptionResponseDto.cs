using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.SubscriptionGetAllDto;

namespace Trackify.SubscriptionTracker.Application.Dtos
{
    public class SubscriptionResponseDto
    {
        public int TotalItem { get; set; }
        public int ActiveItem { get; set; }
        public decimal MoneySpentMonthly { get; set; }
        public List<SubscriptionGetDto> Subscriptions { get; set; }
    }

}
