using System.Net;

namespace DotnetProjectBack.Tools
{
    public static class Extensions
    {
        public static bool IsStatusOk(this HttpStatusCode status)
        {
            return ((int) status >= 200 && (int) status < 300);
        }
    }
}