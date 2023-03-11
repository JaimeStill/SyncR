using Common;
using SyncR.Client;
using SyncR.Core;

namespace SyncCli.Clients;
public class ListenerConnection : SyncConnection<Package>
{
    public ListenerConnection(string endpoint) : base(endpoint)
    {
        OnPush.Set<SyncMessage<Package>>(Output);
        OnNotify.Set<SyncMessage<Package>>(Output);
        OnComplete.Set<SyncMessage<Package>>(Output);
        OnReturn.Set<SyncMessage<Package>>(Output);
        OnReject.Set<SyncMessage<Package>>(Output);
        OnUnavailable.Set<SyncMessage<Package>>(Output);
    }

    static void Output(SyncMessage<Package> message)
    {
        Console.ForegroundColor = GetMessageColor(message.Action);
        Console.WriteLine($"{message.Action}: {message.Key}");
        Console.WriteLine(message.Message);
        Console.ResetColor();
    }

    static ConsoleColor GetMessageColor(SyncActionType action) => action switch
    {
        SyncActionType.Push => ConsoleColor.Cyan,
        SyncActionType.Notify => ConsoleColor.Yellow,
        SyncActionType.Complete => ConsoleColor.Green,
        SyncActionType.Return => ConsoleColor.Magenta,
        SyncActionType.Reject => ConsoleColor.Red,
        _ => Console.ForegroundColor
    };
}