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
    public static class SingleEntityAdvancedOptionsSchema
    {
        [FunctionName("singleEntityAdvancedOptionsSchema")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //var queryParams = req.GetQueryNameValuePairs();

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
            HttpRequestMessage httpRequest3S = new HttpRequestMessage(method: HttpMethod.Get, "https://projectflourish.azurewebsites.net/api/EntitySchema3S?entitytype=File");

            HttpClient httpClient = new HttpClient();
            var response3S = await httpClient.SendAsync(httpRequest3S);

            var entitySchema3S = await response3S.Content.ReadAsStringAsync();


            var obj = JObject.Parse(entitySchema3S);

            return JsonConvert.SerializeObject(JObject.Parse(string.Format(AdvancedOptionsSwaggerView, JsonConvert.SerializeObject(obj["ContentSources"]), JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(obj["Fields"])).Keys))));
        }

        private static readonly string AdvancedOptionsSwaggerView = @"
                {{
                  ""type"": ""object"",
                  ""properties"": {{
                    ""ContentSources"": {{
                      ""type"": ""array"",
                      ""items"": {{
                        ""type"": ""string"",
                        ""enum"": {0}
                      }},
                      ""default"": {0}
                    }},
                    ""Fields"": {{
                      ""type"": ""array"",
                      ""items"": {{
                        ""type"": ""string"",
                        ""enum"": {1}
                      }},
                      ""default"": [
                        ""Field-Bool"",
                        ""Field-Int"",
                        ""Field-Str""
                      ]
                    }}
                  }},
                  ""x-ms-visibility"": ""advanced""
                }}
            ";
    }
}
