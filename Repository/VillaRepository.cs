using Villafy_Api.Data;
using Villafy_Api.Models;
using Villafy_Api.Repository.IRepository;

namespace Villafy_Api.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VillaRepository(ApplicationDbContext db) : base(db)
        {
            _dbContext = db;
        }

        public async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdateDate = DateTime.Now;
            _dbContext.Villas.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
