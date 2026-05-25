namespace Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    /// <summary>
    /// Commits pending persistence changes.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
