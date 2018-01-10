using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace clinic_management.Models
{
    public class ModelContainer
    {
        public IEnumerable<Item> ItemList { get; set; }
        public Item Item { get; set; }
    }
}