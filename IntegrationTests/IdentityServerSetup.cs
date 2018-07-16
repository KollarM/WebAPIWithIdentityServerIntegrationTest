using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
    public class IdentityServerSetup
    {
        private IWebHost _webHost;
        private static IdentityServerSetup instance = null;
        private static readonly object padlock = new object();

        public string IdentityServerUrl => "http://localhost:5005";
        private string TokenEndpoint => IdentityServerUrl + "/connect/token";


        IdentityServerSetup() { }

        public static IdentityServerSetup Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new IdentityServerSetup();
                        instance.InitializeIdentityServer();
                    }
                    return instance;
                }
            }
        }

        public async Task<string> GetAccessTokenForUser(string userName, string password, string clientId = "client", string clientSecret = "secret")
        {
            var client = new TokenClient(TokenEndpoint, clientId, clientSecret);

            var response = await client.RequestResourceOwnerPasswordAsync(userName, password, "api1");
            return response.AccessToken;
        }

        private void InitializeIdentityServer()
        {
            _webHost = WebHost.CreateDefaultBuilder()
                .UseUrls(IdentityServerUrl)
                .ConfigureServices(services =>
                {
                    services.AddIdentityServer()
                        .AddDeveloperSigningCredential()
                        .AddInMemoryApiResources(GetApiResources())
                        .AddInMemoryClients(GetClients())
                        .AddTestUsers(GetUsers());
                })
                .Configure(app => app.UseIdentityServer())
                .Build();
            Task.Factory.StartNew(() => _webHost.Run());
            //_webHost.Run();
        }

        public static List<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API") {ApiSecrets = {new Secret("secret".Sha256())}}
            };
        }


        // Default client
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    },
                    AllowOfflineAccess = true
                }
            };
        }

        // Default user
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "user",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "User"),
                        new Claim("website", "https://user.com")
                    }
                }
            };
        }
    }
}