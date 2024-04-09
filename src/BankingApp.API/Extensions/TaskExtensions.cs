using Nito.AsyncEx;

namespace BankingApp.API.Extensions;

public static class TaskExtensions
{
    public static async IAsyncEnumerable<T> WhenEach<T>(this IEnumerable<Task<T>> tasks)
    {
        foreach (var task in tasks.OrderByCompletion())
        {
             yield return await task;
        }
    }

    public static async IAsyncEnumerable<Task> WhenEach(this IEnumerable<Task> tasks)
    {
        foreach (var task in tasks.OrderByCompletion())
        {
            yield return task;
        }
    }
}