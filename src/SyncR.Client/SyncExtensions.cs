using Microsoft.Extensions.DependencyInjection;

namespace SyncR.Client;
public static class SyncExtensions
{
    public static void AddSyncConnection<Connection, T>(this IServiceCollection services)
        where Connection : SyncConnection<T> =>
            services.AddSingleton<Connection>();
}