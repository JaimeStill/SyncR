using Common;
using SyncR.Client;
using SyncR.Core;

namespace Processor.Services;
public class ProcessorConnection : SyncConnection<Package>
{
    public ProcessorConnection(IConfiguration config) : base(
        config.GetValue<string>("SyncR:ProcessorUrl") ?? "http://localhost:5000/processor"
    )
    {
        OnFinalizeService.Set((Guid key) => Console.WriteLine($"Service successfully registered"));
        OnPush.Set<SyncMessage<Package>>(ProcessPackage);
    }

    public static async Task Initialize(IServiceProvider services)
    {
        ProcessorConnection? processor = services.GetService<ProcessorConnection>();

        if (processor is not null)
            await processor.Register();
    }

    public async Task Register()
    {
        await Connect();
        Console.WriteLine("Registering service");
        await RegisterService();
    }

    public async Task ProcessPackage(SyncMessage<Package> message)
    {
        if (message.Data is not null)
        {
            Console.WriteLine($"Processing package {message.Data.Name}");

            Process process = Extensions.GenerateProcess(message.Data);

            Console.WriteLine($"Notifying group {message.Data.Key}");

            message.Message = $"Submitting package {message.Data.Name} for {message.Data.Intent.ToActionString()}";

            Console.WriteLine(message.Message);
            await Notify(message);

            await Task.Delay(1200);

            message.Message = $"Package {message.Data.Name} assigned process {process.Name}";

            Console.WriteLine(message.Message);
            await Notify(message);

            foreach (ProcessTask task in process.Tasks)
            {
                message.Message = $"Current step: {task.Name}";
                Console.WriteLine(message.Message);
                await Notify(message);

                await Task.Delay(task.Duration);

                message.Message = $"Package {message.Data.Name} was successfully approved by {task.Section}";
                Console.WriteLine(message.Message);
                await Notify(message);
            }

            await Task.Delay(300);

            message.Message = $"Package {message.Data.Name} was successfully approved";
            Console.WriteLine(message.Message);
            await Complete(message);
        }
        else
        {
            message.Message = $"No data was provided for processing";
            await Reject(message);
        }
    }
}