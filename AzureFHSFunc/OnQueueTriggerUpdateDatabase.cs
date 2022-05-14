using AzureFHSFunc.Data;
using AzureFHSFunc.Models;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFHSFunc
{
    public class OnQueueTriggerUpdateDatabase
    {
        private readonly AzureFHSDbContext _dbContext;

        public OnQueueTriggerUpdateDatabase(AzureFHSDbContext dbContext)
        {
                _dbContext = dbContext;
        }

        [FunctionName("OnQueueTriggerUpdateDatabase")]
        public void Run([QueueTrigger("SalesRequestInbound", Connection = "AzureWebJobsStorage")]SalesRequest myQueueItem,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            myQueueItem.Status = "Submitted";

            _dbContext.SalesRequests.Add(myQueueItem);
            _dbContext.SaveChanges();
        }
    }
}
