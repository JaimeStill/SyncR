using SyncR.Core;

namespace SyncR.Client;
public interface ISyncConnection<T> : IAsyncDisposable
{
    Task Connect();
    Task RegisterService();
    Task RegisterListener();
    Task Join(Guid key);
    Task Leave(Guid key);
    Task Push(SyncMessage<T> message);
    Task Notify(SyncMessage<T> message);
    Task Complete(SyncMessage<T> message);
    Task Return(SyncMessage<T> message);
    Task Reject(SyncMessage<T> message);
}