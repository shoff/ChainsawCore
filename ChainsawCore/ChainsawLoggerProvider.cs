using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace ChainsawCore
{
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// An <see cref="ILoggerProvider" /> that writes logs to a file
    /// </summary>
    [ProviderAlias("Chainsaw")]
    public class ChainsawLoggerProvider : IChainsawLoggerProvider
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly UdpClient udpClient;

        /// <summary>
        /// Creates an instance of the <see cref="ChainsawLoggerProvider" /> 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="options">The options object controlling the logger</param>
        public ChainsawLoggerProvider(
            IHostingEnvironment hostingEnvironment,
            IOptionsMonitor<ChainsawLoggerOptions> options)
        {
            this.hostingEnvironment = hostingEnvironment;
            IPAddress address = IPAddress.Parse(options.CurrentValue.UDPIPAddress);
            var port = options.CurrentValue.UDPPort;
            this.udpClient = new UdpClient(18288);
            this.udpClient.Connect(address, port);
        }

        public void WriteMessage(LogMessage message)
        {
            var xml = message.ToLog4JEvent();
            this.udpClient.Send(Encoding.ASCII.GetBytes(xml), Encoding.ASCII.GetByteCount(xml));
        }

        public async Task WriteMessageAsync(LogMessage logMessage)
        {
            var xml = logMessage.ToLog4JEvent();
            await this.udpClient.SendAsync(Encoding.ASCII.GetBytes(xml), Encoding.ASCII.GetByteCount(xml));
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new ChainsawLogger(this, this.hostingEnvironment, categoryName);
        }

        public void Dispose()
        {
            udpClient?.Dispose();
        }
    }
}