using BankingApp.Domain.Core;
using BankingApp.Domain.Events;

namespace BankingApp.Domain;

public class Account : AggregateRoot
{
    private readonly List<Transaction> _transactions = [];

    private Account()
    { }

    public Account(string firstName, string lastName, string pixKey) : this()
    {
        FirstName = firstName;
        LastName = lastName;
        PixKey = pixKey;
        Balance = Money.Zero;
    }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string PixKey { get; private set; }

    public Money Balance { get; private set; }

    public IEnumerable<Transaction> Transactions => _transactions.AsReadOnly();

    public Transaction Debit(Money amount)
    {
        if (amount <= Money.Zero)
        {
            throw new ArgumentException("Money amount for debit transaction must be greater than zero", nameof(amount));
        }

        Balance -= amount;

        var transaction = Transaction.NewDebitTransaction(amount);

        _transactions.Add(transaction);

        AddDomainEvent(new AccountDebitedDomainEvent(PixKey, amount, transaction.Identifier, transaction.Occurence));

        return transaction;
    }

    public Transaction Credit(Money amount)
    {
        if (amount <= Money.Zero)
        {
            throw new ArgumentException("Money amount for credit transaction must be greater than zero", nameof(amount));
        }

        Balance += amount;

        var transaction = Transaction.NewCreditTransaction(amount);

        _transactions.Add(transaction);

        AddDomainEvent(new AccountCreditedDomainEvent(PixKey, amount, transaction.Identifier, transaction.Occurence));

        return transaction;
    }

    public IEnumerable<Transaction> GenerateTransactionStatement(DateOnly start, DateOnly end)
    {
        var startOfDay = StartOfDay(start);
        var endOfDay = EndOfDay(end);

        return _transactions
            .Where(transaction => transaction.Occurence >= startOfDay && transaction.Occurence <= endOfDay);
    }

    private static DateTime StartOfDay(DateOnly start) => new(start.Year, start.Month, start.Day, 0, 0, 0);
    private static DateTime EndOfDay(DateOnly end) => new(end.Year, end.Month, end.Day, 23, 59, 59);

}