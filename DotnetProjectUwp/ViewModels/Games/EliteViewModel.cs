using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Models.Responses;
using RestSharp;

namespace DotnetProjectUwp.ViewModels.Games
{
    public class EliteViewModel : GameViewModel<EliteStatsResponse>
    {

        public EliteViewModel(RestClient restClient) : base(restClient, "Elite")
        {
        }
    }
}
