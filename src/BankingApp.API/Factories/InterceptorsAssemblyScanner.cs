using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BankingApp.API.Factories;

public static class InterceptorAssemblyScanner
{
    public static IEnumerable<IInterceptor> Scan(IServiceProvider? serviceProvider = null, params Assembly[] interceptorAssemblies)
    {
        if (!interceptorAssemblies.Any())
        {
            return Enumerable.Empty<IInterceptor>();
        }

        return interceptorAssemblies
            .Distinct()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type is { IsClass: true, IsAbstract: false } &&
                           type.IsAssignableTo(typeof(IInterceptor)) &&
                           type.GetConstructor(
                               bindingAttr: BindingFlags.Instance | BindingFlags.Public,
                               binder: null,
                               types: Type.EmptyTypes,
                               modifiers: null) is not null)
            .Select(type => serviceProvider is null
                ? Activator.CreateInstance(type)
                : ActivatorUtilities.CreateInstance(serviceProvider, type))
            .Cast<IInterceptor>();
    }
}