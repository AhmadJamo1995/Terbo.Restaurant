using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Order
{
    public class CreateUpdateOrderDto
    {
        public int Id { get; set; }
        public string? Notes { get; set; }
        public int CustomerId { get; set; }
        public List<int> MealIds { get; set; } = [];
 
    }
}
