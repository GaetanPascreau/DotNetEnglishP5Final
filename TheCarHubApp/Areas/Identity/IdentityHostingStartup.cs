using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheCarHubApp.Areas.Identity.Data;
using TheCarHubApp.Data;

[assembly: HostingStartup(typeof(TheCarHubApp.Areas.Identity.IdentityHostingStartup))]
namespace TheCarHubApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<TheCarHubAppContext>(options =>
                    options.UseSqlServer(
                context.Configuration.GetConnectionString("TheCarHubAppContextConnection")));
            //context.Configuration.GetConnectionString("AzureConnection2")));

                services.AddDefaultIdentity<TheCarHubAppUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<TheCarHubAppContext>();
            });
        }
    }
}