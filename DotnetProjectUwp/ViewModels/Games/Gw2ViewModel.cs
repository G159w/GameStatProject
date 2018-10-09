using Models.Responses;
using RestSharp;

namespace DotnetProjectUwp.ViewModels.Games
{
    public class Gw2ViewModel : GameViewModel<Gw2StatsResponse>
    {
        public Gw2ViewModel(RestClient restClient) : base(restClient, "gw2")
        {
        }
    }
}