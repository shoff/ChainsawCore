using Microsoft.AspNetCore.Hosting;

namespace ChainsawCore
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public static class ChainsawLoggerFactoryExtensions
    {
        public static ILoggingBuilder AddChainsaw(this ILoggingBuilder builder)
        {
            //var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //var isDevelopment = environment == "Local";
            //if (isDevelopment)
            //{
                builder.Services.AddSingleton<ILoggerProvider, ChainsawLoggerProvider>();
           // }

            return builder;
        }

        public static ILoggingBuilder AddChainsaw(this ILoggingBuilder builder, Action<ChainsawLoggerOptions> configure)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == "Local";
            if (isDevelopment)
            {
                builder.AddChainsaw();
                builder.Services.Configure(configure);
            }
            return builder;
        }
    }
}