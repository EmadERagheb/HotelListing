using HotelListing.WebAPI.Models;
using System.Linq.Expressions;

namespace HotelListing.WebAPI.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<TResult> GetAsync<TResult>(Expression<Func<T,bool>> filter, params string[] properties);

        Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>?> filter=null, params string[] properties);

        Task<QueryResult<TResult>> GetAllAsync<TResult>(QueryPerimeters query);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> Exists(Expression<Func<T,bool>>filter);
    }
}
