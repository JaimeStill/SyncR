using Common;
using SyncCli.Clients;
using SyncR.Core;
using System.CommandLine;
using System.Net.Http.Json;

namespace SyncCli.Commands;
public class ProcessCommand : CliCommand
{
    public ProcessCommand() : base(
        "process",
        "Post a package to the API server",
        new Func<string, string, Intent, Task>(Call),
        new()
        {
            new Option<string>(
                new string[] { "--api", "-a" },
                getDefaultValue: () => "http://localhost:5002/api/process",
                description: "The API process endpoint"
            ),
            new Option<string>(
                new string[] { "--sync", "-s" },
                getDefaultValue: () => "http://localhost:5000/processor",
                description: "The SyncR server endpoint"
            ),
            new Option<Intent>(
                new string[] { "--intent", "-i" },
                getDefaultValue: () => Intent.Approve,
                description: "Generate a package based on the specified intent"
            )
        }
    ) { }

    static async Task Call(string api, string sync, Intent intent)
    {
        bool exit = false;
        await using ProcessorConnection processor = new(sync);

        async Task finalize(SyncMessage<Package> message)
        {
            Console.WriteLine(message.Message);
            await processor.Leave(message.Key);
            exit = true;
        }

        processor.OnComplete.Set((Func<SyncMessage<Package>, Task>)finalize);
        processor.OnReject.Set((Func<SyncMessage<Package>, Task>)finalize);

        Console.WriteLine($"Generating {intent.ToActionString()} Package");
        Package package = intent.GeneratePackage();

        await processor.Connect();
        await processor.Join(package.Key);

        Console.WriteLine($"Sending package {package.Name} to {api}");
        HttpClient client = new();
        HttpResponseMessage response = await client.PostAsJsonAsync(api, package);
        Console.WriteLine($"Package processing execution {(response.IsSuccessStatusCode ? "succeeded" : "failed")}");

        while (!exit) { }
    }
}