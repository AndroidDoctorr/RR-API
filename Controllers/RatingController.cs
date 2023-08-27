using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterApi.Data;
using RestaurantRaterAPI.Data;

namespace RestaurantRaterAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class RatingController : ControllerBase
{
    private readonly RestaurantDbContext _context;
    public RatingController(RestaurantDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRatings()
    {
        if (_context.Ratings == null) return NoContent();

        List<Rating> ratings = await _context.Ratings.ToListAsync();
        List<RatingDetail> ratingDetails = ratings
            .Select(r => new RatingDetail()
            {
                Restaurant = r.Restaurant?.Name,
                Id = r.Id,
                Score = r.Score,
            }).ToList();

        return Ok(ratings);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRatingsForRestaurant(int id)
    {
        if (_context.Ratings == null) return NoContent();

        List<Rating> ratings = await _context
            .Ratings.Where(r => r.RestaurantId == id).ToListAsync();
        List<RatingListItem> ratingList = ratings
            .Select(r => new RatingListItem()
            {
                Id = r.Id,
                Score = r.Score,
            }).ToList();

        return Ok(ratings);
    }
    [HttpPost]
    public async Task<IActionResult> RateRestaurant(RatingEdit request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (_context.Ratings == null) return NoContent();

        _context.Ratings.Add(new Rating()
        {
            RestaurantId = request.RestaurantId,
            Score = request.Score,
        });
        await _context.SaveChangesAsync();

        return Ok();
    }
}