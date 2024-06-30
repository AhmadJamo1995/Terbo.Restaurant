using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Meal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public List<Ingredient> Ingredients { get; set; } = [];
        public List<Order> Orders { get; set; }
    }
}