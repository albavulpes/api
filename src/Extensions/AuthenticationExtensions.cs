using System.Text;
using System.Threading.Tasks;
using AlbaVulpes.API.Models.Config;
using AlbaVulpes.API.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AlbaVulpes.API.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void AddCookieAuthentication(this IServiceCollection services, IConfiguration config)
        {
            // configure strongly typed settings objects
            var appSettings = config.GetSection("AppSettings").Get<AppSettings>();
            var cookieName = appSettings.AuthCookieName;

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = cookieName;
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = (redirectContext) =>
                        {
                            redirectContext.HttpContext.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}