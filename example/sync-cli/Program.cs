using SyncCli;
using System.CommandLine;

RootCommand root = CommandApp.Initialize();
await root.InvokeAsync(args);