using RestSharp;

namespace DotnetProjectBack.DataAccess.ApiRequests
{
    public interface IRequestFactory
    {
        RestRequest GenerateRequest(string url, Method method, string shortGameName, string personalApiKey = "");
    }
}