using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class WishListItem
    {
        public Guid? Id { get; set; }

        public String Name { get; set; }

        public String Type => typeof(WishListItem).Name;
    }
}
