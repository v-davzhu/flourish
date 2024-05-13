namespace ProjectFlourish.MockData
{
    internal class FileSchema
    {
        public static readonly string Data = @"
            {
              ""ContentSources"": [ ""SharePoint"", ""OneDriveBusiness"" ],
              ""Fields"": {
                ""Description"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": true
                },
                ""DocId"": {
                    ""type"": ""integer"",
                    ""description"": ""Sharepoint Item document id"",
                    ""isQueryable"": false
                },
                ""Filename"": { 
                    ""type"": ""string"",
                    ""description"": ""File name of file item"",
                    ""isQueryable"": true
                },
                ""Path"": {
                    ""type"": ""string"",
                    ""description"": ""Full path to file"",
                    ""isQueryable"": true
                },
                ""Title"": {
                    ""type"": ""string"",
                    ""description"": ""Title of document file item"",
                    ""isQueryable"": true
                },
                ""Rank"": {
                    ""type"": ""integer"",
                    ""description"": ""Rank of file"",
                    ""isQueryable"": false
                },
                ""SiteId"": {
                    ""type"": ""string"",
                    ""description"": ""SharepointItem site id"",
                    ""isQueryable"": false
                },
                ""UniqueId"": {
                    ""type"": ""string"",
                    ""description"": ""The unique id for this SharePoint item in SharePoint"",
                    ""isQueryable"": false
                },
                ""WebId"": {
                    ""type"": ""string"",
                    ""description"": ""SharepointItem web id"",
                    ""isQueryable"": false
                },
                ""contentclass"": {
                    ""type"": ""string"",
                    ""description"": ""SharepointItem content class e.g.: STS_Site, STS_List_GenericList, STS_List, STS_ListItem_DocumentLibrary, STS_ListItem_WebPageLibrary, STS_ListItem_MySiteDocumentLibrary, STS_Web"",
                    ""isQueryable"": false
                },
                ""IsExternalContent"": {
                    ""type"": ""boolean"",
                    ""description"": ""Sharepoint is external content"",
                    ""isQueryable"": false
                },
                ""ListId"": {
                    ""type"": ""string"",
                    ""description"": ""SharepointItem list id"",
                    ""isQueryable"": false
                },
                ""OriginalPath"": {
                    ""type"": ""string"",
                    ""description"": ""Original path to file"",
                    ""isQueryable"": false
                },
                ""CollapsingStatus"": {
                    ""type"": ""integer"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""Id"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""IsDocument"": {
                    ""type"": ""boolean"",
                    ""description"": """",
                    ""isQueryable"": true
                },
                ""SiteDescription"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""SiteTemplateId"": {
                    ""type"": ""integer"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""SiteTitle"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""WebTemplate"": {
                    ""type"": ""string"",
                    ""description"": ""SharepointeItem web template"",
                    ""isQueryable"": false
                },
                ""HitHighlightedProperties"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""CultureName"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""UrlZone"": {
                    ""type"": ""integer"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""GeoLocationSource"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""Author"": {
                    ""type"": ""string"",
                    ""description"": ""Author of document file item"",
                    ""isQueryable"": true
                }
              }
            }
        ";
    }
}
