using Villafy_Api.Data;
using Villafy_Api.Models;
using Villafy_Api.Repository.IRepository;

namespace Villafy_Api.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
            _dbContext = db;
        }

        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _dbContext.VillaNumbers.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }

}
