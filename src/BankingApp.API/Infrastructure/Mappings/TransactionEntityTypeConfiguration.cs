using BankingApp.Domain;
using BankingApp.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace BankingApp.API.Infrastructure.Mappings;

public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(t => t.Identifier);

        builder.Property(t => t.Identifier)
            .ValueGeneratedNever();

        builder.Property<Money>("_amount")
            .HasPrecision(19, 4)
            .HasColumnName("Amount")
            .HasConversion(
                m => m.Value,
                value => new Money(value)
            );

        builder.Property(t => t.Type)
            .HasConversion(
                type => type.Key,
                value => TransactionType.ParseByKey<TransactionType>(value)
            );

        builder.Ignore(t => t.Amount);

    }
}

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.Property<int>("Id")
            .ValueGeneratedOnAdd();

        builder.HasKey("Id");

        builder.Property(a => a.FirstName).IsRequired();
        builder.Property(a => a.LastName).IsRequired();
        builder.Property(a => a.PixKey).IsRequired();

        builder.Property(a => a.Balance)
            .HasPrecision(19, 4)
            .HasConversion(
                m => m.Value,
                value => new Money(value)
            );

        builder.Property<DateTime>("CreatedAt");
        builder.Property<DateTime?>("UpdatedAt").IsRequired(false);

        builder.HasMany<Transaction>("_transactions")
            .WithOne()
            .HasForeignKey("AccountId");

        builder.Ignore(a => a.Transactions);
    }
}