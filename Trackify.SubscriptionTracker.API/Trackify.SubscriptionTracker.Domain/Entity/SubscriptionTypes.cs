using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.SubscriptionTracker.Domain.Entity
{
    public class SubscriptionTypes
    {
        public int Id { get; private set; }
        [Required,MaxLength(25)]
        public string TypeName { get; private set; }
        public int? CreatedByUserId { get; private set; }

        public SubscriptionTypes(string typeName,int? createdByUserId = null) 
        {
            TypeName = typeName;
            CreatedByUserId = createdByUserId;
        }

        private SubscriptionTypes() { }
    }
}


