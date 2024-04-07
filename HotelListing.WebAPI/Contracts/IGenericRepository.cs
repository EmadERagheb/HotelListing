using System.Linq.Expressions;

namespace HotelListing.WebAPI.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T,bool>> filter, params string[] properties);

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>?> filter=null, params string[] properties);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> Exists(Expression<Func<T,bool>>filter);
    }
}
