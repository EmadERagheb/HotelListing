using HotelListing.Data;
using HotelListing.WebAPI.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace HotelListing.WebAPI.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelListingDbcontext _context;


        public GenericRepository(HotelListingDbcontext context)
        {
            _context = context;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.AddAsync(entity);
         await    _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(Expression<Func<T,bool>>filter)
        {
            var entity = await GetAsync(filter);
            return entity is not null;
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>?> filter=null, params string[] properties)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);

                if (properties != null)
                {
                    foreach (var property in properties)
                    {
                        query.Include(property);

                    }
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, params string[] includedProperires)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includedProperires is not null)
            {
                foreach (string property in includedProperires)
                {
                    query.Include(property);
                }
            }
            return await _context.Set<T>().Where(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
