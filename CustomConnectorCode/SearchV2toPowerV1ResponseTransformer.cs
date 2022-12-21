
namespace ProjectFlourish.CustomConnectorCode
{
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal class SearchV2toPowerV1ResponseTransformer
    {
        public class PowerResponse
        {
            public IList<IDictionary<string, object>> Results;
            public IDictionary<string, object> Diagnostics;
        }

        public async Task<PowerResponse> Transform(JObject jsonObject)
        {
            var rows = await Tabularize(jsonObject);

            var response = new PowerResponse
            {
                Results = rows,
                Diagnostics = new Dictionary<string, object>() { { "trace-id", "123456789" } }
            };

            return response;
        }

        private async Task<IList<IDictionary<string, object>>> Tabularize(JObject jsonObject)
        {
            var rows = new List<IDictionary<string, object>>();
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
                        var resultDict = await Flatten(jObjResult);
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
        private async Task<IDictionary<string, object>> Flatten(JObject jsonObject, bool includeNullAndEmptyValues = true)
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
                    if (!string.IsNullOrEmpty(name) && !properties.ContainsKey(name))
                    {
                        properties.Add(name, value);
                    }

                    return properties;
                });
        }
    }
}
