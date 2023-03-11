namespace SyncR.Core;
public class SyncServiceStatus
{
    public string? ConnectionId { get; private set; }
    public string State { get; private set; }

    public SyncServiceStatus(string? connectionId, string state)
    {
        ConnectionId = connectionId;
        State = state;
    }
}