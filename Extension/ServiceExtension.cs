using FutsalSemuaSenang.Models;
using FutsalSemuaSenang.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FutsalSemuaSenang.Extension
{
    public static class ServiceExtension
    {
        public static void KonfigurasiUTSRandi(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<AppDbContext>(o =>
            {
                o.UseMySQL(Configuration.GetConnectionString("mysql"));
            });

            services.AddSingleton<OTPService>();

            services.AddSingleton<IdUserService>();

            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", options =>
                {
                    options.LoginPath = "/home/login";
                });
        }
    }
}