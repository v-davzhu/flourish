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

namespace ProjectFlourish
{
    public static class SingleEntitySearch
    {
        [FunctionName("singleEntitySearch")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var queryParams = HttpUtility.ParseQueryString(req.RequestUri.Query);
            //var entityType = queryParams["entityType"];
            var entitySchema = string.Empty;
            /*
            switch (entityType)
            {
                case "File":
                    entitySchema = GetFileEntitySchema();
                    break;
                default:
                    break;
            }*/
            entitySchema = GetFileEntitySchema();

            var responseMessage = entitySchema; // JsonConvert.SerializeObject(entitySchema);
            var stringContent = new StringContent(responseMessage, Encoding.UTF8, "application/json");
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = stringContent;
            
            return response;
        }


        private static string GetFileEntitySchema()
        {
            //var data = File.ReadAllText(Path.Combine(".","AdvancedOptionsSchema.json"));

            return FilEntitySchema;
        }


        private static readonly string FilEntitySchema = @"
                {
                  ""results"": [
                    {
                      ""Field-Bool"": false,
                      ""Field-Int"": 666,
                      ""Field-Str"": ""AString"",
                      ""Field-Num"": 88.06
                    },
                    {
                      ""Field-Bool"": true,
                      ""Field-Int"": 999,
                      ""Field-Str"": ""BString"",
                      ""Field-Num"": 60.88
                    },
                  ],
                  ""diagnostics"": {
                    ""Data"":  ""blahblah diags""
                  }
                }
            ";
    }
}
