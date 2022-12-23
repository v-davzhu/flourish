using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.Internal;

namespace PowerScript
{
    public abstract class ScriptBase
    {
        // Context object
        public IScriptContext Context { get; set; }

        // CancellationToken for the execution
        public CancellationToken CancellationToken { get; }

        // Helper: Creates a StringContent object from the serialized JSON
        public abstract StringContent CreateJsonContent(string serializedJson);

        // Abstract method for your code
        public abstract Task<HttpResponseMessage> ExecuteAsync();
    }

    public class ScriptContext : IScriptContext
    {
        public static readonly HttpClient httpClient = new HttpClient();

        public string CorrelationId { get; }

        public string OperationId { get; set; }
        public HttpRequestMessage Request { get; set; }

        public ILogger Logger { get; set; }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return httpClient.SendAsync(request, cancellationToken);
        }
    }

    public interface IScriptContext
    {
        // Correlation Id
        string CorrelationId { get; }

        // Connector Operation Id
        string OperationId { get; set; }

        // Incoming request
        HttpRequestMessage Request { get; set; }

        // Logger instance
        ILogger Logger { get; set; }

        // Used to send an HTTP request
        // Use this method to send requests instead of HttpClient.SendAsync
        Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken);
    }

    public class Script : ScriptBase
    {
        public override StringContent CreateJsonContent(string serializedJson)
        {
            return new StringContent(serializedJson, Encoding.UTF8, "application/json");
        }

        public override async Task<HttpResponseMessage> ExecuteAsync()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            if (this.Context.OperationId == "GetEntities")
            {
                HttpRequestMessage httpRequest3S = new HttpRequestMessage(method: HttpMethod.Get, "https://projectflourish.azurewebsites.net/api/Entities3S");
                var response3S = await this.Context.SendAsync(httpRequest3S, this.CancellationToken);
                var response3SContent = await response3S.Content.ReadAsStringAsync();

                response.Content = CreateJsonContent(response3SContent);
                return response;
            }
            else if (this.Context.OperationId == "GetAdvancedOptionsSchema")
            {
                var queryParams = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
                var entityType = queryParams["entityType"];

                if (string.IsNullOrEmpty(entityType))
                {
                    response.Content = CreateJsonContent(string.Empty);
                    return response;
                }

                HttpRequestMessage httpRequest3S = new HttpRequestMessage(method: HttpMethod.Get, $"https://projectflourish.azurewebsites.net/api/EntitySchema3S?entitytype={entityType}");

                var response3S = await this.Context.SendAsync(httpRequest3S, this.CancellationToken);
                var entitySchema3S = await response3S.Content.ReadAsStringAsync();

                var advancedOptionsSchema = ConvertToAdvancedOptionsSchema(entitySchema3S);

                response.Content = CreateJsonContent(JsonConvert.SerializeObject(advancedOptionsSchema));
                //entitySchema = JsonConvert.SerializeObject(JObject.Parse(string.Format(AdvancedOptionsSwaggerView, JsonConvert.SerializeObject(obj["ContentSources"]), JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(obj["Fields"])).Keys))));

                return response;
            }
            else if (this.Context.OperationId == "GetResponseSchema")
            {
                var queryParams = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
                var entityType = queryParams["entityType"];
                var entitySchema = string.Empty;

                switch (entityType)
                {
                    case "File":
                        HttpRequestMessage httpRequest3S = new HttpRequestMessage(method: HttpMethod.Get, "https://projectflourish.azurewebsites.net/api/EntitySchema3S?entitytype=File");

                        var response3S = await this.Context.SendAsync(httpRequest3S, this.CancellationToken);
                        var entitySchema3S = await response3S.Content.ReadAsStringAsync();

                        var joFormat = JObject.Parse(ResponseSchemaSwaggerView);
                        var joData = JObject.Parse(entitySchema3S);
                        joFormat["properties"]["Results"]["items"]["properties"] = joData["Fields"];

                        entitySchema = JsonConvert.SerializeObject(joFormat);
                        break;
                    default:
                        break;
                }

                response.Content = CreateJsonContent(entitySchema);
                return response;
            }

            return null;
        }

        private JObject ConvertToAdvancedOptionsSchema(string entitySchema3S)
        {
            var objEntitySchema3S = JObject.Parse(entitySchema3S);

            var contentSources = objEntitySchema3S["ContentSources"].ToObject<List<string>>();
            var fields = objEntitySchema3S["Fields"].ToObject<Dictionary<string, object>>();

            var advancedOptionsSchema = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["ContentSources"] = new JObject
                    {
                        ["type"] = "array",
                        ["items"] = new JObject
                        {
                            ["type"] = "string",
                            ["enum"] = JToken.FromObject(contentSources)
                        },
                        ["default"] = JToken.FromObject(contentSources)
                    },
                    ["Fields"] = new JObject
                    {
                        ["type"] = "array",
                        ["items"] = new JObject
                        {
                            ["type"] = "string",
                            ["enum"] = JToken.FromObject(fields.Keys)
                        },
                        ["default"] = JToken.FromObject(fields.Keys)
                    }
                }
            };

            return advancedOptionsSchema;
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
                  }}
                }}
            ";

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