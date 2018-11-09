using System;
using Fixer.Utils;
using Moq;
using RestSharp;
using Xunit;

namespace Fixer.Tests.Utils
{
    public class RestClientWrapperTests
    {
        [Fact]
        public void ExecuteRequest_SetsClientsBaseUrl()
        {
            var mockRestClient = new Mock<IRestClient>();
            var mockResponse = new Mock<IRestResponse<object>>();
            mockRestClient.Setup(c => c.Execute<object>(It.IsAny<IRestRequest>())).Returns(mockResponse.Object);
            mockRestClient.SetupProperty(c => c.BaseUrl);
            var wrapper = new RestClientWrapper(mockRestClient.Object);

            const string url = "https://fake.url/";
            wrapper.ExecuteRequest<object>(new RestRequest(), url);

            Assert.Equal(url, mockRestClient.Object.BaseUrl.ToString());
        }

        [Fact]
        public void ExecuteRequest_ErrorException_ThrowsApplicationException()
        {
            var mockRestClient = new Mock<IRestClient>();
            var mockResponse = new Mock<IRestResponse<object>>();
            mockRestClient.Setup(c => c.Execute<object>(It.IsAny<IRestRequest>())).Returns(mockResponse.Object);
            mockResponse.Setup(r => r.ErrorException).Returns(new Exception("test"));
            var wrapper = new RestClientWrapper(mockRestClient.Object);

            const string url = "https://fake.url/";
            Assert.Throws<ApplicationException>(() => wrapper.ExecuteRequest<object>(new RestRequest(), url));
        }
    }
}