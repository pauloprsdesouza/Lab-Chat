using System.Collections.Generic;

namespace Lab.Chat.Models.Messages
{
    public class GetMessageResponse
    {
        /// <summary>
        /// Messa unique identifier
        /// </summary>
        public IEnumerable<MessageResponse> Messages {get;set;}
    }
}
