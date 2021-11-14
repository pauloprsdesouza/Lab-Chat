using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Lab.Chat.Models.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Linq;
using Lab.Chat.Infrastructure.Database.DataModel.Messages;
using NUlid;
using Lab.Chat.Infrastructure.Serialization.Messages;
using Lab.Chat.Features;

namespace Lab.Chat.Controllers
{
    [Route("messages")]
    public class MessagesController : Controller
    {
        const string UserId = "1A2B3C4D";
        private readonly DynamoDBContext _dbContext;

        public MessagesController(IAmazonDynamoDB dynamoDB)
        {
            _dbContext = new DynamoDBContext(dynamoDB);
        }

        /// <summary>
        /// Get messages
        /// </summary>
        /// <remarks>
        /// Direct messages sent from one user to another user or group.
        /// </remarks>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(GetMessageResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult> List([FromQuery] GetMessagesQuery queryString)
        {
            var query = new MessageQuery();
            query.UserId = UserId;
            query.BeforeMessage = queryString.Before ?? Ulid.NewUlid();
            query.Length = queryString.Length ?? 30;

            var messages = await _dbContext
                .FromQueryAsync<Message>(query.ToDynamoDBQuery())
                .GetRemainingAsync();

            return Ok(new GetMessageResponse
            {
                Messages = messages.Select(message => message.MapToResponse())
            });
        }

        /// <summary>
        /// Create messages
        /// </summary>
        /// <remarks>
        /// Send direct messages to another user or group.
        /// </remarks>
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult> Create([FromBody] PostMessageRequest postMessageRequest)
        {
            var messageSending = new MessageSending(_dbContext);
            var message = postMessageRequest.ToMessage();

            await messageSending.Send(UserId, message);

            return Ok(message.MapToResponse());
        }
    }
}
