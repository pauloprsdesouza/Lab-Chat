using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Lab.Chat.Infrastructure.Database.DataModel.Messages;

namespace Lab.Chat.Features.Messages
{
    public class MessageDelete
    {
        public readonly DynamoDBContext _dbContext;

        public MessageDelete(DynamoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Delete(Message message)
        {
            await _dbContext.DeleteAsync(message);
        }
    }
}
