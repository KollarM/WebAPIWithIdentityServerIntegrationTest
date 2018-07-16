using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace WebAPI
{
    public class Startup
    {
        private IApplicationBuilder _builder;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddOptions();
            services.Configure<IdentityServerConfig>(Configuration.GetSection("IdentityServerConfig"));

            // cors for testing locally
            //services.AddCors(options => options.AddPolicy("AllowCors",builder => 
            //    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    var isOptions = _builder.ApplicationServices.GetRequiredService<IOptions<IdentityServerConfig>>().Value;
                    options.Authority = isOptions.Authority;
                    options.ApiName = isOptions.ApiName;
                    options.ApiSecret = isOptions.ApiSecret;
                    options.RequireHttpsMetadata = isOptions.RequireHttpsMetadata;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            _builder = app;

            app.UseAuthentication();
            //app.UseCors("AllowCors");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
