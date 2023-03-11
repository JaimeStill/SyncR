namespace Common;
public static class Packages
{
    public static Package Approval() =>
        new()
        {
            Key = Guid.NewGuid(),
            Name = "Approval Package",
            Intent = Intent.Approve,
            Resources = GenerateResources()
        };

    public static Package Acquisition() =>
        new()
        {
            Key = Guid.NewGuid(),
            Name = "Acquisition Package",
            Intent = Intent.Acquire,
            Resources = GenerateResources()
        };

    public static Package Transfer() =>
        new()
        {
            Key = Guid.NewGuid(),
            Name = "Transfer Package",
            Intent = Intent.Transfer,
            Resources = GenerateResources()
        };

    public static Package Destruction() =>
        new()
        {
            Key = Guid.NewGuid(),
            Name = "Destruction Package",
            Intent = Intent.Destroy,
            Resources = GenerateResources()
        };

    public static List<Resource> GenerateResources() => new()
    {
        new() {
            Key = Guid.NewGuid(),
            Name = "Training Plan"
        },
        new() {
            Key = Guid.NewGuid(),
            Name = "R&D Proposal"
        }
    };
}