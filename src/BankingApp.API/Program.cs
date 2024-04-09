using BankingApp.API.Extensions;
using BankingApp.API.Features.CreateAccount;
using BankingApp.API.Features.GetAccountById;
using BankingApp.API.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilogConsoleLogger();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMySqlDbContext<AccountsDbContext>(builder.Configuration);
builder.Services.AddUnitOfWork<AccountsDbContext>();
builder.Services.AddCQRS();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var group = app.MapGroup("/api/account");
{
    group.MapPost("/", CreateAccountEndpoint.PostAsync);
    group.MapGet("/{id:min(1):int}", GetAccountByIdEndpoint.GetAsync);
}

await app.RunAsync();

public partial class Program
{
    protected Program()
    { }
}