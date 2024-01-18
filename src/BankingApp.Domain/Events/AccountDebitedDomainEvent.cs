using MediatR;

namespace BankingApp.Domain.Events;

public record AccountDebitedDomainEvent(
    string PixKey,
    decimal Amount,
    Guid TransactionIdentifier,
    DateTime Occurrence) : INotification;