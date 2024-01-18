using Serilog;

namespace BankingApp.API.Extensions;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddSerilogConsoleLogger(this ILoggingBuilder builder)
    {
        builder.ClearProviders();

        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        Log.Logger = logger;

        builder.AddSerilog(logger);

        return builder;
    }

}