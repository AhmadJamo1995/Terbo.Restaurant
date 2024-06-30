using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EfCore;
using Entities;
using AutoMapper;
using Dtos.Ingredient;
using Dtos.Customer;
using Dtos.LookUp;

namespace Terbo.Restaurant.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        #region Data and Const
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public IngredientsController(AppDbContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion
        #region Methods
        // GET: api/Ingredients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> GetIngredients()
        {
            var ingredients = await _context.Ingredients.ToListAsync();

            var ingredientDtos = _mapper.Map<List<IngredientDto>>(ingredients);

            return Ok(ingredientDtos);
        }

        // GET: api/Ingredients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientDto>> GetIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient == null)
            {
                return NotFound();
            }
            var ingredientDto = _mapper.Map<IngredientDto>(ingredient);

            return ingredientDto;
        }
        

            // PUT: api/Ingredients/5
            [HttpPut("{id}")]
         public async Task<IActionResult> PutIngredient(int id, IngredientDto ingredientDto)
        {
            if (id != ingredientDto.Id)
            {
                return BadRequest();
            }

            var ingredient = await _context.Ingredients.FindAsync(id);

            _mapper.Map(ingredientDto, ingredient);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientExists(id))
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

        // POST: api/Ingredients
        [HttpPost]
        public async Task<ActionResult<Ingredient>> PostIngredient(IngredientDto ingredientDto)
        {
            var ingredient = _mapper.Map<Ingredient>(ingredientDto);

            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Ingredients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientDto>> GetIngredientForEdit(int id)
        {
            var ingredient = await _context
                                    .Ingredients
                                    .Where(c => c.Id == id)
                                    .SingleOrDefaultAsync();

            if (ingredient == null)
            {
                return NotFound();
            }

            var ingredientDto = _mapper.Map<IngredientDto>(ingredient);

            return ingredientDto;
        }
        [HttpGet]
        public async Task<IEnumerable<LookUpDto>> GetIngredientLookup()
        {
            var ingredientLookup = await _context
                                        .Ingredients
                                        .Select(ingredient => new LookUpDto()
                                        {
                                            Id = ingredient.Id,
                                            Name = ingredient.Name
                                        })
                                        .ToListAsync();

            return ingredientLookup;
        }
        #endregion
        #region Private
        private bool IngredientExists(int id)
        {
            return _context.Ingredients.Any(e => e.Id == id);
        }
    }
    #endregion
}
