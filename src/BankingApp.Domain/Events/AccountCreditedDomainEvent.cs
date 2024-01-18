using MediatR;

namespace BankingApp.Domain.Events;

public record AccountCreditedDomainEvent(
    string PixKey,
    decimal Amount,
    Guid TransactionIdentifier,
    DateTime Occurrence) : INotification;