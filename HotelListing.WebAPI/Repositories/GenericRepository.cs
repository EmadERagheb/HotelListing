using HotelListing.Data;
using HotelListing.WebAPI.Contracts;
using Microsoft.EntityFrameworkCore;

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
            _context.SaveChanges();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            if (entity is not null)
            {
                _context.Entry(entity).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity is not null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int? id)
        {
            if (id is null)
                return null;
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
