using System;
using RestSharp;

namespace Fixer.Utils
{
    public class RestClientWrapper : IRestClientWrapper
    {
        private readonly IRestClient _restClient;

        public RestClientWrapper(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public T ExecuteRequest<T>(IRestRequest request, string url) where T : new()
        {
            _restClient.BaseUrl = new Uri(url);
            var response = _restClient.Execute<T>(request);

            if (response.ErrorException != null)
            {
                throw new ApplicationException(
                    "Error retrieving response. Check inner details for more info.",
                    response.ErrorException);
            }

            return response.Data;
        }
    }
}