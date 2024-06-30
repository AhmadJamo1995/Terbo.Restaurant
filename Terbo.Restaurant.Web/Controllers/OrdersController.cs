using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EfCore;
using Entities;
using AutoMapper;
using Dtos.Order;

namespace Terbo.Restaurant.Web.Controllers
{
    #region Data and Const
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrdersController(AppDbContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion
        #region Methods
        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var order = await _context
                              .Orders
                              .Include(o => o.Meals)
                              .Include(o => o.Customer)
                              .ToListAsync();
            var orderDtos = _mapper.Map<List<OrderDto>>(order);

            return Ok(orderDtos);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }
            order.TotalPrice = GetOrderTotalPrice(order.Meals);

            var orderDto = _mapper.Map<OrderDetailsDto>(order);

            return Ok(orderDto);
        }

        

        // PUT: api/Orders/
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, CreateUpdateOrderDto createUpdateOrderDto)
        {
            if (id != createUpdateOrderDto.Id)
            {
                return BadRequest();
            }

            var order = await _context.Orders.FindAsync(id);

            _mapper.Map(createUpdateOrderDto, order);

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
                                    .Where(o => o.Id == id)
                                    .SingleOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            var orderDto = _mapper.Map<CreateUpdateOrderDto>(order);

            return orderDto;
        }
        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(CreateUpdateOrderDto createUpdateOrderDto)
        {
            var order = _mapper.Map<Order>(createUpdateOrderDto);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Orders/5
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

        private decimal GetOrderTotalPrice(List<Meal> meals)
        {
            return meals.Sum(m => m.Price);
        }

        #endregion
    }
}
