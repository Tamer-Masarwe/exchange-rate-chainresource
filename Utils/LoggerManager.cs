using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace MizeProject.Utils
{
    public static class LoggerManager
    {
        private static readonly IServiceProvider _provider;

        static LoggerManager()
        {
            _provider = new ServiceCollection()
                .AddLogging(config =>
                {
                    config.SetMinimumLevel(LogLevel.Information);
                })
                .BuildServiceProvider();
        }

        public static ILogger<T> CreateLogger<T>()
        {
            return _provider.GetRequiredService<ILoggerFactory>().CreateLogger<T>();
        }
    }
}
