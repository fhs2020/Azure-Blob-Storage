using AzureFHSFunc.Data;
using AzureFHSFunc.Models;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureFHSFunc
{
    public class UpdateStatusToCompletedAndSendEmail
    {
        private readonly AzureFHSDbContext _dbContext;

        public UpdateStatusToCompletedAndSendEmail(AzureFHSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [FunctionName("UpdateStatusToCompletedAndSendEmail")]
        public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            IEnumerable<SalesRequest> salesRequestFromDb = _dbContext.SalesRequests.Where(u => u.Status == "Image Processed");

            foreach (SalesRequest salesRequest in salesRequestFromDb)
            {
                //for each request update status 
                salesRequest.Status = "Completed";
            }

            _dbContext.UpdateRange(salesRequestFromDb); 
            _dbContext.SaveChanges();
        }
    }
}
