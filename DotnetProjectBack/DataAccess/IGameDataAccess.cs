using DotnetProjectBack.DatabaseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetProjectBack.DataAccess
{
    public interface IGameDataAccess
    {
        Task<TGame> GetDbGame(string shortName);
        Task<IEnumerable<TGame>> GetSupportedGames();
    }
}
