using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFHSFunc.Data;
using AzureFHSFunc.Models;
using System.Linq;

namespace AzureFHSFunc
{
    public class GroceryAPI
    {
        private readonly AzureFHSDbContext _context;

        public GroceryAPI(AzureFHSDbContext context)
        {
            _context = context;
        }


        [FunctionName("CreateGrocery")]
        public async Task<IActionResult> CreateGorcery(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "GroceryList")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Creating Gorcery List Item.");

           

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            GroceryItem_Upsert data = JsonConvert.DeserializeObject<GroceryItem_Upsert>(requestBody);

            var groceryItem = new GroceryItem
            {
                Name = data.Name
            };

            _context.GroceryItems.Add(groceryItem);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                var a = ex.Message;
            }
  

            return new OkObjectResult(groceryItem);
        }

        [FunctionName("GetGrocery")]
        public async Task<IActionResult> GetGorcery(
         [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GroceryList")] HttpRequest req,
         ILogger log)
        {
            log.LogInformation("Getting all the groceries.");


            return new OkObjectResult(_context.GroceryItems.ToList());
        }


        [FunctionName("GetGroceryById")]
        public async Task<IActionResult> GetGroceryById(
         [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GroceryList/{id}")] HttpRequest req,
         ILogger log, string id)
        {
            log.LogInformation("Getting Grocery List Item by ID.");

            var item = _context.GroceryItems.FirstOrDefault(x => x.Id == id);

            if(item == null) return new NotFoundResult();

 

            return new OkObjectResult(item);
        }

        [FunctionName("UpdateGrocery")]
        public async Task<IActionResult> UpdateGorcery(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "GroceryList/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Updating Gorcery List Item.");

            var item = _context.GroceryItems.FirstOrDefault(x => x.Id == id);

            if (item == null) return new NotFoundResult();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            GroceryItem_Upsert updatedData = JsonConvert.DeserializeObject<GroceryItem_Upsert>(requestBody);

            if(!string.IsNullOrEmpty(updatedData.Name)) 
                item.Name = updatedData.Name;

            _context.GroceryItems.Update(item);
            _context.SaveChanges();

            return new OkObjectResult(item);
        }

        [FunctionName("DeleteGrocery")]
        public async Task<IActionResult> DeleteGorcery(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "GroceryList/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Delete Gorcery List Item.");

            var item = _context.GroceryItems.FirstOrDefault(x => x.Id == id);

            if (item == null) return new NotFoundResult();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            GroceryItem_Upsert deletData = JsonConvert.DeserializeObject<GroceryItem_Upsert>(requestBody);

            _context.GroceryItems.Remove(item);
            _context.SaveChanges();


            return new OkResult();
        }
    }
}
