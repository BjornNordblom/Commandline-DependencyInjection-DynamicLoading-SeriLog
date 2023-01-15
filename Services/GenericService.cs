using Microsoft.Extensions.Logging;

public class GenericService
{
    private readonly ILogger<GenericService> _logger;

    public GenericService(ILogger<GenericService> logger)
    {
        _logger = logger;
    }

    public void Run()
    {
        _logger.LogInformation($"Starting {this.GetType()}");
    }
}
