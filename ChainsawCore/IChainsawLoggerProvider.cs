
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ChainsawCore
{
    public interface IChainsawLoggerProvider : ILoggerProvider
    {
        void WriteMessage(LogMessage message);
        Task WriteMessageAsync(LogMessage logMessage);
    }
}