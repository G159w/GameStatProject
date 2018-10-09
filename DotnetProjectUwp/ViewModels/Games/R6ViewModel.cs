using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Responses;
using RestSharp;

namespace DotnetProjectUwp.ViewModels.Games
{
    public class R6ViewModel : GameViewModel<R6StatsResponse>
    {
        public R6ViewModel(RestClient restClient) : base(restClient, "R6")
        {
        }
    }
}
