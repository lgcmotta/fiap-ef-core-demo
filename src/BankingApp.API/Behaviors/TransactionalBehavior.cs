using BankingApp.API.Extensions;
using BankingApp.API.Infrastructure;
using BankingApp.Domain.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.API.Behaviors;

public class TransactionalBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public TransactionalBehavior(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var strategy = _unitOfWork.CreateExecutionStrategy();

        var response = await strategy.ExecuteAsync(async () => await TryExecuteStrategy(next, cancellationToken))
            .ConfigureAwait(false);

        return response;
    }

    private async Task<TResponse> TryExecuteStrategy(RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            var response = await next();

            var domainEvents = _unitOfWork.ExtractDomainEventsFromAggregates();

            await _unitOfWork.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            await _unitOfWork.CommitTransactionAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            var dispatchingTasks = domainEvents.Select(domainEvent => _mediator.Publish(domainEvent, cancellationToken));

            await foreach (var dispatchingTask in dispatchingTasks.WhenEach().WithCancellation(cancellationToken))
            {
                await dispatchingTask;
            }

            return response;
        }
        catch
        {
            await _unitOfWork.RollBackTransactionAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            throw;
        }
    }
}