using Common;
using SyncR.Server;

namespace Sync.Hubs;
public class ProcessorHub : SyncHub<Package>
{
    public ProcessorHub(SyncServiceManager manager) : base(manager) { }
}