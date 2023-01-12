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
                    ""description"": """"
                },
                ""FlagStatus"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""GlobalMessageCount"": {
                    ""type"": ""integer"",
                    ""description"": """"
                },
                ""GlobalUnreadCount"": {
                    ""type"": ""integer"",
                    ""description"": """"
                },
                ""HasAttachments"": {
                    ""type"": ""boolean"",
                    ""description"": ""Indicates whether the message has attachments. This property doesn't include inline attachments, so if a message contains only inline attachments, this property is false.""
                },
                ""HasIrm"": {
                    ""type"": ""boolean"",
                    ""description"": """"
                },
                ""Importance"": {
                    ""type"": ""string"",
                    ""description"": ""The importance of the message. The possible values are: low, normal, and high.""
                },
                ""ItemClasses"": {
                    ""type"": ""array"",
                    ""description"": """"
                },
                ""LastDeliveryOrRenewTime"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""LastDeliveryTime"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""LastModifiedTime"": {
                    ""type"": ""string"",
                    ""description"": ""The time at which the message was last modified. This also includes the system modifications to the message.""
                },
                ""MailboxGuids"": {
                    ""type"": ""array"",
                    ""description"": """"
                },
                ""MessageCount"": {
                    ""type"": ""integer"",
                    ""description"": """"
                },
                ""Preview"": {
                    ""type"": ""string"",
                    ""description"": ""The first 255 characters of the message body. It is in text format.""
                },
                ""SenderSMTPAddress"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""SortKey"": {
                    ""type"": ""integer"",
                    ""description"": """"
                },
                ""UniqueRecipients"": {
                    ""type"": ""array"",
                    ""description"": """"
                },
                ""UniqueSenders"": {
                    ""type"": ""array"",
                    ""description"": """"
                },
                ""UnreadCount"": {
                    ""type"": ""integer"",
                    ""description"": """"
                },
                ""ImmutableId"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""IsMentioned"": {
                    ""type"": ""boolean"",
                    ""description"": """"
                },
                ""ParentFolderHexId"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""ParentFolderRestId"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""SortOrderSource"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""GlobalItemIds"": {
                    ""type"": ""array"",
                    ""description"": """"
                },
                ""ItemIds"": {
                    ""type"": ""array"",
                    ""description"": """"
                },
                ""ConversationHexId"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""ConversationRestId"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""ConversationIndex"": { 
                    ""type"": ""string"",
                    ""description"": ""This property is used internally for heuristics to determine the position of the message in the Store conversation. This also includes ConversationId as a part of its value.""
                },
                ""ConversationThreadId"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""DateTimeCreated"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""DateTimeLastModified"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""DateTimeReceived"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""DateTimeSent"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""DisplayBcc"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""DisplayCc"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""DisplayTo"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""ClientConversationId"": {
                    ""type"": ""string"",
                    ""description"": ""This value is provided by IC3 and shows the unique conversation from IC3 perspective. ClientConversationId is derived from ClientThreadId along with a few additional pieces.
Attributes: First-class property, retrievable, and queryable.""
                },
                ""ClientThreadId"": {
                    ""type"": ""integer"",
                    ""description"": ""The value of this property is provided by IC3 as the parent containers of the messages.""
                },
                ""IconIndex"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""InternetMessageId"": {
                    ""type"": ""integer"",
                    ""description"": """"
                },
                ""IsDraft"": {
                    ""type"": ""boolean"",
                    ""description"": ""Indicates whether the message is a draft. A message is draft if it has not yet been sent. If not a draft, the value is set to False; otherwise, True.""
                },
                ""IsRead"": {
                    ""type"": ""boolean"",
                    ""description"": ""Indicates whether the message has been read. If not, the value is set to False; otherwise, True.""
                },
                ""ItemHexId"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""ItemRestId"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""ItemClass"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""Sensitivity"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""Size"": {
                    ""type"": ""integer"",
                    ""description"": """"
                },
                ""Subject"": {
                    ""type"": ""string"",
                    ""description"": """"
                },
                ""WebLink"": {
                    ""type"": ""string"",
                    ""description"": ""URL to open the message in the Outlook web app.""
                },
                ""InferenceClassification"": {
                    ""type"": ""string"",
                    ""description"": ""The message is classified as either Focused or Other for the user, depending on inferred relevance, importance, or an explicit override.""
                },
                ""ParentFolderDisplayName"": {
                    ""type"": ""string"",
                    ""description"": """"
                }
              }
            }
        ";
    }
}
