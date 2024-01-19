namespace BankingApp.Domain.Exceptions;

public class AccountNotFoundException(string? message) : Exception(message);