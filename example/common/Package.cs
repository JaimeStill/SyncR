namespace Common;
public class Package
{
    public Guid Key { get; set; }
    public string Name { get; set; } = string.Empty;
    public Intent Intent { get; set; }

    public List<Resource> Resources { get; set; } = new();
}