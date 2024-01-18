namespace BankingApp.API.Factories;

public static class HostingEnvironmentVariables
{
    private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

    public static string? GetAspNetCoreEnvironment()
        => Environment.GetEnvironmentVariable(AspNetCoreEnvironment);
}