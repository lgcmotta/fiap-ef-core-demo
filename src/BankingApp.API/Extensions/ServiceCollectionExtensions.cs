using System.Reflection;
using BankingApp.API.Behaviors;
using BankingApp.API.Infrastructure;
using BankingApp.Domain;
using EFCoreSecondLevelCacheInterceptor;
using MediatR.NotificationPublishers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BankingApp.API.Extensions;

// ReSharper disable PossibleMultipleEnumeration
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCQRS(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(typeof(Program).Assembly, typeof(Account).Assembly);
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
            configuration.AddOpenBehavior(typeof(TransactionalBehavior<,>));
            configuration.NotificationPublisherType = typeof(TaskWhenAllPublisher);
        });

        return services;
    }


    public static IServiceCollection AddMySqlDbContext<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] interceptorsAssemblies) where TDbContext : DbContext
    {
        var connectionString = configuration.GetConnectionString(typeof(TDbContext).ResolveTypeName());

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"Connection string for {nameof(TDbContext)} was not found.");
        }

        var maxRetryCount = configuration.GetValue<int>("MySql:MaxRetryCount");
        var enableSecondLevelCache = configuration.GetValue<bool>("MySql:EnableSecondLevelCache");

        if (enableSecondLevelCache)
        {
            services.AddEFSecondLevelCache(options =>
                options.UseMemoryCacheProvider().DisableLogging(value: true)
            );
        }

        services.AddDbContext<TDbContext>((provider, optionsBuilder) =>
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mysqlBuilder =>
            {
                mysqlBuilder.EnableRetryOnFailure(maxRetryCount);

            });

            var interceptors = provider.ResolveEfCoreInterceptors(enableSecondLevelCache, interceptorsAssemblies);

            if (interceptors.Any())
            {
                optionsBuilder.AddInterceptors(interceptors);
            }
        });

        return services;
    }

    public static IServiceCollection AddUnitOfWork<TDbContext>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TDbContext : DbContext
    {
        services.TryAdd(new ServiceDescriptor(typeof(IUnitOfWork), typeof(UnitOfWork<TDbContext>), serviceLifetime));
    
        return services;
    }
}