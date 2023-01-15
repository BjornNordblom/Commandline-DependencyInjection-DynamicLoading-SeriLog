using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

using CrudMaker.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(
                (context, configApp) =>
                {
                    var env = context.HostingEnvironment;
                    configApp.SetBasePath(env.ContentRootPath);
                    configApp.AddJsonFile($"appsettings.json", optional: false);
                }
            )
            .ConfigureServices(
                (context, services) =>
                {
                    services.AddSingleton<GenericService>();

                    Type grabCommandType = typeof(GreetCommand);
                    Type commandType = typeof(Command);

                    IEnumerable<Type> commands = grabCommandType.Assembly
                        .GetExportedTypes()
                        .Where(
                            x =>
                                x.Namespace == grabCommandType.Namespace
                                && commandType.IsAssignableFrom(x)
                        );

                    foreach (Type command in commands)
                    {
                        services.AddSingleton(commandType, command);
                    }
                }
            )
            .UseSerilog(
                (context, configuration) =>
                    configuration.Enrich
                        .FromLogContext()
                        .MinimumLevel.Debug()
                        .WriteTo.Console()
                        .ReadFrom.Configuration(context.Configuration),
                preserveStaticLogger: true
            );

        var host = hostBuilder.Build();

        var logger = host.Services.GetRequiredService<Serilog.ILogger>();

        var root = new RootCommand("CrudMaker");

        foreach (Command command in host.Services.GetServices<Command>())
        {
            logger.Information($"Added command {command}");
            root.AddCommand(command);
        }

        var parser = new CommandLineBuilder(root).UseDefaults().Build();
        return await parser.InvokeAsync(args);
    }
}
