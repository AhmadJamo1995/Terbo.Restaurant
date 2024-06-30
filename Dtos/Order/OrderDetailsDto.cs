using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dtos.Meal;

namespace Dtos.Order
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public DateTime OrderTime { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }
        public string Notes { get; set; }
        public int CustomerId { get; set; }
        public List<MealDto> Meals { get; set; }
    }
}
