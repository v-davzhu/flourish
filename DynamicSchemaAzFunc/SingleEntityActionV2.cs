using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;

namespace Company.Function
{
    public abstract class SwaggerBase
    {
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Description { get; set; }

        [JsonProperty("x-ms-summary", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Summary { get; set; }

        [JsonProperty("x-ms-visibility", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Visibility { get; set; }
    }

    public class Swagger<T> : SwaggerBase
    {
        private string _type = "object";
        public Swagger(string type = null)
        {
            if (type != null)
            {
                Type = type;
            }
        }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Type { get { return _type; } protected set { _type = value; } }

        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        public virtual Dictionary<string, T> Properties { get; set; }

        [JsonProperty("enum", NullValueHandling = NullValueHandling.Ignore)]
        public virtual List<T> Enum { get; set; }

        [JsonProperty("default", NullValueHandling = NullValueHandling.Ignore)]
        public virtual T Default { get; set; }
    }

    public class SwaggerArray<T> : SwaggerBase
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Type { get { return "array"; } set => Type = value; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public virtual Swagger<T> Items { get; set; }

        [JsonProperty("default", NullValueHandling = NullValueHandling.Ignore)]
        public virtual List<T> Default { get; set; }

        public SwaggerArray(List<T> options = null)
        {
            var arrayType = "object";
            if (typeof(T) == typeof(string))
            {
                arrayType = "string";
            }
            else if (typeof(T) == typeof(bool))
            {
                arrayType = "boolean";
            }
            else if (typeof(T) == typeof(int))
            {
                arrayType = "number";
            }

            Items = new Swagger<T>(arrayType)
            {
                Enum = options
            };

            Default = options;
        }
    }

    
    public class SwaggerInt : Swagger<int>
    {
        public override string Type { get { return "number"; } protected set => Type = value; }
    }

    public class SwaggerBool : Swagger<bool>
    {
        public override string Type { get { return "boolean"; } protected set => Type = value; }
    }

    public class SwaggerStr : Swagger<string>
    {
        public override string Type { get { return "string"; } protected set => Type = value; }
    }

    public class SwaggerObj : Swagger<object>
    {
        public SwaggerObj(string type = null)
        {
            Type = type ?? "object";
        }
    }

    public static class Entities
    {
        [FunctionName("Entities")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            var entities = new List<string>() { "External", "File", "Message", "People" };

            var json = JsonConvert.SerializeObject(entities);

            response.Content = new StringContent(json);

            return response;
        }
    }

    public static class SingleEntityRequestSchema
    {
        [FunctionName("SingleEntityRequestSchema")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            var queryDictionary = System.Web.HttpUtility.ParseQueryString(req.RequestUri.Query);
            string entityType = null;

            if (queryDictionary.HasKeys())
            {
                entityType = queryDictionary["entityType"];
            }

            string advancedOptions;
            switch (entityType)
            {
                case "File":
                    advancedOptions = new AdvancedOptions(
                        new List<string>() { "SharePoint", "OneDriveBusiness" },
                        new List<string>() { "Field1", "Field2", "Field3" }).ToString();
                    break;

                case "People":
                    advancedOptions = new AdvancedOptions(
                        new List<string>() { "Exchange", "SharePoint" },
                        new List<string>() { "FieldA", "FieldB", "FieldC" }).ToString();

                    break;

                case "Message":
                    advancedOptions = new AdvancedOptions(
                        new List<string>() { "Exchange", "Teams" },
                        new List<string>() { "FieldX", "FieldY", "FieldZ" }).ToString();
                    break;

                default:
                    advancedOptions = JsonConvert.SerializeObject(new Dictionary<string, object>()
                    {
                        {"", new SwaggerObj() }
                    });
                    
                    break;
            }

            response.Content = new StringContent(advancedOptions);

            return response;
        }

        class AdvancedOptions
        {
            private readonly Dictionary<string, object> schema;
            public AdvancedOptions(List<string> contentSources, List<string> fields)
            {
                schema = new Dictionary<string, object>()
                        {
                            {
                                "AdvancedOptions", new SwaggerObj()
                                {
                                    Visibility = "advanced",
                                    Properties = new Dictionary<string, object>()
                                    {
                                        {
                                            "ContentSources", new SwaggerArray<string>(contentSources)
                                        },
                                        {
                                            "Fields", new SwaggerArray<string>(fields)
                                        }
                                    }
                                }
                            }
                        };
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(schema);
            }
        }
    }

    public static class SingleEntityResponseSchema
    {
        [FunctionName("SingleEntityResponseSchema")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            var contentAsString = await req.Content.ReadAsStringAsync().ConfigureAwait(false);

            log.LogInformation(contentAsString);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            var responseSchema = new ResponseSchema(
                new List<(string, string)>() {("Field01", "string"), ("Field02", "boolean"), ("Field03", "number") });


            response.Content = new StringContent(responseSchema.ToString());

            return response;
        }

        class ResultsSchema
        {
            public ResultsSchema(List<(string name,string type)> fields)
            {
                var fieldsSchema = new Dictionary<string, Object>();
                foreach (var field in fields)
                {
                    fieldsSchema.Add(field.name, new SwaggerObj(field.type));
                }
                
                var schema = new SwaggerArray<object>()
                {
                    Items = new SwaggerObj()
                    {
                        Properties = fieldsSchema
                    }
                };
            }
        }

        class ResponseSchema
        {
            private readonly Dictionary<string, Object> schema;
            public ResponseSchema(List<(string name, string type)> fields)
            {
                schema = new Dictionary<string, Object>()
                {
                    { "Results", new ResultsSchema(fields) },
                    { "Diagbostics", new SwaggerObj() }
                };
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this.schema);
            }
        }
    }
}
