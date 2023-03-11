using Common;
using SyncR.Client;
using SyncR.Core;

namespace SyncCli.Clients;
public class ProcessorConnection : SyncConnection<Package>
{
    public ProcessorConnection(string endpoint) : base(endpoint)
    {
        OnPush.Set<SyncMessage<Package>>(Output);
        OnNotify.Set<SyncMessage<Package>>(Output);
    }

    static void Output(SyncMessage<Package> message) =>
        Console.WriteLine(message.Message);
}