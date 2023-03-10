namespace SyncR.Server;
public class SyncServiceManager
{
    List<SyncService> Services { get; set; }

    public SyncServiceManager()
    {
        Services = new();
    }

    public IEnumerable<SyncService> GetServices() => Services.AsReadOnly();

    public void RegisterServices(IEnumerable<SyncService> services) =>
        Services.AddRange(services);

    public SyncService RegisterService(string name, string endpoint, Type hub)
    {
        SyncService service = new(name, endpoint, hub);
        Services.Add(service);
        return service;
    }

    public SyncService GetService(Type hub)
    {
        try
        {
            return Services.First(service => service.Hub == hub);
        }
        catch (ArgumentNullException)
        {
            throw new Exception($"No service registration was found for Hub {nameof(hub)}");
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void RemoveService(SyncService service)
    {
        Services.Remove(service);
    }
}