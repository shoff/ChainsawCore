namespace ChainsawCore
{
    using System;
    using Microsoft.Extensions.Logging;
    using System.Xml.Linq;

    public class LogMessage
    {
        public const string Logger = "Microsoft.AspNetCore.Hosting.Internal.WebHost";
        public LogLevel Level { get; set; }
        public int Thread { get; set; }
        public string Message { get; set; }
        public LocationInfo Location { get; set; }
        public string ApplicationName { get; set; }
        public string MachineName { get; set; }

        public class LocationInfo
        {
            public string Class { get; set; } = "com.howtodoinjava.Log4jXMLLayoutExample";
            public string Message { get; set; }
            public string File { get; set; }
            public int Line { get; set; }
        }

        // TODO how to get application name
        // TODO how to get machine name
        // TODO how to get calling class
        // TODO how to get calling method
        public string ToLog4JEvent()
        {
            var level = ConvertToLevel(this.Level);
            return CreateEvent(level, this.Message, this.ApplicationName, this.MachineName).ToString();
        }

        private string ConvertToLevel(LogLevel level)
        {
            if (level == LogLevel.Information)
            {
                return "INFO";
            }

            if (level == LogLevel.Critical)
            {
                return "FATAL";
            }

            if (level == LogLevel.Debug)
            {
                return "DEBUG";
            }

            if (level == LogLevel.Trace)
            {
                return "TRACE";
            }

            if (level == LogLevel.Error)
            {
                return "ERROR";
            }

            if (level == LogLevel.Warning)
            {
                return "WARN";
            }

            return "NONE";
        }

        private static readonly XNamespace ns = "log4j";
        private static XElement CreateMessage(string message)
        {
            return new XElement(ns + "message", message);
        }

        private static XElement CreateEvent(string level, string message, string applicationName, string machineName)
        {
            return new XElement(ns + "event",
                new XAttribute("logger", "Microsoft.AspNetCore.Hosting.Internal.WebHost"),
                new XAttribute("level", level),
                new XAttribute("timestamp", $"{(DateTime.UtcNow.Ticks - 621356166000000000) / 10000000}"),
                new XAttribute("thread", "28"),
                CreateMessage(message),
                CreateProperties(applicationName, machineName));
        }

        private static XElement CreateProperties(string applicationName, string machineName)
        {
            return new XElement(ns + "properties",
                new XElement(ns + "data", 
                    new XAttribute("name", "log4japp"),
                    new XAttribute("value", applicationName)),
                new XElement(ns + "data",
                    new XAttribute("name", "log4jmachinename"),
                    new XAttribute("value", machineName)));
        }

    }
}