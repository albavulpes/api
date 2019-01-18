using AlbaVulpes.API.Extensions;
using AlbaVulpes.API.Models.Config;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace AlbaVulpes.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());

            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddCors();

            services.AddAutoMapper();
            services.AddSecretsManager();

            services.AddMarten();
            services.AddUnitOfWork();

            services.AddValidator();

            services.AddCookieAuthentication(Configuration);

            //services.AddAuthentication().AddGoogle(googleOptions =>
            //{
            //    var authSettings = Configuration.GetSection("AuthSettings").Get<AuthSettings>();
            //    googleOptions.ClientId = authSettings.Google.ClientId;
            //    googleOptions.ClientSecret = authSettings.Google.ClientSecret;
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMapper autoMapper)
        {
            autoMapper.ConfigurationProvider.AssertConfigurationIsValid();

            app.UseCors(builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowCredentials()
            );

            app.UseAuthentication();

            app.UseMvc();

            app.UseConsoleLogging();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSeqLogging();
            }
        }
    }
}

