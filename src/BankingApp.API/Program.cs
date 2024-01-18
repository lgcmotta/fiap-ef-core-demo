using BankingApp.API.Extensions;
using BankingApp.API.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilogConsoleLogger();

builder.Services.AddMySqlDbContext<AccountsDbContext>(builder.Configuration);
builder.Services.AddCQRS();

var app = builder.Build();

await app.RunAsync();

public partial class Program
{
    protected Program()
    { }
}