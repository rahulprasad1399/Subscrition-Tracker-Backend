using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.SubscriptionTracker.Application.NotificationDto
{
    public class UserNotificationGetDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }    
        public int SubscriptionId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }

    }
}
