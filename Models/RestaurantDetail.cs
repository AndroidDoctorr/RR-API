public class RestaurantDetail
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public List<RatingListItem> Ratings = new();
    public double AverageScore => Ratings.Average(r => r.Score);
}