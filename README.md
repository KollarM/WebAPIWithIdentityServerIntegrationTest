# WebAPIWithIdentityServerIntegrationTest
This contains example solution for.NET Core 2.1 with IdentityServer Authentication and IntegrationTest (using xUnit)

# The problem
For WebAPI which is using authentication against STS (Security Token Service, like IdentityServer) we need our IntegrationTests to send AuthenticationToken in request header. Problem is to create this token and not override authentication mechanism in Startup of WebAPI or hack it too much.

# The easy solution
Easiest way is to have IdentityServer (or other STS) running in TEST environment with test users and our IntegrationTest to do a real HTTP call to this service to log in and obtain a token. Easy, straightfoward but not 100% clean! Ideally we don't want our build server which is running unit tests to do a real HTTP calls outside of its box.

# Better solution
Better is to create 2 WebHosts. One for WebAPI which we are testing and another for running IdentityServer on different port. In ConfigureServices we override URL of this IdentityServer.

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.Configure<IdentityServerConfig>(config =>
                {
                    config.Authority = IdentityServerSetup.Instance.IdentityServerUrl;
                    config.ApiName = "api1";
                    config.ApiSecret = "secret";
                    config.RequireHttpsMetadata = false;
                });
            });
        }
