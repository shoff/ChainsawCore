namespace ChainsawCore
{
    /// <summary>
    /// Options for file logging.
    /// </summary>
    public class ChainsawLoggerOptions
    {
        public string UDPIPAddress { get; set; } = "127.0.0.1";
        public int UDPPort { get; set; } = 7071;
    }
}