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
        public static readonly string Entities3SEndpoint = "https://projectflourish.azurewebsites.net/api/Entities3S";
        public static readonly string AdvancedOptions3SEndpoint = @"https://projectflourish.azurewebsites.net/api/EntitySchema3S?entitytype={0}";
        public static readonly string ResponseSchema3SEndpoint = @"https://projectflourish.azurewebsites.net/api/EntitySchema3S?entitytype={0}";

        public override StringContent CreateJsonContent(string serializedJson)
        {
            return new StringContent(serializedJson, Encoding.UTF8, "application/json");
        }

        public override async Task<HttpResponseMessage> ExecuteAsync()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            if (this.Context.OperationId == "GetEntities")
            {
                HttpRequestMessage httpRequest3S = new HttpRequestMessage(method: HttpMethod.Get, Entities3SEndpoint);
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

                HttpRequestMessage httpRequest3S = new HttpRequestMessage(method: HttpMethod.Get, string.Format(AdvancedOptions3SEndpoint, entityType));

                var response3S = await this.Context.SendAsync(httpRequest3S, this.CancellationToken);
                var entitySchema3S = await response3S.Content.ReadAsStringAsync();

                string advancedOptionsSchemaSwagger = GetAdvancedOptionsSchemaSwagger(entitySchema3S);

                response.Content = CreateJsonContent(advancedOptionsSchemaSwagger);
                return response;
            }
            else if (this.Context.OperationId == "GetResponseSchema")
            {
                var queryParams = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
                var entityType = queryParams["entityType"];

                if (string.IsNullOrEmpty(entityType))
                {
                    response.Content = CreateJsonContent(string.Empty);
                    return response;
                }

                HttpRequestMessage httpRequest3S = new HttpRequestMessage(method: HttpMethod.Get, string.Format(ResponseSchema3SEndpoint, entityType));

                var response3S = await this.Context.SendAsync(httpRequest3S, this.CancellationToken);
                var responseSchema3S = await response3S.Content.ReadAsStringAsync();

                string responseSchemaSwagger = GetResponseSchemaSwagger(responseSchema3S);

                response.Content = CreateJsonContent(responseSchemaSwagger);
                return response;

            }

            response.Content = CreateJsonContent(string.Empty);
            return response;
        }

        private string GetAdvancedOptionsSchemaSwagger(string entitySchema3S)
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

            return JsonConvert.SerializeObject(advancedOptionsSchema);
        }

        private string GetResponseSchemaSwagger(string responseSchema3S)
        {
            var objResponseSchema3S = JObject.Parse(responseSchema3S);

            var responseSchema = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["results"] = new JObject
                    {
                        ["type"] = "array",
                        ["items"] = new JObject
                        {
                            ["type"] = "object",
                            ["properties"] = objResponseSchema3S["Fields"]
                        }
                    },
                    ["diagnostics"] = new JObject
                    {
                        ["type"] = "object"
                    }
                }
            };

            return JsonConvert.SerializeObject(responseSchema);
        }
    }
}