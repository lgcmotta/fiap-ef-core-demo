using BankingApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BankingApp.API.Infrastructure.Interceptors;

public class AccountSaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(eventData);

        var accountEntries = eventData?.Context?.ChangeTracker.Entries<Account>() ?? Enumerable.Empty<EntityEntry<Account>>();

        foreach (var accountEntry in accountEntries)
        {
            if (accountEntry.State == EntityState.Added)
            {
                accountEntry.Property<DateTime>("CreatedAt").CurrentValue = DateTime.UtcNow;
            }

            if (accountEntry.State == EntityState.Modified)
            {
                accountEntry.Property<DateTime?>("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SavingChangesAsync(eventData!, result, cancellationToken);
    }
}