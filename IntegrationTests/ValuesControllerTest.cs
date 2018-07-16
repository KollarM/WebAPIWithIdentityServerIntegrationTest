using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class ValuesControllerTest: IClassFixture<CustomWebApplicationFactory<WebAPI.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<WebAPI.Startup> _factory;

        public ValuesControllerTest(CustomWebApplicationFactory<WebAPI.Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_should_return_ok_for_unauthorized_user()
        {
            // Arrange

            // Act
            var result = await _factory.CreateClient().GetAsync("api/values/get");

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        }


        [Fact]
        public async Task GetForAuthorized_should_return_unauthorized_for_unauthorized_user()
        {
            // Arrange

            // Act
            var result = await _factory.CreateClient().GetAsync("api/values/GetForAuthorized");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }


        [Fact]
        public async Task GetForAuthorized_should_return_ok_for_authorized_user()
        {
            // Arrange
            string token = await IdentityServerSetup.Instance.GetAccessTokenForUser("user", "password");
            var client = _factory.CreateClient();
            client.SetBearerToken(token);

            // Act
            var result = await client.GetAsync("api/values/GetForAuthorized");

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
