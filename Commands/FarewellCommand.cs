using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using Microsoft.Extensions.Logging;

namespace CrudMaker.Commands;

public class FarewellCommand : Command
{
    private readonly ILogger<GenericService> _logger;
    private readonly GenericService _service;

    public FarewellCommand(ILogger<GenericService> logger, GenericService service)
        : base("farewell", "Farewell!")
    {
        _logger = logger;
        _service = service;
        var name = new Option<string>("--name")
        {
            Name = "name",
            Description = "The name of the person to bid farewell.",
            IsRequired = true
        };
        var times = new Option<int>("--times")
        {
            Name = "times",
            Description = "How many times to bid farewell.",
            IsRequired = false
        };
        times.SetDefaultValue(1);

        this.AddOption(name);
        this.AddOption(times);

        this.Handler = CommandHandler.Create(
            (int times, string name) => this.HandleCommand(times, name)
        );
    }

    private int HandleCommand(int times, string name)
    {
        _logger.LogInformation($"HandleCommand {this.GetType()}");
        try
        {
            while (times > 0)
            {
                Console.WriteLine($"Farewell {name}!");
                times--;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return 0;
        }

        return 1;
    }
}
