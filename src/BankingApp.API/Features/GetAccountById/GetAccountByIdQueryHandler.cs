using BankingApp.API.Infrastructure;
using BankingApp.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.API.Features.GetAccountById;

public record GetAccountByIdQuery(int AccountId) : IRequest<AccountModel>;

public class AccountModel
{
    public string FullName { get; set; }

    public string PixKey { get; set; }

    public decimal Balance { get; set; }
}

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountModel>
{
    private readonly AccountsDbContext _context;

    public GetAccountByIdQueryHandler(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task<AccountModel> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _context.Set<Account>()
            .FirstOrDefaultAsync(a => EF.Property<int>(a, "Id") == request.AccountId, cancellationToken);

        if (account is null)
        {
            throw new InvalidOperationException(nameof(account));
        }

        return new AccountModel
        {
            FullName = $"{account.FirstName} {account.LastName}",
            PixKey = account.PixKey,
            Balance = account.Balance,
        };
    }
}