namespace ProjectFlourish.MockData
{
    internal class MessageSchema
    {
        public static readonly string Data = @"
            {
              ""ContentSources"": [ ""Exchange"", ""Teams"" ],
              ""Fields"": {
                ""ConversationTopic"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": true
                },
                ""Preview"": {
                    ""type"": ""string"",
                    ""description"": ""The first 255 characters of the message body. It is in text format."",
                    ""isQueryable"": false
                },
                ""DateTimeLastModified"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""DisplayTo"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": true
                },
                ""Subject"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": true
                },
                ""FlagStatus"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""GlobalMessageCount"": {
                    ""type"": ""integer"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""GlobalUnreadCount"": {
                    ""type"": ""integer"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""HasAttachments"": {
                    ""type"": ""boolean"",
                    ""description"": ""Indicates whether the message has attachments. This property doesn't include inline attachments, so if a message contains only inline attachments, this property is false."",
                    ""isQueryable"": true
                },
                ""HasIrm"": {
                    ""type"": ""boolean"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""Importance"": {
                    ""type"": ""string"",
                    ""description"": ""The importance of the message. The possible values are: low, normal, and high."",
                    ""isQueryable"": true
                },
                ""ItemClasses"": {
                    ""type"": ""array"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""LastDeliveryOrRenewTime"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""LastDeliveryTime"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""LastModifiedTime"": {
                    ""type"": ""string"",
                    ""description"": ""The time at which the message was last modified. This also includes the system modifications to the message."",
                    ""isQueryable"": false
                },
                ""MailboxGuids"": {
                    ""type"": ""array"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""MessageCount"": {
                    ""type"": ""integer"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""SenderSMTPAddress"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""SortKey"": {
                    ""type"": ""integer"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""UniqueRecipients"": {
                    ""type"": ""array"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""UniqueSenders"": {
                    ""type"": ""array"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""UnreadCount"": {
                    ""type"": ""integer"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""ImmutableId"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""IsMentioned"": {
                    ""type"": ""boolean"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""ParentFolderHexId"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""ParentFolderRestId"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""SortOrderSource"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""GlobalItemIds"": {
                    ""type"": ""array"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""ItemIds"": {
                    ""type"": ""array"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""ConversationHexId"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""ConversationRestId"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""ConversationIndex"": { 
                    ""type"": ""string"",
                    ""description"": ""This property is used internally for heuristics to determine the position of the message in the Store conversation. This also includes ConversationId as a part of its value."",
                    ""isQueryable"": false
                },
                ""ConversationThreadId"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""DateTimeCreated"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": true
                },
                ""DateTimeReceived"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""DateTimeSent"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": true
                },
                ""DisplayBcc"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""DisplayCc"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": true
                },
                ""ClientConversationId"": {
                    ""type"": ""string"",
                    ""description"": ""This value is provided by IC3 and shows the unique conversation from IC3 perspective. ClientConversationId is derived from ClientThreadId along with a few additional pieces.
Attributes: First-class property, retrievable, and queryable."",
                    ""isQueryable"": false
                },
                ""ClientThreadId"": {
                    ""type"": ""integer"",
                    ""description"": ""The value of this property is provided by IC3 as the parent containers of the messages."",
                    ""isQueryable"": false
                },
                ""IconIndex"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""InternetMessageId"": {
                    ""type"": ""integer"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""IsDraft"": {
                    ""type"": ""boolean"",
                    ""description"": ""Indicates whether the message is a draft. A message is draft if it has not yet been sent. If not a draft, the value is set to False; otherwise, True."",
                    ""isQueryable"": true
                },
                ""IsRead"": {
                    ""type"": ""boolean"",
                    ""description"": ""Indicates whether the message has been read. If not, the value is set to False; otherwise, True."",
                    ""isQueryable"": true
                },
                ""ItemHexId"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""ItemRestId"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""ItemClass"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""Sensitivity"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                },
                ""Size"": {
                    ""type"": ""integer"",
                    ""description"": """",
                    ""isQueryable"": true
                },
                ""WebLink"": {
                    ""type"": ""string"",
                    ""description"": ""URL to open the message in the Outlook web app."",
                    ""isQueryable"": false
                },
                ""InferenceClassification"": {
                    ""type"": ""string"",
                    ""description"": ""The message is classified as either Focused or Other for the user, depending on inferred relevance, importance, or an explicit override."",
                    ""isQueryable"": false
                },
                ""ParentFolderDisplayName"": {
                    ""type"": ""string"",
                    ""description"": """",
                    ""isQueryable"": false
                }
              }
            }
        ";
    }
}
