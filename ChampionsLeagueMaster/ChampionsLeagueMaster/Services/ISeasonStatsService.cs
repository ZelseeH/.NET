using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.ViewModels.SeasonStats;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChampionsLeagueMaster.Services
{
    public interface ISeasonStatsService
    {
        Task<IQueryable<SeasonStats>> GetSeasonStatsAsync(string season);
        Task<List<string>> GetSeasonsAsync();
        Task<string> GetDefaultSeasonAsync();
        Task RegenerateSeasonStatsAsync(string season);
    }

}
