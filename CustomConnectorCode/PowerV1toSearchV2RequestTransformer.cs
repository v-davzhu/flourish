
namespace ProjectFlourish.CustomConnectorCode
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal class PowerV1toSearchV2RequestTransformer
    {
        public static readonly string MessageRequestFormat = @"
            {{
              ""ContentSources"": {0},
              ""From"": {1},
              ""Size"": {2},
              ""Query"": {{
                ""QueryString"": ""{3}""}},
              ""EntityType"": ""Message"",
              ""BypassResultTypes"": false,
              ""PreferredResultSourceFormat"": ""AdaptiveCardTemplateBinding"",
              ""Sort"": [
                {{
                  ""Field"": ""Score"",
                  ""SortDirection"": ""Desc""
                }}],
                ""ResultsMerge"": {{
                    ""Type"": ""Interleaved""
                }},
                ""Fields"": [
                ""Subject"",
                ""Weblink"",
                ""Extension_SkypeSpaces_ConversationPost_Extension_Topic_String""
                ]
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
                }}],
                ""ResultsMerge"": {{
                    ""Type"": ""Interleaved""
                }},
                ""Fields"": [
                    ""DisplayName"",
                    ""EmailAddresses"",
                    ""JobTitle""
                ],
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
                    ""QueryString"": ""{3}""}},
               ""Fields"": {4},
              ""EntityType"": ""File"",
              ""BypassResultTypes"": false,
              ""PreferredResultSourceFormat"": ""AdaptiveCardTemplateBinding"",
              ""Sort"": [
                {{
                  ""Field"": ""Score"",
                  ""SortDirection"": ""Desc""
                }}],
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

        public static readonly string CommanRequestEnvelope = @"{
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
                public IList<string> ContentSources;
                public IList<string> Fields;

            };
        };

        public async Task<string> Transform(string powerRequestString)
        {
            var powerRequest = JsonConvert.DeserializeObject<PowerRequest>(powerRequestString);

            var substrateRequest = JObject.Parse(CommanRequestEnvelope);
            substrateRequest["EntityRequests"] = new JArray();

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

            (substrateRequest["EntityRequests"] as JArray).Add(JObject.Parse(string.Format(requestFormat, contentSources, powerRequest.From, powerRequest.Size, powerRequest.SearchPhrase, fields)));

            return JsonConvert.SerializeObject(substrateRequest);
        }
    }
}
