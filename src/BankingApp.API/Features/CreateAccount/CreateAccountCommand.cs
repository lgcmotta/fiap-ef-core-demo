using BankingApp.API.Infrastructure;
using BankingApp.Domain;
using BankingApp.Domain.Core;
using BankingApp.Domain.Events;
using MediatR;

namespace BankingApp.API.Features.CreateAccount;

public static class CreateAccountEndpoint
{
    public static async Task<IResult> PostAsync(
        CreateAccountCommand command, 
        IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(command, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }
}

public record CreateAccountResponse(string FirstName, string LastName, string PixKey);

// ReSharper disable once ClassNeverInstantiated.Global
public record CreateAccountCommand(string FirstName, string LastName, string PixKey) 
    : IRequest<CreateAccountResponse>, ICommand;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, CreateAccountResponse>
{
    private readonly AccountsDbContext _context;

    public CreateAccountCommandHandler(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task<CreateAccountResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var (firstName, lastName, pixKey) = request;
        
        var account = new Account(firstName, lastName, pixKey);
        
        await _context.Set<Account>().AddAsync(account, cancellationToken);
        
        return new CreateAccountResponse(account.FirstName, account.LastName, account.PixKey);
    }
}