using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BankingApp.API.Infrastructure;

public class AccountsDesignTimeDbContextFactory : IDesignTimeDbContextFactory<AccountsDbContext>
{
    public AccountsDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .Build();

        var connectionString = configuration.GetConnectionString(nameof(AccountsDbContext));

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"Connection string {nameof(AccountsDbContext)} was not found.");
        }

        var options = new DbContextOptionsBuilder<AccountsDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .Options;

        return new AccountsDbContext(options);
    }
}