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
                    ""description"": ""The target URL of the item in the source system"",
                    ""isQueryable"": false
                },
                ""Description"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": true
                },
                ""DocumentId"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": true
                },
                ""Id"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""Title"": {
                    ""type"": ""string"",
                    ""description"": ""The title for the item that you want shown in search and other experiences"",
                    ""isQueryable"": true
                },
                ""ImmutableEntryId"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""HitHighlightedProperties"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                }
              }
            }
        ";
    }
}
