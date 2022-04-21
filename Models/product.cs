using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cosmeticApp.Models
{
    public class product
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public string description { get; set; }
        public int productquantity { get; set; }
        public int price { get; set; }
        public string productfile { get; set; }
        public int exhibitId { get; set; }
    }
}
