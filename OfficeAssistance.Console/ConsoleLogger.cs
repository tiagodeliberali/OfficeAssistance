using Microsoft.Extensions.Logging;

public class ConsoleLogger : ILogger
{
    private bool enabled;

    public ConsoleLogger(bool v)
    {
        this.enabled = v;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        // implement a relevant BeginScope method
        return null!;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        // implement a log function using Console.WriteLine and displaying all relevand inputs from this method
        if (enabled)
        {
            Console.WriteLine($"[log: {logLevel}] {formatter(state, exception)}");
        }
    }
}