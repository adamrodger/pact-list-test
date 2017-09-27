using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using Xunit;
using Xunit.Abstractions;

namespace PactTest.Lists.Tests
{
    public class Provider : IDisposable
    {
        private readonly IWebHost webHost;
        private readonly IPactVerifier pactVerifier;

        /// <summary>
        /// Initialises a new instance of the <see cref="Provider"/> class.
        /// </summary>
        public Provider(ITestOutputHelper output)
        {
            this.webHost = WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            this.webHost.Start();

            this.pactVerifier = new PactVerifier(new PactVerifierConfig
            {
                Verbose = true,
                Outputters = new IOutput[] { new ConsoleOutput(), new XunitOutput(output) }
            });
        }

        [Fact]
        public void ValuesApi_HonoursPactWith_Consumer()
        {
            this.pactVerifier
                .ServiceProvider("Values API", "http://localhost:5000")
                .HonoursPactWith("Consumer")
                .PactUri(@"C:\temp\pacts\consumer-values_api.json")
                .Verify();
        }

        public void Dispose()
        {
            this.webHost.StopAsync().GetAwaiter().GetResult();
        }
    }
}
