using PactNet.Infrastructure.Outputters;
using Xunit.Abstractions;

namespace PactTest.Lists.Tests
{
    public class XunitOutput : IOutput
    {
        private readonly ITestOutputHelper output;

        public XunitOutput(ITestOutputHelper output) => this.output = output;

        public void WriteLine(string line)
        {
            this.output.WriteLine(line);
        }
    }
}
