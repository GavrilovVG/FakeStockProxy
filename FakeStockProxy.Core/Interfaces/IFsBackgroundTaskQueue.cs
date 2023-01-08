namespace FakeStockProxy.Core.Interfaces;

public interface IFsBackgroundTaskQueue
{
    Task QueueBackgroundWorkItemAsync(Func<CancellationToken, Task> workItem);

    Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
}
