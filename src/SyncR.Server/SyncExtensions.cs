using Microsoft.Extensions.DependencyInjection;

namespace SyncR.Server;
public static class SyncExtensions
{
    public static void AddSyncServiceManager(this IServiceCollection services) =>
        services.AddSingleton<SyncServiceManager>();

    public static void MapSyncServices(this IServiceProvider provider, IEnumerable<SyncService> services)
    {
        SyncServiceManager? manager = provider.GetService<SyncServiceManager>();
        manager?.RegisterServices(services);
    }
}