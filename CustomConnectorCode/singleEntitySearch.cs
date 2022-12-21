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
            entitySchema = await GetFileEntityResultsAsync();

            var responseMessage = entitySchema; // JsonConvert.SerializeObject(entitySchema);
            var stringContent = new StringContent(responseMessage, Encoding.UTF8, "application/json");
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = stringContent;

            return response;
        }


        private static async Task<string> GetFileEntityResultsAsync()
        {
            HttpRequestMessage httpRequest3S = new HttpRequestMessage(method: HttpMethod.Get, "https://projectflourish.azurewebsites.net/api/EntitySearch3S?entitytype=File");

            HttpClient httpClient = new HttpClient();
            var response3S = await httpClient.SendAsync(httpRequest3S);

            var results3S = await response3S.Content.ReadAsStringAsync();

            var jOResults = JObject.Parse(results3S);

            var transformer = new SearchV2toPowerV1ResponseTransformer();

            var table = await transformer.Transform(jOResults);

            return JsonConvert.SerializeObject(table);
        }
    }
}
