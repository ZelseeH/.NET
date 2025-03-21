using ChampionsLeagueMaster.Models;
using System.Linq.Expressions;

namespace ChampionsLeagueMaster.Repository
{
    public interface IPlayerRepository
    {
        Task<IQueryable<Player>> GetAllAsync();
        Task<Player?> GetByIdAsync(int playerId);
        Task InsertAsync(Player player);
        Task UpdateAsync(Player player);
        Task DeleteAsync(int playerId);
        Task SaveAsync();
    }
}
