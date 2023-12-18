namespace ToEat.Domain.Models;

public class Inventory
{
    public int Id { get; set; }
    
    public string UserId { get; set; }
    public User User { get; set; }
    public ICollection<InventoryItem>? InventoryItems { get; set; }
}
