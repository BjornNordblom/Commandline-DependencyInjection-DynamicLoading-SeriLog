using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using Microsoft.Extensions.Logging;

namespace CrudMaker.Commands;

public class GreetCommand : Command
{
    private readonly ILogger<GenericService> _logger;
    private readonly GenericService _service;

    public GreetCommand(ILogger<GenericService> logger, GenericService service)
        : base("greet", "Greet!")
    {
        _logger = logger;
        _service = service;
        AddOption(new Option<string>(new string[] { "--message", "-m" }, "The greeting message"));
        AddOption(new Option<string>(new string[] { "--name", "-n" }, "The person to greet"));

        this.Handler = CommandHandler.Create(
            (string name, string message) => this.HandleCommand(name, message)
        );
    }

    private int HandleCommand(string name, string message)
    {
        _logger.LogInformation($"HandleCommand {this.GetType()}");
        try
        {
            Console.WriteLine($"{message} {name}!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return 0;
        }

        return 1;
    }
}
