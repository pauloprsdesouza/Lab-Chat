using System.Threading.Tasks;
using Lab.Tests.Fakes;
using Xunit;

namespace Lab.Tests.Functional.Messages
{
    public class ListMessagesTest
    {
        private readonly FakeApiServer _server;
        private readonly FakeApiClient _client;

        public ListMessagesTest(){
            _server = new FakeApiServer();
            _client = new FakeApiClient(_server);
        }

        [Fact]
        public async Task ShouldList(){

        }
    }
}
