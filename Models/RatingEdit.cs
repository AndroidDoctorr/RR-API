using System.ComponentModel.DataAnnotations;

public class RatingEdit
{
    [Required]
    public int RestaurantId { get; set; }
    [Required]
    [Range(1, 5)]
    public float Score { get; set; }
}