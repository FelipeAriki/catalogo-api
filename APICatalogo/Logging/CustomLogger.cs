
namespace APICatalogo.Logging
{
    public class CustomLogger(string name, CustomLoggerProviderConfiguration loggerConfig) : ILogger
    {
        private readonly string _loggerName = name;
        private readonly CustomLoggerProviderConfiguration _loggerConfig = loggerConfig;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string mensagem = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";
            EscreverTextoNoArquivo(mensagem);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == _loggerConfig.LogLevel;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }


        private static void EscreverTextoNoArquivo(string mensagem)
        {
            string caminhoArquivoLog = @"C:\Users\felip\source\repos\LogApiCatalago.txt";
            using (StreamWriter streamWriter = new(caminhoArquivoLog, true))
            {
                try
                {
                    streamWriter.WriteLine(mensagem);
                    streamWriter.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
