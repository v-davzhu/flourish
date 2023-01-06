using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Linq;

namespace ProjectFlourish.Schema3S
{
    public static class SupportedEntities3S
    {
        [FunctionName("Entities3S")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            req.Headers.TryGetValues("Authorization", out var authorization);
            log.LogInformation($"Authorization Size: {authorization?.FirstOrDefault()?.Length}");

            var entities = new List<string>() {
                "File",
                "Message",
                "People",
                "External"
            };

            var responseMessage = JsonConvert.SerializeObject(entities);
            var stringContent = new StringContent(responseMessage, Encoding.UTF8, "application/json");
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = stringContent;
            
            return response;
        }
    }
}
