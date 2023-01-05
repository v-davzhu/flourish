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
using PowerScript;

namespace ProjectFlourish.CustomConnectorCode
{
    public static class SingleEntitySearch
    {
        public static Script script = new Script();

        [FunctionName("singleEntitySearch")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

            script.Context = new ScriptContext();
            script.Context.OperationId = "SingleEntitySearch";
            script.Context.Request = req;

            return await script.ExecuteAsync();

            /*log.LogInformation("C# HTTP trigger function processed a request.");

            var powerRequestString = await req.Content.ReadAsStringAsync();

            log.LogInformation($"Power Request: \n{powerRequestString}\n");

            var queryParams = HttpUtility.ParseQueryString(req.RequestUri.Query);
            //var entityType = queryParams["entityType"];
            var entitySchema = string.Empty
            entitySchema = await GetFileEntityResultsAsync(powerRequestString);

            var responseMessage = entitySchema; // JsonConvert.SerializeObject(entitySchema);
            var stringContent = new StringContent(responseMessage, Encoding.UTF8, "application/json");
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = stringContent;

            return response;*/
        }


        private static async Task<string> GetFileEntityResultsAsync(string powerRequestString)
        {
            var requestTransformer = new PowerV1toSearchV2RequestTransformer();
            var string3SRequest = await requestTransformer.Transform(powerRequestString);

            HttpRequestMessage httpRequest3S = new HttpRequestMessage(method: HttpMethod.Post, "https://projectflourish.azurewebsites.net/api/EntitySearch3S?entitytype=File");
            httpRequest3S.Content = new StringContent(string3SRequest, Encoding.UTF8, "application/json");
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
