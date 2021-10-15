using System.Collections.Generic;

namespace Lab.Chat.Models.Messages
{
    public class ListMessageResponse
    {
        /// <summary>
        /// Messa unique identifier
        /// </summary>
        public ICollection<MessageResponse> Messages {get;set;}
    }
}
