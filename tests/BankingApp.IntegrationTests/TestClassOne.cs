namespace BankingApp.IntegrationTests;

public class TestClassOne : IClassFixture<BankingAppWebApplicationFactory>, IAsyncLifetime
{
    private readonly BankingAppWebApplicationFactory _factory;
    private readonly Faker _faker = new();

    public TestClassOne(BankingAppWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public async Task InitializeAsync()
    {
        await _factory.InitializeContainerAsync();
        using var scope = _factory.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AccountsDbContext>();

        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _factory.DisposeAsync();
    }

    [Fact]
    public async Task TestWithDbContextAgainstActualMySql()
    {
        // Arrange
        var account = new Account(
            _faker.Name.FirstName(),
            _faker.Name.LastName(),
            _faker.Internet.Email()
        );

        using var scope = _factory.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AccountsDbContext>();

        // Act
        var entityEntry = await context.Set<Account>().AddAsync(account);

        await context.SaveChangesAsync();

        // Assert
        entityEntry.Property<int>("Id").CurrentValue.Should().Be(1);
    }


}