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
using Lab.Chat.Features.Messages;

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
        /// Create a message
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

        /// <summary>
        /// Update a message
        /// </summary>
        /// <remarks>
        /// Update a message already sent to another user or group.
        /// </remarks>
        /// <param name="messageId" example="01FME0F949HAVJ91A9100N16ZS">Message's ID</param>
        [HttpPut, Route("{messageId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageNotFoundError), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update([FromRoute] Ulid messageId, [FromBody] PutMessageRequest putMessageRequest)
        {
            var messageSearch = new MessageSearch(_dbContext);
            var message = await messageSearch.Find(UserId, messageId);

            if (messageSearch.MessageNotFound)
            {
                return NotFound(new MessageNotFoundError(messageId.ToString()));
            }

            putMessageRequest.MapTo(message);

            var messageUpdate = new MessageUpdate(_dbContext);
            await messageUpdate.Update(message);

            return Ok(message.MapToResponse());
        }

        /// <summary>
        /// Find a message
        /// </summary>
        /// <remarks>
        /// Find a message already sent to another user or group.
        /// </remarks>
        /// <param name="messageId" example="01FME0F949HAVJ91A9100N16ZS">Message's ID</param>
        [HttpGet, Route("{messageId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageNotFoundError), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Find([FromRoute] Ulid messageId)
        {
            var messageSearch = new MessageSearch(_dbContext);
            var message = await messageSearch.Find(UserId, messageId);

            if (messageSearch.MessageNotFound)
            {
                return NotFound(new MessageNotFoundError(messageId.ToString()));
            }

            return Ok(message.MapToResponse());
        }

        /// <summary>
        /// Delete a message
        /// </summary>
        /// <remarks>
        /// Delete a message already sent to another user or group.
        /// </remarks>
        /// <param name="messageId" example="01FME0F949HAVJ91A9100N16ZS">Message's ID</param>
        [HttpDelete, Route("{messageId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageNotFoundError), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete([FromRoute] Ulid messageId)
        {
            var messageSearch = new MessageSearch(_dbContext);
            var message = await messageSearch.Find(UserId, messageId);

            if (messageSearch.MessageNotFound)
            {
                return NotFound(new MessageNotFoundError(messageId.ToString()));
            }

            var messageDelete = new MessageDelete(_dbContext);
            await messageDelete.Delete(message);

            return Ok(message.MapToResponse());
        }
    }
}
