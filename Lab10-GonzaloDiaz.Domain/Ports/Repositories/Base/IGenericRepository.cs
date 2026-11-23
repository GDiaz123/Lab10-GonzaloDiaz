using System.Linq.Expressions;

namespace Lab10_GonzaloDiaz.Domain.Ports.Repositories.Base;

public interface IGenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Query();

        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<int> SaveChangesAsync();
        Task AddAsync(T entity); 
        Task AddAndSaveAsync(T entity);
        
        void Update(T entity);
        void Delete(T entity);
    }
}