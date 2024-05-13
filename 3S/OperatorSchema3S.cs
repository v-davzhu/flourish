using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Linq;
using ProjectFlourish.MockData;

namespace ProjectFlourish.Schema3S
{
    public static class OperatorSchema3S
    {
        [FunctionName("OperatorSchema3S")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            req.Headers.TryGetValues("Authorization", out var authorization);
            log.LogInformation($"Authorization Size: {authorization?.FirstOrDefault()?.Length}");

            var queryParams = HttpUtility.ParseQueryString(req.RequestUri.Query);
            //var operatorType = queryParams["operatorType"];
            var operatorSchema = string.Empty;

            operatorSchema = GetOperatorSchema();

            var responseMessage = operatorSchema; // JsonConvert.SerializeObject(entitySchema);
            var stringContent = new StringContent(responseMessage, Encoding.UTF8, "application/json");

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = stringContent;
            
            return await Task.FromResult(response);
        }

        private static string GetOperatorSchema()
        {
            return OperatorSchema.Data;
        }
    }
}
