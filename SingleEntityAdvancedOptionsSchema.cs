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

namespace ProjectFlourish
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
                    entitySchema = GetFileEntitySchema();
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


        private static string GetFileEntitySchema()
        {
            //var data = File.ReadAllText(Path.Combine(".","AdvancedOptionsSchema.json"));

            //return FilEntitySchema;

            var obj = JObject.Parse(FileSchemaFrom3S);

            //string.Format(FilEntitySchemaFormat, obj["ContentSources"], obj["Fields"].Values());

            return JsonConvert.SerializeObject(JObject.Parse(string.Format(FilEntitySchemaFormat, JsonConvert.SerializeObject(obj["ContentSources"]), JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(obj["Fields"])).Keys))));
        }

        private static readonly string FileSchemaFrom3S = @"
            {
              ""ContentSources"": [ ""SPO"", ""ODB"" ],
              ""Fields"": {
                ""Field-Bool"": {
                  ""type"": ""boolean"",
                  ""default"": ""false""
                },
                ""Field-Int"": {
                  ""type"": ""integer"",
                  ""default"": ""-99""
                },
                ""Field-Str"": {
                  ""type"": ""string"",
                  ""default"": ""-99""
                },
                ""Field-Num"": {
                  ""type"": ""number"",
                  ""default"": ""-99.99""
                }
              }
            }
        ";

        private static readonly string FilEntitySchemaFormat = @"
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
