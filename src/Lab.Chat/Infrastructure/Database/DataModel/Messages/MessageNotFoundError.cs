using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUlid;

namespace Lab.Chat.Infrastructure.Database.DataModel.Messages
{
    public class MessageNotFoundError : IActionResult
    {
        public MessageNotFoundError() { }

        public MessageNotFoundError(string messageId)
        {
            MessageId = messageId;
        }

        /// <summary>
        /// Message ID not found.
        /// </summary>
        /// <value></value>
        public string MessageId { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var jsonResult = new JsonResult(this);
            jsonResult.StatusCode = StatusCodes.Status404NotFound;

            await jsonResult.ExecuteResultAsync(context);
        }
    }
}
