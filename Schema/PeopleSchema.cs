namespace ProjectFlourish.MockData
{
    internal class PeopleSchema
    {
        public static readonly string Data = @"
            {
              ""ContentSources"": [ ""Exchange"" ],
              ""Fields"": {
                ""Id"": {
                    ""type"": ""string"",
                    ""description"": ""Id of the contact. For organizational users it is uid@tid (User.ExternalObjectDirectoryId@Tenant.ExternalDirectoryObjectId) For external contact it is personId. It is not recommended to use or parse this id in your product""
                },
                ""DisplayName"": { 
                    ""type"": ""string"",
                    ""description"": ""This attribute specifies the display name for an object. This attribute is usually the combination of the user's first name, middle initial, and last name.""
                },
                ""GivenName"": {
                    ""type"": ""string"",
                    ""description"": ""This attribute contains the given name (first name) of the user""
                },
                ""Surname"": {
                    ""type"": ""string"",
                    ""description"": ""Surname of the user""
                },
                ""EmailAddresses"": {
                    ""type"": ""array"",
                    ""description"": ""Email addresses of the user. Use the first in the list to send email.	""
                },
                ""ProxyEmailAddresses"": {
                    ""type"": ""array"",
                    ""description"": """"
                },
                ""CompanyName"": {
                    ""type"": ""string"",
                    ""description"": ""Company name of the user""
                },
                ""Department"": {
                    ""type"": ""string"",
                    ""description"": ""Department of the user""
                },
                ""OfficeLocation"": {
                    ""type"": ""string"",
                    ""description"": ""Office location name of the user""
                },
                ""AdditionalOfficeLocation"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""Phones"": {
                    ""type"": ""array"",
                    ""description"": ""List of phone numbers. Possible type of phone numbers: Business, Home, Mobile""
                },
                ""JobTitle"": {
                    ""type"": ""string"",
                    ""description"": ""Job title of the user""
                },
                ""ImAddress"": {
                    ""type"": ""string"",
                    ""description"": ""Instant message address of consumer user. SIP address for business users.""
                },
                ""PeopleType"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""PeopleSubtype"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""ADObjectId"": {
                    ""type"": ""integer"",
                    ""description"": ""Exchange Online Active Directory ID. It is local to Exchange. Apart from a very rare and special cases, this Id shouldn't be used. Use AADObjectId (ExternalDirectoryObjectId) which is global and immutable.""
                },
                ""UserPrincipalName"": {
                    ""type"": ""string"",
                    ""description"": ""Universal Principal name. This is the network domain login credential in an email format. Most often backed by a real email address. Only business users have it. It can change.""
                },
                ""ExternalDirectoryObjectId"": {
                    ""type"": ""string"",
                    ""description"": ""This is the ID of the contact in Azure Active Directory of an organization object. Also known as AADObjectId (AzureADObjecId). In almost all cases, this is the right ID to identify the user. Only exist for objects in AAD.""
                },
                ""Text"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""QueryText"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""PropertyHits"": {
                    ""type"": ""array"",
                    ""description"": """"
                }
              }
            }
        ";
    }
}
