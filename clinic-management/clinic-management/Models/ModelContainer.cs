using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace clinic_management.Models
{
    public class ModelContainer
    {
        public IEnumerable<Item> Medicine { get; set; }
        public IEnumerable<Item> Utensil { get; set; }
        public Item Item { get; set; }
        public IEnumerable<Supply> SupplyList { get; set; }
        public Supply Supply { get; set; }
    }
}