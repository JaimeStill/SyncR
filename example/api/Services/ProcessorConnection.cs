using Common;
using SyncR.Client;
using SyncR.Core;

namespace Api.Services;
public class ProcessorConnection : SyncConnection<Package>
{
    public ProcessorConnection(IConfiguration config) : base(
        config.GetValue<string>("SyncR:ProcessorUrl") ?? "http://localhost:5000/processor"
    ) { }

    public async Task<bool> SendPackage(Package package)
    {
        await Connect();
        await Join(package.Key);

        SyncMessage<Package> message = new()
        {
            Id = Guid.NewGuid(),
            Key = package.Key,
            Data = package,
            Message = $"Initializing package {package.Name} for processing"
        };

        await Push(message);
        await Leave(package.Key);

        return true;
    }
}