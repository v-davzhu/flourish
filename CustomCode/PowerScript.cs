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
using ProjectFlourish.CustomConnectorCode;

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

    // Develop Script class here and copy over to Custom Code
    public class Script : ScriptBase
    {
        //public static readonly string Entities3SEndpoint = "https://projectflourish.azurewebsites.net/api/Entities3S";
        //public static readonly string EntitySchema3SEndpoint = @"https://projectflourish.azurewebsites.net/api/EntitySchema3S?entitytype={0}";
        public static readonly string Entities3SEndpoint = "https://dzsearchfunc.azurewebsites.net/api/Entities3S";
        public static readonly string EntitySchema3SEndpoint = @"https://dzsearchfunc.azurewebsites.net/api/EntitySchema3S?entitytype={0}";
        public static readonly string SingleEntitySearch3SEndpoint = "https://substrate.office.com/search/api/v2/query?debug=1&setflight=NuowoFlight&ClientAppOverride=e7157397-db36-4c3f-9e7d-905509378877";
        public static readonly string Operators3SEndpoint = "https://dzsearchfunc.azurewebsites.net/api/Operators3S";

        // Ignore this when copying
        public override StringContent CreateJsonContent(string serializedJson)
        {
            return new StringContent(serializedJson, Encoding.UTF8, "application/json");
        }

        public override async Task<HttpResponseMessage> ExecuteAsync()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            
            this.Context.Request.Headers.TryGetValues("Authorization", out var auth);

            if (this.Context.OperationId == "GetOperators") // get list of operators
            {
                HttpRequestMessage httpRequest3S = new HttpRequestMessage(method: HttpMethod.Get, Operators3SEndpoint);
                if (auth != null && auth.Any())
                {
                    httpRequest3S.Headers.Add("Authorization", auth);
                }
                var response3S = await this.Context.SendAsync(httpRequest3S, this.CancellationToken);
                var response3SContent = await response3S.Content.ReadAsStringAsync();

                response.Content = CreateJsonContent(response3SContent);
                return response;
            }

            if (this.Context.OperationId == "GetEntities") // get list of entities
            {
                HttpRequestMessage httpRequest3S = new HttpRequestMessage(method: HttpMethod.Get, Entities3SEndpoint);
                if (auth != null && auth.Any())
                {
                    httpRequest3S.Headers.Add("Authorization", auth);
                }
                var response3S = await this.Context.SendAsync(httpRequest3S, this.CancellationToken);
                var response3SContent = await response3S.Content.ReadAsStringAsync();

                response.Content = CreateJsonContent(response3SContent);
                return response;
            }
            else if (this.Context.OperationId == "GetAdvancedOptionsSchema") // get advanced options schema
            {
                var queryParams = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
                var entityType = queryParams["entityType"];


                if (string.IsNullOrEmpty(entityType))
                {
                    response.Content = CreateJsonContent(string.Empty);
                    return response;
                }

                HttpRequestMessage httpRequest3S = new HttpRequestMessage(method: HttpMethod.Get, string.Format(EntitySchema3SEndpoint, entityType));
                if (auth != null && auth.Any())
                {
                    httpRequest3S.Headers.Add("Authorization", auth);
                }
                var response3S = await this.Context.SendAsync(httpRequest3S, this.CancellationToken);
                var entitySchema3S = await response3S.Content.ReadAsStringAsync();

                string advancedOptionsSchemaSwagger = GetAdvancedOptionsSchemaSwagger(entitySchema3S);

                response.Content = CreateJsonContent(advancedOptionsSchemaSwagger);
                return response;
            }
            else if (this.Context.OperationId == "GetResponseSchema") // get response schema
            {
                var queryParams = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
                var entityType = queryParams["entityType"];

                if (string.IsNullOrEmpty(entityType))
                {
                    response.Content = CreateJsonContent(string.Empty);
                    return response;
                }

                HttpRequestMessage httpRequest3S = new HttpRequestMessage(method: HttpMethod.Get, string.Format(EntitySchema3SEndpoint, entityType));
                if (auth != null && auth.Any())
                {
                    httpRequest3S.Headers.Add("Authorization", auth);
                }
                var response3S = await this.Context.SendAsync(httpRequest3S, this.CancellationToken);
                var responseSchema3S = await response3S.Content.ReadAsStringAsync();

                string responseSchemaSwagger = GetResponseSchemaSwagger(responseSchema3S);

                response.Content = CreateJsonContent(responseSchemaSwagger);
                return response;

            }
            else // get mock 3S single entity search response
            {
                var powerRequestString = await this.Context.Request.Content.ReadAsStringAsync();
                var string3SRequest = PowerV1ToSearchV2RequestTransform(powerRequestString);

                HttpRequestMessage httpRequest3S = new HttpRequestMessage(method: HttpMethod.Post, SingleEntitySearch3SEndpoint);
                httpRequest3S.Content = new StringContent(string3SRequest, Encoding.UTF8, "application/json");
                if (auth != null && auth.Any())
                {
                    httpRequest3S.Headers.Add("Authorization", auth);
                }
                var response3S = await this.Context.SendAsync(httpRequest3S, this.CancellationToken);
                var results3S = await response3S.Content.ReadAsStringAsync();

                if (response3S.StatusCode != HttpStatusCode.OK)
                {
                    return response3S;
                }

                var table = SearchV2toPowerV1ResponseTransformer(results3S);
                response.Content = CreateJsonContent(table);
                return response;
            }
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
                        ["default"] = JToken.FromObject(fields.Keys.Take(2))
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


        private string PowerV1ToSearchV2RequestTransform(string powerRequestString)
        {
            var powerRequest = JsonConvert.DeserializeObject<PowerRequest>(powerRequestString);

            string requestFormat = string.Empty;
            switch (powerRequest.EntityType)
            {
                case "File":
                    requestFormat = FileRequestFormat;
                    break;
                case "Message":
                    requestFormat = MessageRequestFormat;
                    break;
                case "People":
                    requestFormat = PeopleRequestFormat;
                    break;
                default:
                    //unknown
                    throw new Exception($"Undefined entity type: {powerRequest.EntityType}");
            }

            var contentSources = JsonConvert.SerializeObject(powerRequest.AdvancedOptions.ContentSources);
            var fields = JsonConvert.SerializeObject(powerRequest.AdvancedOptions.Fields);

            var entityRequests = new JArray();
            entityRequests.Add(JObject.Parse(string.Format(requestFormat, contentSources, powerRequest.From, powerRequest.Size, powerRequest.SearchPhrase, fields)));
            
            var substrateRequest = JObject.Parse(CommonRequestEnvelope);
            substrateRequest["EntityRequests"] = entityRequests;

            return JsonConvert.SerializeObject(substrateRequest);
        }

        private string SearchV2toPowerV1ResponseTransformer(string results3S)
        {
            var jsonObjectResults3S = JObject.Parse(results3S);

            var rows = Tabularize(jsonObjectResults3S);

            var response = new PowerResponse
            {
                Results = rows,
                Diagnostics = new Dictionary<string, object>() { { "trace-id", "123456789" } }
            };

            return JsonConvert.SerializeObject(response);
        }

        public struct PowerRequest
        {
            public string SearchPhrase;
            public string EntityType;
            public bool SortByRelevance;
            public int From;
            public int Size;
            public AdvOptions AdvancedOptions;

            public struct AdvOptions
            {
                public List<string> ContentSources;
                public List<string> Fields;

            };
        };

        public static readonly string CommonRequestEnvelope = @"{
                ""Cvid"": ""72aff190-3fa5-44a8-845d-96f24eb01942"",
                ""Scenario"": {
                    ""Dimensions"": [
                        {
                        ""DimensionName"": ""QueryType"",
                        ""DimensionValue"": ""Conversation""
                        }
                    ],
                    ""Name"": ""owa.react""
                },
                ""WholePageRankingOptions"": {
                    ""EnableEnrichedRanking"": true,
                    ""EnableLayoutHints"": true,
                    ""SupportedSerpRegions"": [
                        ""MainLine""
                    ]
                }
            }";

        public static readonly string MessageRequestFormat = @"
            {{
              ""ContentSources"": {0},
              ""From"": {1},
              ""Size"": {2},
              ""Query"": {{
                ""QueryString"": ""{3}""
              }},
              ""Fields"": {4},
              ""EntityType"": ""Message"",
              ""BypassResultTypes"": false,
              ""PreferredResultSourceFormat"": ""AdaptiveCardTemplateBinding"",
              ""Sort"": [
                    {{
                      ""Field"": ""Score"",
                      ""SortDirection"": ""Desc""
                    }}
                ],
                ""ResultsMerge"": {{
                    ""Type"": ""Interleaved""
                }}
            }}";


        public static readonly string PeopleRequestFormat = @"
            {{
              ""ContentSources"": {0},
              ""From"": {1},
              ""Size"": {2},
              ""Query"": {{
                ""QueryString"": ""{3}""
              }},
              ""EntityType"": ""People"",
              ""BypassResultTypes"": false,
              ""PreferredResultSourceFormat"": ""AdaptiveCardTemplateBinding"",
              ""Sort"": [
                    {{
                      ""Field"": ""Score"",
                      ""SortDirection"": ""Desc""
                    }}
                ],
                ""ResultsMerge"": {{
                    ""Type"": ""Interleaved""
                }},
                ""Fields"": {4},
                ""EnableQueryUnderstanding"": false,
                ""EnableSpeller"": false,
                ""IdFormat"": 0,
                ""Filter"": {{
                        ""And"": [
                            {{
                                ""Term"": {{""PeopleType"": ""Person""}}
                            }},
                            {{
                                ""Term"": {{""PeopleSubtype"": ""OrganizationUser""}}
                            }}
                        ]
                }}
            }}";


        public static readonly string FileRequestFormat = @"
            {{
              ""ContentSources"": {0},
              ""From"": {1},
              ""Size"": {2},
              ""Query"": {{
                    ""QueryString"": ""{3}""
              }},
              ""Fields"": {4},
              ""EntityType"": ""File"",
              ""BypassResultTypes"": false,
              ""PreferredResultSourceFormat"": ""AdaptiveCardTemplateBinding"",
              ""Sort"": [
                {{
                  ""Field"": ""Score"",
                  ""SortDirection"": ""Desc""
                }}
                ],
                ""ResultsMerge"": {{
                    ""Type"": ""Interleaved""
                }},
                ""EnableQueryUnderstanding"": false,
                ""EnableSpeller"": false,
                ""IdFormat"": 0,
                ""HitHighlight"": {{
                    ""HitHighlightedProperties"": [
                      ""HitHighlightedSummary""
                    ],
                    ""SummaryLength"": 200
                  }}
            }}";

        public struct PowerResponse
        {
            public List<Dictionary<string, object>> Results;
            public Dictionary<string, object> Diagnostics;
        }

        private List<Dictionary<string, object>> Tabularize(JObject jsonObject)
        {
            var rows = new List<Dictionary<string, object>>();
            var esNo = 0;
            foreach (var entitySet in jsonObject?["EntitySets"] ?? new JArray())
            {
                var jObjEntitySet = entitySet as JObject;
                var rsNo = 0;
                foreach (var resultSet in jObjEntitySet?["ResultSets"] ?? new JArray())
                {
                    var jObjResultSet = resultSet as JObject;

                    foreach (var result in jObjResultSet?["Results"] ?? new JArray())
                    {
                        var jObjResult = result as JObject;
                        var resultDict = Flatten(jObjResult);
                        rows.Add(resultDict);
                    }
                    rsNo++;
                }
                esNo++;
            }

            return rows;
        }

        /// <summary>
        /// FROM: https://github.com/GFoley83/JsonFlatten/blob/master/JsonFlatten/JsonExtensions.cs
        /// Using code instead of nuget for hack
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <param name="includeNullAndEmptyValues"></param>
        /// <returns></returns>
        private Dictionary<string, object> Flatten(JObject jsonObject, bool includeNullAndEmptyValues = true)
        {
            return jsonObject?
                .Descendants()?
                .Where(p => !p?.Any() ?? false)?
                .Aggregate(new Dictionary<string, object>(), (properties, jToken) =>
                {
                    var value = (jToken as JValue)?.Value;

                    if (!includeNullAndEmptyValues)
                    {
                        if (value?.Equals("") == false)
                        {
                            properties.Add(jToken.Path, value);
                        }

                        return properties;
                    }

                    var strVal = jToken?.Value<object>()?.ToString()?.Trim();
                    if (strVal?.Equals("[]") == true)
                    {
                        value = Enumerable.Empty<object>();
                    }
                    else if (strVal?.Equals("{}") == true)
                    {
                        value = new object();
                    }

                    //properties.Add(jToken.Path, value);

                    var name = (jToken?.Parent as JProperty)?.Name;

                    // TODO: handling of values that are array of objects
                    if (jToken?.Parent.Type == JTokenType.Array)
                    {
                        name = (jToken?.Parent?.Parent as JProperty)?.Name;

                        if (!properties.ContainsKey(name))
                        {
                            properties.Add(name, new List<object>());
                        }

                        (properties[name] as List<object>).Add(value);
                    } else if (!string.IsNullOrEmpty(name) && !properties.ContainsKey(name))
                    {
                        properties.Add(name, value);
                    }

                    return properties;
                });
        }
    }
}