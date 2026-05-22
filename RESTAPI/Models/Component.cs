namespace RESTAPI.Models;

public class Component
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int ComponentManufacturersId { get; set; }
    public int ComponentTypesId { get; set; }

    public ComponentManufacturer Manufacturer { get; set; } = null!;
    public ComponentType Type { get; set; } = null!;
    public ICollection<PCComponent> PCComponents { get; set; } = new List<PCComponent>();
}