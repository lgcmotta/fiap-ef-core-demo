using BankingApp.Domain.Core;

namespace BankingApp.Domain;

public class TransactionType : Enumeration<int, string>
{
    private TransactionType(int key, string value) : base(key, value)
    { }

    public static TransactionType Debit => new(0, nameof(Debit));

    public static TransactionType Credit => new(1, nameof(Credit));
}