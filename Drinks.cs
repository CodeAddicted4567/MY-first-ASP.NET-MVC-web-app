using System.ComponentModel.DataAnnotations;

namespace DrinksStoreManage.Models;

public class Drinks
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(20)]
    public string? Name { get; set; }
    
    [Required]
    public long Qty { get; set; }
    
    [Required]
    public string? Type { get; set; }
}
