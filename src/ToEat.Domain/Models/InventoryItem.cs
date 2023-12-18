namespace ToEat.Domain.Models;
public class InventoryItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float Quantity { get; set; }
    public string MeasurementUnit { get; set; }
    public int InventoryId { get; set; }
    public Inventory Inventory { get; set; }
}
