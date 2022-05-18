using AzureFHSFunc;
using AzureFHSFunc.Data;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;

[assembly: WebJobsStartup(typeof(Startup))]
namespace AzureFHSFunc
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("AzureSQLDatabase");

            builder.Services.AddDbContext<AzureFHSDbContext>(options => options.UseSqlServer(connectionString));

            

            builder.Services.BuildServiceProvider();
        }
    }
}
