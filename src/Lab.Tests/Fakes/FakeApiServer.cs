using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Lab.Tests.Fakes
{
    public sealed class FakeApiServer : TestServer
    {
        public FakeApiServer() : base(new Program().CreateWebHostBuilder()) { }

        public IWebHostEnvironment Environment => Host.Services.GetService<IWebHostEnvironment>();
        public IAmazonDynamoDB AmazonDynamoDB => Host.Services.GetService<IAmazonDynamoDB>();
    }
}
