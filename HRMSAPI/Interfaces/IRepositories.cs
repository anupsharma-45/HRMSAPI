using System.Linq.Expressions;

namespace HRMSAPI.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}

public interface IUserRepository : IBaseRepository<Models.Entities.User>
{
    Task<Models.Entities.User?> GetByEmailAsync(string email);
    Task<Models.Entities.User?> GetUserWithDetailsAsync(string email);
}

public interface IOrganizationRepository : IBaseRepository<Models.Entities.Organization>
{
}

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IOrganizationRepository Organizations { get; }
    IBaseRepository<Models.Entities.RefreshToken> RefreshTokens { get; }
    Task<int> CompleteAsync();
}