using RestSharp;

namespace Fixer.Utils
{
    public interface IRestClientWrapper
    {
        T ExecuteRequest<T>(IRestRequest request, string url) where T : new();
    }
}