using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetProjectBack.DatabaseModels;

namespace DotnetProjectBack.DataAccess
{
    public class GameDataAccess : IGameDataAccess
    {
        private readonly DatabaseContext _databaseContext;

        public GameDataAccess(DatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }

        public async Task<TGame> GetDbGame(string shortName)
        {
            return _databaseContext.TGame.FirstOrDefault(g => g.ShortName == shortName);
        }

        public async Task<IEnumerable<TGame>> GetSupportedGames()
        {
            return await _databaseContext.TGame.ToListAsync();
        }
    }
}
