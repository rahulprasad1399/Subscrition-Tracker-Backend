using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.SubscriptionTracker.Domain.Entity
{
    public class Service
    {
        public int Id { get; private set; }
        [Required, MaxLength(25)]
        public string ServiceName { get; private set; }
        public int CategoryId { get; private set; }
        public int? CreatedByUserId { get; private set; }
        public Category Category { get; private set; }
        public User User { get; set; }
        private Service()
        {
        }

        public Service(string serviceName, int categoryId, int? createdByUserId = null)
        {
            ServiceName = serviceName;
            CategoryId = categoryId;
            CreatedByUserId = createdByUserId;
        }
        public void UpdateService(string newServiceName, int newCategoryId)
        {
            ServiceName = newServiceName;
            CategoryId = newCategoryId;
        }
    }
    

}
