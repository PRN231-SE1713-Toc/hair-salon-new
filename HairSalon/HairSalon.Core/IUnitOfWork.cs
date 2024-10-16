namespace HairSalon.Core
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {
        Task CommitAsync(CancellationToken cancellationToken = default);

        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
