using System.Collections.Concurrent;

namespace APICatalogo.Logging
{
    public class CustomLoggerProvider(CustomLoggerProviderConfiguration loggerConfig) : ILoggerProvider
    {
        private readonly CustomLoggerProviderConfiguration _loggerConfig = loggerConfig;
        private readonly ConcurrentDictionary<string, CustomLogger> loggers = new ConcurrentDictionary<string, CustomLogger>();

        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, name => new CustomLogger(name, _loggerConfig));
        }

        public void Dispose()
        {
            loggers.Clear();
        }
    }
}
