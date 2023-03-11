using SyncCli.Commands;
using System.CommandLine;

namespace SyncCli;
public static class CommandApp
{
    public static RootCommand Initialize() =>
        BuildCommands()
            .BuildRootCommand();

    static List<Command> BuildCommands() => new()
    {
        new ListenerCommand().Build(),
        new ProcessCommand().Build()
    };

    public static RootCommand BuildRootCommand(this List<Command> commands)
    {
        var root = new RootCommand("SyncR CLI");

        root.AddGlobalOption(new Option<string>(
            new[] { "--server", "-s" },
            getDefaultValue: () => "http://localhost:5002/api/",
            description: "The root of the API server"
        ));

        commands.ForEach(root.AddCommand);
        return root;
    }
}