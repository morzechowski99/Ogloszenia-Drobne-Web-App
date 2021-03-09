using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ogłoszenia_Drobne_Web_App.Data;

[assembly: HostingStartup(typeof(Ogłoszenia_Drobne_Web_App.Areas.Identity.IdentityHostingStartup))]
namespace Ogłoszenia_Drobne_Web_App.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}