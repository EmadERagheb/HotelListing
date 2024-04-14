using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.Data;
using HotelListing.WebAPI.Contracts;
using HotelListing.WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace HotelListing.WebAPI.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelListingDbcontext _context;
        private readonly IMapper _mapper;

        public GenericRepository(HotelListingDbcontext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            var entity = await GetAsync<T>(filter);
            return entity is not null;
        }

        public async Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>?> filter = null, params string[] properties)
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
            return await query.ProjectTo<TResult>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<QueryResult<TResult>> GetAllAsync<TResult>(QueryPerimeters queryPerimeters)
        {
          
            return new QueryResult<TResult>()
            {
                PageNumber = queryPerimeters.PageNumber,
                Items = await _context.Set<T>().Skip(queryPerimeters.PageSize * (queryPerimeters.PageNumber-1))
                   .Take(queryPerimeters.PageSize)
                   .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                   .ToListAsync(),
                TotalCount = await _context.Set<T>().CountAsync(),
                RecordNumber=queryPerimeters.PageSize
           

            };

        }



        public async Task<TResult> GetAsync<TResult>(Expression<Func<T, bool>> filter, params string[] includedProperires)
        {
            IQueryable<T> query = _context.Set<T>().Where(filter);

            if (includedProperires is not null)
            {
                foreach (string property in includedProperires)
                {
                    query = query.Include(property);
                }
            }
            return await query.ProjectTo<TResult>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

      

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
