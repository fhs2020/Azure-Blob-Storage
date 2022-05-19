using AzureFHSFunc;
using AzureFHSFunc.Data;
using AzureFHSFunc.Interfaces;
using AzureFHSFunc.Models;
using AzureFHSFunc.Services;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;

[assembly: WebJobsStartup(typeof(Startup))]
namespace AzureFHSFunc
{
    public class Startup : IWebJobsStartup
    {

         public IConfiguration Configuration { get; }

        public void Configure(IWebJobsBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("AzureSQLDatabase");

            builder.Services.AddDbContext<AzureFHSDbContext>(options => options.UseSqlServer(connectionString));

           // builder.Services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            builder.Services.AddTransient<IMailService, MailService>();

            builder.Services.BuildServiceProvider();
        }
    }
}
