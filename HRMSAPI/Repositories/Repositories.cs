using HRMSAPI.Data;
using HRMSAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HRMSAPI.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly AppDbContext _context;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}

public class UserRepository : BaseRepository<Models.Entities.User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Models.Entities.User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Models.Entities.User?> GetUserWithDetailsAsync(string email)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
            .Include(u => u.UserOrganizations)
                .ThenInclude(uo => uo.Organization)
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}

public class OrganizationRepository : BaseRepository<Models.Entities.Organization>, IOrganizationRepository
{
    public OrganizationRepository(AppDbContext context) : base(context)
    {
    }
}

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IUserRepository Users { get; private set; }
    public IOrganizationRepository Organizations { get; private set; }
    public IBaseRepository<Models.Entities.RefreshToken> RefreshTokens { get; private set; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new UserRepository(_context);
        Organizations = new OrganizationRepository(_context);
        RefreshTokens = new BaseRepository<Models.Entities.RefreshToken>(_context);
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}