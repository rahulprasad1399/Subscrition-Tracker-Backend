using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.SubscriptionTracker.Domain.Entity
{
    public class Category
    {
        public int Id { get; private set; }
        [Required, MaxLength(25)]
        public string CategoryName { get; private set; }

        public Category(string categoryName)
        {
            CategoryName = categoryName;
        }
        private Category()
        {

        }
    }
}
