using System.ComponentModel.DataAnnotations;

public class RestaurantEdit
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string Location { get; set; } = string.Empty;
}