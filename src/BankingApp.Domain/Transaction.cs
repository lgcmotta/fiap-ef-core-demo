namespace BankingApp.Domain;

public class Transaction
{
    private Money _amount;

    private Transaction() {}

    public Guid Identifier { get; private set; }

    public DateTime Occurence { get; private set; }

    public TransactionType Type { get; private set; }

    public Money Amount => Type == TransactionType.Debit ? _amount.Negative() : _amount;

    public static Transaction NewDebitTransaction(Money amount) => new()
    {
        _amount = amount,
        Identifier = Guid.NewGuid(),
        Occurence = DateTime.UtcNow,
        Type = TransactionType.Debit
    };

    public static Transaction NewCreditTransaction(Money amount) => new()
    {
        _amount = amount,
        Identifier = Guid.NewGuid(),
        Occurence = DateTime.UtcNow,
        Type = TransactionType.Credit
    };
}