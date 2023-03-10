namespace SyncR.Core;
public class SyncMessage<T>
{
    public Guid Id { get; set; }
    public Guid Key { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public SyncActionType Action { get; set; } = SyncActionType.Push;
}