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

            return FilEntitySchema;
        }

        private static readonly string FileSchemaFrom3S = @"
            {
              ""ContentSources"": [ ""SPO"", ""ODB"" ],
              ""Fields"": {
                ""Field-A"": {
                  ""type"": ""boolean"",
                  ""default"": ""false""
                },
                ""Field-B"": {
                  ""type"": ""integer"",
                  ""default"": ""-99""
                }
              }
            }
        ";

        private static readonly string FilEntitySchema = @"
                {
                  ""type"": ""object"",
                  ""properties"": {
                    ""Results"": {
                      ""type"": ""array"",
                      ""items"": {
                        ""type"": ""object"",
                        ""properties"": {
                          ""Field1"": {
                            ""type"": ""string""
                          },
                          ""Field2"": {
                            ""type"": ""integer""
                          },
                          ""File-Field1"": {
                            ""type"": ""string""
                          },
                          ""File-Field2"": {
                            ""type"": ""boolean""
                          }
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
