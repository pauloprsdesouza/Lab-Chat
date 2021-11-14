using System;

namespace Lab.Chat.Models.Messages
{
    public class MessageResponse
    {
        /// <summary>
        /// Message's unique identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Message's content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Date/time when the message was sent.
        /// </summary>
        public DateTimeOffset SentOn { get; set; }

        /// <summary>
        /// Date/time when the message was updated.
        /// </summary>
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
