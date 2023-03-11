using SyncCli.Clients;
using System.CommandLine;

namespace SyncCli.Commands;
public class ListenerCommand : CliCommand
{
    public ListenerCommand() : base(
        "listen",
        "Listen to SyncR endpoint broadcasts",
        new Func<string, Task>(Call),
        new()
        {
            new Option<string>(
                new string[] { "--sync", "-s" },
                getDefaultValue: () => "http://localhost:5000/processor",
                description: "The SyncR server endpoint"
            )
        }
    ) { }

    static async Task Call(string sync)
    {
        await using ListenerConnection listener = new(sync);
        await listener.Connect();
        await listener.RegisterListener();

        while (true) { }
    }
}