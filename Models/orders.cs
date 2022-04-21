using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cosmeticApp.Models
{
    public class orders
    {
        public int ordersId { get; set; }
        public int ProductId { get; set; }
        public int Userid { get; set; }
        public int noofproduct { get; set; }
        public DateTime orderdate { get; set; }


    }
}
