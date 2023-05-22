using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Villafy_Api.Data;
using Villafy_Api.Models;
using Villafy_Api.Repository.IRepository;

namespace Villafy_Api.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VillaRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Villa entity)
        {
            await _dbContext.Villas.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<Villa> query = _dbContext.Villas;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Villa> query = _dbContext.Villas;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync();

        }

        public async Task Remove(Villa entity)
        {
            _dbContext.Villas.Remove(entity);
            await SaveAsync();
        }



        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Villa entity)
        {
            _dbContext.Villas.Update(entity);
            await SaveAsync();
        }
    }
}
