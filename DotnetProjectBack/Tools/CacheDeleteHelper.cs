using DotnetProjectBack.DataAccess.GamesStats;
using System.Threading.Tasks;

namespace DotnetProjectBack.Tools
{
    public class CacheDeleteHelper
    {
        private readonly IEliteDataAccess _eliteDataAccess;
        private readonly IR6DataAccess _r6DataAccess;
        private readonly ILolDataAccess _lolDataAccess;
        private readonly IFortniteDataAccess _fortniteDataAcces;
        private readonly IGw2DataAccess _gw2DataAccess;

        public CacheDeleteHelper(IEliteDataAccess eliteDataAccess, IR6DataAccess r6DataAccess, ILolDataAccess lolDataAccess,
            IFortniteDataAccess fortniteDataAcces, IGw2DataAccess gw2DataAccess)
        {
            _eliteDataAccess = eliteDataAccess;
            _r6DataAccess = r6DataAccess;
            _lolDataAccess = lolDataAccess;
            _fortniteDataAcces = fortniteDataAcces;
            _gw2DataAccess = gw2DataAccess;
        }

        public async Task<bool> DeleteUserCache(string shortGameName, long userId)
        {
            switch (shortGameName)
            {
                case "lol":
                    return (await _lolDataAccess.DeleteCache(userId)).Result.Success;
                case "elite":
                    return (await _eliteDataAccess.DeleteCache(userId)).Result.Success;
                case "r6":
                    return (await _r6DataAccess.DeleteCache(userId)).Result.Success;
                case "fortnite":
                    return (await _fortniteDataAcces.DeleteCache(userId)).Result.Success;
                case "gw2":
                    return (await _gw2DataAccess.DeleteCache(userId)).Result.Success;
                default:
                    return false;
            }
        }
    }
}
