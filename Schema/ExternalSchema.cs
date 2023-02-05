namespace ProjectFlourish.MockData
{
    internal class ExternalSchema
    {
        public static readonly string Data = @"
            {
              ""ContentSources"": [ ""Connectors"" ],
              ""Fields"": {
                ""URL"": {
                    ""type"": ""string"",
                    ""description"": ""The target URL of the item in the source system""
                },
                ""Description"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""SubstrateContentDomainId"": { 
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""SubstrateLocationId"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""DocumentId"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""ImmutableEntryId"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""Id"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""Title"": {
                    ""type"": ""string"",
                    ""description"": ""The title for the item that you want shown in search and other experiences""
                },
                ""HitHighlightedProperties"": {
                    ""type"": ""string"",
                    ""description"": """"
                }
              }
            }
        ";
    }
}
