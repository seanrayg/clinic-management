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
        public IEnumerable<Item> CriticalStock { get; set; }
        public IEnumerable<Item> OutOfStock { get; set; }
        public Item Item { get; set; }
        public IEnumerable<Item> ItemList { get; set; }
        public IEnumerable<Supply> SupplyList { get; set; }
        public Supply Supply { get; set; }
        public MedCheck medcheck { get; set; }
        public IEnumerable<MedCheckItem> MedCheckItem { get; set; }
        public IEnumerable<ModelExpiredItem> ModelExpiredItem { get; set;}
    }
}