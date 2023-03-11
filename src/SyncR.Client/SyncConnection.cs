using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.SignalR.Client;
using SyncR.Core;

namespace SyncR.Client;
public abstract class SyncConnection<T> : ISyncConnection<T>
{
    protected readonly HubConnection connection;
    protected readonly string endpoint;
    protected CancellationToken token;

    protected List<Guid> Groups { get; set; }

    public bool Available { get; private set; }

    protected SyncAction OnAvailable { get; private set; }
    protected SyncAction OnDisconnected { get; private set; }

    public SyncAction OnFinalizeService { get; protected set; }
    public SyncAction OnFinalizeListener { get; protected set; }
    public SyncAction OnPush { get; protected set; }
    public SyncAction OnNotify { get; protected set; }
    public SyncAction OnComplete { get; protected set; }
    public SyncAction OnReturn { get; protected set; }
    public SyncAction OnReject { get; protected set; }
    public SyncAction OnUnavailable { get; protected set; }

    public SyncConnection(string endpoint)
    {
        this.endpoint = endpoint;
        Groups = new();
        token = new();

        Console.WriteLine($"Building Sync connection at {endpoint}");
        connection = BuildHubConnection(endpoint);

        InitializeEvents();
        InitializeActions();
    }

    public async Task Connect()
    {
        if (connection.State != HubConnectionState.Connected)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine($"Connecting to {endpoint}");
                    await connection.StartAsync(token);
                    return;
                }
                catch when (token.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to connect to {endpoint}");
                    Console.WriteLine(ex.Message);
                    await Task.Delay(5000);
                }
            }
        }
    }

    public async Task RegisterListener()
    {
        Console.WriteLine($"Registering to listen to SyncR endpoint {endpoint}");
        await connection.InvokeAsync("RegisterListener");
    }

    public async Task RegisterService()
    {
        Console.WriteLine($"Registering as as a service for SyncR endpoint {endpoint}");
        await connection.InvokeAsync("RegisterService");
    }

    public async Task Join(Guid key)
    {
        Groups.Add(key);
        Console.WriteLine($"Joining gruop {key}");
        await connection.InvokeAsync("Join", key);
    }

    public async Task Leave(Guid key)
    {
        Groups.Remove(key);
        Console.WriteLine($"Leaving group {key}");
        await connection.InvokeAsync("Leave", key);
    }

    public async Task Push(SyncMessage<T> message)
    {
        if (Available)
            await connection.InvokeAsync("SendPush", message);
        else
            throw new SyncServiceUnavailableException(endpoint);
    }

    public async Task Notify(SyncMessage<T> message) =>
        await connection.InvokeAsync("SendNotify", message);

    public async Task Complete(SyncMessage<T> message) =>
        await connection.InvokeAsync("SendComplete", message);

    public async Task Return(SyncMessage<T> message) =>
        await connection.InvokeAsync("SendReturn", message);

    public async Task Reject(SyncMessage<T> message) =>
        await connection.InvokeAsync("SendReject", message);

    protected virtual HubConnection BuildHubConnection(string endpoint) =>
        new HubConnectionBuilder()
        .WithUrl(endpoint)
        .WithAutomaticReconnect()
        .Build();

    [MemberNotNull(
        nameof(OnAvailable),
        nameof(OnDisconnected)
    )]
    protected void InitializeEvents()
    {
        connection.Closed += async (error) =>
        {
            await Task.Delay(5000);
            await Connect();
        };

        OnAvailable = new("Available", connection);
        OnDisconnected = new("Disconnected", connection);

        OnAvailable.Set(() =>
        {
            Console.WriteLine("Sync Service is available");
            Available = true;
        });

        OnDisconnected.Set(() =>
        {
            Console.WriteLine("Sync Service disconnected");
            Available = false;
        });
    }

    [MemberNotNull(
        nameof(OnFinalizeListener),
        nameof(OnFinalizeService),
        nameof(OnPush),
        nameof(OnNotify),
        nameof(OnComplete),
        nameof(OnReturn),
        nameof(OnReject),
        nameof(OnUnavailable)
    )]
    protected void InitializeActions()
    {
        OnFinalizeListener = new("FinalizeListener", connection);
        OnFinalizeService = new("FinalizeService", connection);
        OnPush = new("Push", connection);
        OnNotify = new("Notify", connection);
        OnComplete = new("Complete", connection);
        OnReturn = new("Return", connection);
        OnReject = new("Reject", connection);
        OnUnavailable = new("Unavailable", connection);
    }

    public async ValueTask DisposeAsync()
    {
        DisposeEvents();

        await DisposeConnection()
            .ConfigureAwait(false);

        GC.SuppressFinalize(this);
    }

    protected void DisposeEvents()
    {
        OnFinalizeListener.Dispose();
        OnFinalizeService.Dispose();
        OnPush.Dispose();
        OnNotify.Dispose();
        OnComplete.Dispose();
        OnReturn.Dispose();
        OnReject.Dispose();
        OnUnavailable.Dispose();
    }

    protected async ValueTask DisposeConnection()
    {
        if (connection is not null)
        {
            foreach (Guid key in Groups)
                await Leave(key);

            await connection
                .DisposeAsync()
                .ConfigureAwait(false);
        }
    }
}