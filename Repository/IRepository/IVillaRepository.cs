using System.Linq.Expressions;
using Villafy_Api.Models;

namespace Villafy_Api.Repository.IRepository
{
    public interface IVillaRepository
    {
        Task<List<Villa>> GetAllAsync(Expression<Func<Villa>>? filter = null);
        Task<Villa> GetByIdAsync(Expression<Func<Villa>>? filter = null, bool tracked = true);
        Task Create(Villa entity);
        Task Remove(Villa entity);
        Task Save();
    }
}
