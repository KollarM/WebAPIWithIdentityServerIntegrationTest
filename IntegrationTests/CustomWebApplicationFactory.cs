using System;
using System.Collections.Generic;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using WebAPI;

namespace IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<WebAPI.Startup>
    {
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
    }
}
