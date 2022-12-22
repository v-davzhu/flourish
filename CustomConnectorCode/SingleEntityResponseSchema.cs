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

namespace ProjectFlourish.CustomConnectorCode
{
    public static class SingleEntityResponseSchema
    {
        [FunctionName("singleEntityResponseSchema")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var queryParams = HttpUtility.ParseQueryString(req.RequestUri.Query);
            var entityType = queryParams["entityType"];
            var entitySchema = string.Empty;

            switch (entityType)
            {
                case "File":
                    entitySchema = await GetFileEntitySchemaAsync();
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


        private static async Task<string> GetFileEntitySchemaAsync()
        {
            HttpRequestMessage httpRequest3S = new HttpRequestMessage(method: HttpMethod.Get, "https://projectflourish.azurewebsites.net/api/EntitySchema3S?entitytype=File"); //"https://projectflourish.azurewebsites.net/api/EntityResponseSchema3S?entitytype=File");

            HttpClient httpClient = new HttpClient();
            var response3S = await httpClient.SendAsync(httpRequest3S);

            var entitySchema3S = await response3S.Content.ReadAsStringAsync();

            var joFormat = JObject.Parse(ResponseSchemaSwaggerView);
            var joData = JObject.Parse(entitySchema3S);
            joFormat["properties"]["Results"]["items"]["properties"] = joData["Fields"];

            return JsonConvert.SerializeObject(joFormat);
        }

        private static readonly string ResponseSchemaSwaggerView = @"
                {
                  ""type"": ""object"",
                  ""properties"": {
                    ""Results"": {
                      ""type"": ""array"",
                      ""items"": {
                        ""type"": ""object"",
                        ""properties"": {
                            // actual fields go here
                        }
                      }
                    },
                    ""Diagnostics"": {
                      ""type"": ""object""
                    }
                  }
                }
            ";
    }
}
