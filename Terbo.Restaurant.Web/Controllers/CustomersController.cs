using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EfCore;
using Entities;
using AutoMapper;
using Dtos.Customer;
using Dtos.LookUp;

namespace Terbo.Restaurant.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        #region Data and Const
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CustomersController(AppDbContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion
        #region Methods
        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            var customer = await _context
                         .Customers
                         .ToListAsync();
            var customerDto = _mapper.Map<List<CustomerDto>>(customer);

            return Ok(customerDto);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDetailsDto>> GetCustomer(int id)
        {
            var customer = await _context
                                 .Customers
                                 .FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }
            var customerDetailsDto = _mapper.Map<Customer, CustomerDetailsDto>(customer);

            return customerDetailsDto;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CreateUpdateCustomerDto>> GetCustomerForEdit(int id)
        {
            var customer = await _context
                                    .Customers
                                    .Where(c => c.Id == id)
                                    .SingleOrDefaultAsync();

            if (customer == null)
            {
                return NotFound();
            }

            var customerDto = _mapper.Map<CreateUpdateCustomerDto>(customer);

            return customerDto;
        }
        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CreateUpdateCustomerDto createUpdateCustomerDto)
        {
            if (id != createUpdateCustomerDto.Id)
            {
                return BadRequest();
            }

            var customer = await _context.Customers.FindAsync(id);

            _mapper.Map(createUpdateCustomerDto, customer);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(CreateUpdateCustomerDto createUpdateCustomerDto)
        {
            var customer = _mapper.Map<Customer>(createUpdateCustomerDto);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet]
        public async Task<IEnumerable<LookUpDto>> GetCustomerLookup()
        {
            var customerLookup = await _context
                                        .Customers
                                        .Select(customer => new LookUpDto()
                                        {
                                            Id = customer.Id,
                                            Name = customer.FullName
                                        })
                                        .ToListAsync();

            return customerLookup;
        }
        #endregion
        #region Private
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
    #endregion
}
