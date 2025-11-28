using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.SubscriptionTracker.Application.Dtos
{
    public class SubscriptionTypeDto
    {
        public int Id { get;  set; }
        public string TypeName { get;  set; }
        public int? CreatedByUserId { get; set; }
    }
}
