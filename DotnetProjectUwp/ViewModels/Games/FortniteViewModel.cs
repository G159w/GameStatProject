using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Responses;
using RestSharp;

namespace DotnetProjectUwp.ViewModels.Games
{
    public class FortniteViewModel : GameViewModel<FortniteStatsResponse>
    {
        public FortniteViewModel(RestClient restClient) : base(restClient, "fortnite")
        {

        }
    }
}
