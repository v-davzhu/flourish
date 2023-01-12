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
                    ""description"": """"
                },
                ""DocId"": {
                    ""type"": ""integer"",
                    ""description"": ""Sharepoint Item document id""
                },
                ""Filename"": { 
                    ""type"": ""string"",
                    ""description"": ""File name of file item""
                },
                ""Path"": {
                    ""type"": ""string"",
                    ""description"": ""Full path to file""
                },
                ""Rank"": {
                    ""type"": ""integer"",
                    ""description"": ""Description of file""
                },
                ""SiteId"": {
                    ""type"": ""string"",
                    ""description"": ""SharepointItem site id""
                },
                ""UniqueId"": {
                    ""type"": ""string"",
                    ""description"": ""The unique id for this SharePoint item in SharePoint""
                },
                ""WebId"": {
                    ""type"": ""string"",
                    ""description"": ""SharepointItem web id""
                },
                ""contentclass"": {
                    ""type"": ""string"",
                    ""description"": ""SharepointItem content class e.g.: STS_Site, STS_List_GenericList, STS_List, STS_ListItem_DocumentLibrary, STS_ListItem_WebPageLibrary, STS_ListItem_MySiteDocumentLibrary, STS_Web""
                },
                ""IsExternalContent"": {
                    ""type"": ""boolean"",
                    ""description"": ""Sharepoint is external content""
                },
                ""ListId"": {
                    ""type"": ""string"",
                    ""description"": ""SharepointItem list id""
                },
                ""OriginalPath"": {
                    ""type"": ""string"",
                    ""description"": ""Original path to file""
                },
                ""CollapsingStatus"": {
                    ""type"": ""integer"",
                    ""description"": """"
                },
                ""Id"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""IsDocument"": {
                    ""type"": ""boolean"",
                    ""description"": """"
                },
                ""SiteDescription"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""SiteTemplateId"": {
                    ""type"": ""integer"",
                    ""description"": """"
                },
                ""SiteTitle"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""Title"": {
                    ""type"": ""string"",
                    ""description"": ""Title of document file item""
                },
                ""WebTemplate"": {
                    ""type"": ""string"",
                    ""description"": ""SharepointeItem web template""
                },
                ""HitHighlightedProperties"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""CultureName"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""UrlZone"": {
                    ""type"": ""integer"",
                    ""description"": """"
                },
                ""GeoLocationSource"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""Author"": {
                    ""type"": ""string"",
                    ""description"": ""Author of document file item""
                }
              }
            }
        ";
    }
}
