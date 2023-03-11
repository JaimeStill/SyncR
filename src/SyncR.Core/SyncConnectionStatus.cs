namespace SyncR.Core;
public class SyncConnectionStatus
{
    public string? ConnectionId { get; private set; }
    public string State { get; private set; }

    public SyncConnectionStatus(string? connectionId, string state)
    {
        ConnectionId = connectionId;
        State = state;
    }
}