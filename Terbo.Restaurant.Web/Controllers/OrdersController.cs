using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EfCore;
using Entities;
using AutoMapper;
using Dtos.Order;

namespace Terbo.Restaurant.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        #region Data and Const

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrdersController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Actions

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var order = await _context
                              .Orders
                              .Include(o => o.Customer)
                              .ToListAsync();

            var orderDtos = _mapper.Map<List<OrderDto>>(order);

            return Ok(orderDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context
                             .Orders
                             .Include(o => o.Customer)
                             .Include(o => o.Meals)
                             .Where(o => o.Id == id)
                             .SingleOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }


            var orderDto = _mapper.Map<OrderDetailsDto>(order);

            return Ok(orderDto);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateUpdateOrderDto createUpdateOrderDto)
        {
            var order = _mapper.Map<Order>(createUpdateOrderDto);

            await UpdateOrderMeals(order, createUpdateOrderDto.MealIds);

            order.TotalPrice = GetTotalPrice(order);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditOrder(int id, CreateUpdateOrderDto createUpdateOrderDto)
        {
            if (id != createUpdateOrderDto.Id)
            {
                return BadRequest();
            }

            var order = await _context
                                .Orders
                                .Include(o => o.Meals)
                                .Where(o => o.Id == id)
                                .SingleOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            _mapper.Map(createUpdateOrderDto, order);

            await UpdateOrderMeals(order, createUpdateOrderDto.MealIds);

            order.TotalPrice = GetTotalPrice(order);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CreateUpdateOrderDto>> GetOrderForEdit(int id)
        {
            var order = await _context
                                    .Orders
                                    .Include(o => o.Meals)
                                    .Where(o => o.Id == id)
                                    .SingleOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            var orderDto = _mapper.Map<CreateUpdateOrderDto>(order);

            return orderDto;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region Private
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        private async Task UpdateOrderMeals(Order order, List<int> mealIds)
        {
            order.Meals.Clear();

            var meals = await _context
                                .Meals
                                .Where(i => mealIds.Contains(i.Id))
                                .ToListAsync();

            order.Meals.AddRange(meals);
        }

        private decimal GetTotalPrice(Order order)
        {
            var totalPrice = order.Meals.Sum(e => e.Price);
            var totalPriceWithTax = totalPrice * 1.4m;
            return totalPriceWithTax;
        }

        #endregion
    }
}
