using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using PactNet;
using PactNet.Matchers;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace PactTest.Lists.Tests
{
    public class Consumer : IDisposable
    {
        private readonly PactBuilder pactBuilder;
        private readonly IMockProviderService mockProviderService;
        private readonly HttpClient client;

        public Consumer()
        {
            this.pactBuilder = new PactBuilder(new PactConfig
            {
                SpecificationVersion = "2.0.0",
                LogDir = @"C:\temp\logs",
                PactDir = @"C:\temp\pacts"
            });

            this.pactBuilder
                    .ServiceConsumer("Consumer")
                    .HasPactWith("Values API");

            this.mockProviderService = this.pactBuilder.MockService(5000);
            this.mockProviderService.ClearInteractions();

            this.client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000"),
                DefaultRequestHeaders = { Accept = { MediaTypeWithQualityHeaderValue.Parse("application/json") } }
            };
        }

        [Fact]
        public async Task GetValues_WhenCalled_GetsAllValues()
        {
            this.mockProviderService
                .UponReceiving("A GET request to retrieve all values")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/api/values",
                    Headers = new Dictionary<string, object> { { "Accept", "application/json" } }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object> { { "Content-Type", "application/json; charset=utf-8" } },
                    Body = new { values = Match.MinType("example", 1) }
                });

            await this.client.GetAsync("/api/values");

            this.mockProviderService.VerifyInteractions();
        }

        public void Dispose()
        {
            this.pactBuilder.Build();
            this.client.Dispose();
        }
    }
}
