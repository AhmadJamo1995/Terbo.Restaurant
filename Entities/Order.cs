using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderTime { get; set; } = DateTime.Now;


        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }
        public string? Notes { get; set; }


        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public List<Meal> Meals { get; set; } = [];
    }
}
