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
using ProjectFlourish.MockData;

namespace ProjectFlourish.Schema3S
{
    public static class EntitySearch3S
    {
        [FunctionName("EntitySearch3S")]
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
            entitySchema = GetFileEntitySearch();

            var responseMessage = entitySchema; // JsonConvert.SerializeObject(entitySchema);
            var stringContent = new StringContent(responseMessage, Encoding.UTF8, "application/json");
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = stringContent;
            
            return response;
        }


        private static string GetFileEntitySearch()
        {
            //return FileEntityResults3S;

            return FileAndPeopleResultsFrom3S.Data;
        }


        private static readonly string FileEntityResults3S = @"
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
