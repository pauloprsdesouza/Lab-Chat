using System;

namespace Lab.Chat.Models.Messages
{
    public class MessageResponse
    {
        /// <summary>
        /// Message unique identifier
        /// </summary>
        public string Id {get;set;}

        /// <summary>
        /// Message content
        /// </summary>
        public string Content {get;set;}

        /// <summary>
        /// Date/time when the message was sent
        /// </summary>
        public DateTimeOffset SentOn {get;set;}
    }
}
