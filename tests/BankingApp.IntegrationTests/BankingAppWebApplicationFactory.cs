// ReSharper disable PossibleMultipleEnumeration
namespace BankingApp.IntegrationTests;

public class BankingAppWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly MySqlContainer _mySqlContainer;

    public BankingAppWebApplicationFactory()
    {
        var mySqlBuilder = new MySqlBuilder();

        _mySqlContainer = mySqlBuilder.WithDatabase("BankingApp")
            .WithUsername("user")
            .WithPassword("123456")
            .WithPortBinding("3306", true)
            .Build();
    }

    public async Task InitializeContainerAsync()
    {
        await _mySqlContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(DbContextOptions<AccountsDbContext>));

            if (dbContextServiceDescriptor is not null)
            {
                services.Remove(dbContextServiceDescriptor);
            }

            var connectionString = _mySqlContainer.GetConnectionString();

            services.AddDbContext<AccountsDbContext>((provider, optionsBuilder) =>
            {
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mysqlBuilder =>
                {
                    mysqlBuilder.EnableRetryOnFailure(3);

                });

                var interceptors = provider.ResolveEfCoreInterceptors(true, typeof(AccountsDbContext).Assembly);

                if (interceptors.Any())
                {
                    optionsBuilder.AddInterceptors(interceptors);
                }
            });
        });
    }
}