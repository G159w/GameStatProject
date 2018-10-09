using System;
using RestSharp;

namespace DotnetProjectBack.DataAccess.ApiRequests
{
    public class RequestFactory: IRequestFactory
    {
        private const string LolApiKey = "<KEY>";
        private const string FortniteApiKey = "<KEY>";

        public RestRequest GenerateRequest(string url, Method method, string shortGameName, string personalApiKey = "")
        {
            var request = new RestRequest(url, method);
            switch (shortGameName.ToLowerInvariant())
            {
                case "lol":
                    request.AddParameter("api_key", LolApiKey);
                    break;
                case "fortnite":
                    request.AddHeader("TRN-Api-Key", FortniteApiKey);
                    break;
                case "gw2":
                    request.AddHeader("Authorization", $"Bearer {personalApiKey}");
                    break;
                case "elite":
                case "r6":
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(shortGameName), shortGameName, null);
            }

            return request;
        }
    }
}