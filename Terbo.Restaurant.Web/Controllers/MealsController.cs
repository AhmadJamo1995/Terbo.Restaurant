using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EfCore;
using Entities;
using Dtos.Meal;
using AutoMapper;
using Dtos.LookUp;

namespace Terbo.Restaurant.Web.Controllers
{
    #region Data and Const
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MealsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MealsController(AppDbContext context , IMapper mapper)
        {
            _context = context;
           _mapper = mapper;
        }
        #endregion
        #region Methods
        // GET: api/Meals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealDto>>> GetMeals()
        {
           var meal = await _context
                            .Meals
                            .ToListAsync();

            var mealDtos = _mapper.Map<List<MealDto>>(meal);
            
            return Ok(mealDtos);
        }

        // GET: api/Meals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MealDetailsDto>> GetMeal(int id)
        {
            var meal = await _context
                             .Meals
                             .Include( m => m.Ingredients)
                             .Where(o => o.Id == id)
                             .SingleOrDefaultAsync();

            if (meal == null)
            {
                return NotFound();
            }
            var MealDto = _mapper.Map<MealDetailsDto>(meal);

            return MealDto;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CreateUpdateMealDto>> GetMealForEdit(int id)
        {
            var meal = await _context
                                    .Meals
                                    .Where(m => m.Id == id)
                                    .SingleOrDefaultAsync();

            if (meal == null)
            {
                return NotFound();
            }

            var mealDto = _mapper.Map<CreateUpdateMealDto>(meal);

            return mealDto;
        }

        // PUT: api/Meals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeal(int id, CreateUpdateMealDto createUpdateMealDto)
        {
            if (id != createUpdateMealDto.Id)
            {
                return BadRequest();
            }

            var meal = await _context.Meals.FindAsync(id);

            _mapper.Map(createUpdateMealDto, meal);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MealExists(id))
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

        // POST: api/Meals
        [HttpPost]
        public async Task<ActionResult<Meal>> PostMeal(CreateUpdateMealDto createUpdateMealDto)
        {
            var meal = _mapper.Map<Meal>(createUpdateMealDto);
            await UpdateMealIngredients(meal, createUpdateMealDto.IngredientIds);
             meal.Price = await GetMealPriceAsync(createUpdateMealDto.IngredientIds);
            _context.Meals.Add(meal);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Meals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeal(int id)
        {
            var meal = await _context.Meals.FindAsync(id);
            if (meal == null)
            {
                return NotFound();
            }

            _context.Meals.Remove(meal);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet]
        public async Task<IEnumerable<LookUpDto>> GetMealLookup()
        {
            var mealLookup = await _context
                                        .Meals
                                        .Select(meal => new LookUpDto()
                                        {
                                            Id = meal.Id,
                                            Name = meal.Name
                                        })
                                        .ToListAsync();

            return mealLookup;
        }
        #endregion
        #region Private
        private bool MealExists(int id)
        {
            return _context.Meals.Any(e => e.Id == id);
        }

        private async Task UpdateMealIngredients(Meal meal, List<int> ingredientIds)
        {
            meal.Ingredients.Clear();

            var ingredients = await _context
                                        .Ingredients
                                        .Where(i => ingredientIds.Contains(i.Id))
                                        .ToListAsync();

            meal.Ingredients.AddRange(ingredients);
        }

        private async Task<decimal> GetMealPriceAsync(List<int> ingredientIds)
        {
            var ingredients = await _context
                                        .Ingredients
                                        .Where(i => ingredientIds.Contains(i.Id))
                                        .ToListAsync();

            var ingredientsPrice = ingredients.Sum(i => i.Price);

            return ingredientsPrice;
        }

        #endregion
    }
}
