using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostPerWear.Models
{
    public class ClothingItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public int Wears { get; set; }
        public double CostPerWear { get; set; }
        public ClothingItem()
        {

        }


    }
}
