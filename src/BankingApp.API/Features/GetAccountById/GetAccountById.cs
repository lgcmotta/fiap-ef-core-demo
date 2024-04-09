using BankingApp.API.Infrastructure;
using BankingApp.Domain;
using BankingApp.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.API.Features.GetAccountById;

public static class GetAccountByIdEndpoint
{
    public static async Task<IResult> GetAsync(int id, IMediator mediator, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new GetAccountByIdQuery(id), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }
}

public record GetAccountByIdQuery(int AccountId) : IRequest<GetAccountByIdResponse>;

public record GetAccountByIdResponse(string FullName, string PixKey, decimal Balance);

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, GetAccountByIdResponse>
{
    private readonly AccountsDbContext _context;

    public GetAccountByIdQueryHandler(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task<GetAccountByIdResponse> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _context.Set<Account>()
            .FirstOrDefaultAsync(a => EF.Property<int>(a, "Id") == request.AccountId, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account with id: ${request.AccountId} was not found.");
        }

        return new GetAccountByIdResponse($"{account.FirstName} {account.LastName}", account.PixKey, account.Balance);
    }
}