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
    public static class EntitySchema3S
    {
        [FunctionName("EntitySchema3S")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            req.Headers.TryGetValues("Authorization", out var authorization);
            log.LogInformation($"Authorization Size: {authorization?.FirstOrDefault()?.Length}");

            //var queryParams = req.GetQueryNameValuePairs();

            var queryParams = HttpUtility.ParseQueryString(req.RequestUri.Query);
            var entityType = queryParams["entityType"];
            var entitySchema = string.Empty;

            switch (entityType)
            {
                case "File":
                    entitySchema = FileSchema.Data;
                    break;
                case "Message":
                    entitySchema = MessageSchema.Data;
                    break;
                case "People":
                    entitySchema = PeopleSchema.Data;
                    break;
                case "External":
                    entitySchema = ExternalSchema.Data;
                    break;
                default:
                    break;
            }

            var responseMessage = entitySchema; // JsonConvert.SerializeObject(entitySchema);
            var stringContent = new StringContent(responseMessage, Encoding.UTF8, "application/json");

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = stringContent;
            
            return response;
        }
    }
}
