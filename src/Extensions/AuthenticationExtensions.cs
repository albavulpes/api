﻿using System;
using System.Text;
using System.Threading.Tasks;
using AlbaVulpes.API.Models.Config;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlbaVulpes.API.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void AddCookieAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var appSettings = config.GetSection("AppSettings").Get<AppSettings>();
            var cookieName = appSettings.AuthCookieName;

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = cookieName;
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.ExpireTimeSpan = TimeSpan.FromDays(1);
                    options.SlidingExpiration = true;
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = (redirectContext) =>
                        {
                            redirectContext.HttpContext.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        },
                        OnRedirectToAccessDenied = (redirectContext) =>
                        {
                            redirectContext.HttpContext.Response.StatusCode = 403;
                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}