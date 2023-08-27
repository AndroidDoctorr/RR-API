using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterApi.Data;
using RestaurantRaterAPI.Data;


namespace RestaurantRaterApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RestaurantController : ControllerBase
{
    private readonly RestaurantDbContext _context;
    public RestaurantController(RestaurantDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetRestaurants()
    {
        if (_context.Restaurants == null) return NoContent();

        List<RestaurantListItem> restaurants = await _context.Restaurants
            .Include(r => r.Ratings)
            .Select(r => new RestaurantListItem()
            {
                Id = r.Id,
                Name = r.Name,
                Location = r.Location,
            })
            .ToListAsync();
        return Ok(restaurants);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRestaurantById(int id)
    {
        if (_context.Restaurants == null) return NoContent();

        Restaurant? restaurant = await _context.Restaurants
            .Include(r => r.Ratings)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (restaurant == null)
            return NotFound();

        RestaurantDetail restaurantDetail = new()
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Location = restaurant.Location,
            Ratings = restaurant.Ratings
                .Select(r => new RatingListItem()
                {
                    Id = r.Id,
                    Score = r.Score,
                }).ToList(),
        };
        return Ok(restaurantDetail);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant([FromForm] RestaurantEdit request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (_context.Restaurants == null) return NoContent();

        _context.Restaurants.Add(new Restaurant()
        {
            Name = request.Name,
            Location = request.Location,
        });

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRestaurant(int id, [FromForm] RestaurantEdit request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (_context.Restaurants == null) return NoContent();

        Restaurant? restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == id);

        if (restaurant == null) return NotFound();

        restaurant.Name = request.Name;
        restaurant.Location = request.Location;

        await _context.SaveChangesAsync();
        return Ok();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> RemoveRestaurant(int id)
    {
        if (_context.Restaurants == null) return NoContent();

        Restaurant? restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == id);

        if (restaurant == null) return NotFound();

        _context.Restaurants.Remove(restaurant);
        await _context.SaveChangesAsync();
        return Ok();
    }
}