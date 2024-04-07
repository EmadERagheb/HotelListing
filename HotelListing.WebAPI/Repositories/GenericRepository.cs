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
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(Expression<Func<T, bool>> filter)
        {
            var entity = await GetAsync(filter);
            return entity is not null;
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>?> filter = null, params string[] properties)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter is not null)
            {
                query = query.Where(filter);
            }

            if (properties is not null)
            {
                foreach (var property in properties)
                {
                    query = query.Include(property);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, params string[] includedProperires)
        {
            IQueryable<T> query = _context.Set<T>().Where(filter);

            if (includedProperires is not null)
            {
                foreach (string property in includedProperires)
                {
                    query = query.Include(property);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
