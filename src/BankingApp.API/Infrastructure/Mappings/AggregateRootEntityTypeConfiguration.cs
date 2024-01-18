using BankingApp.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingApp.API.Infrastructure.Mappings;

public class AggregateRootEntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
    where T : class, IAggregateRoot
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Ignore(aggregate => aggregate.DomainEvents);
    }
}