using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Lab.Chat.Infrastructure.Database.DataModel.Messages;
using NUlid;

namespace Lab.Chat.Features.Messages
{
    public class MessageSearch
    {
        public readonly IDynamoDBContext _dbContext;

        public MessageSearch(IDynamoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool MessageNotFound { get; private set; }

        public async Task<Message> Find(string userId, Ulid messageId)
        {
            var messageKey = new MessageKey(userId, messageId);

            var message =  await _dbContext.LoadAsync<Message>(messageKey.PK, messageKey.SK);

            MessageNotFound = message == null;

            return message;
        }
    }
}
