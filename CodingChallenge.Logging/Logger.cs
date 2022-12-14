using Microsoft.Extensions.Logging;

namespace CodingChallenge.Logging
{
    public class Logger<T>  : CodingChallenge.Logging.Interface.ILogging<T> where T : class
    {

        private readonly ILogger<T> _logger;
        private readonly string _logMessageSplitString = "----======================================---------------";
        /// <summary>
        /// Inject the Logger Type Via Constructor, Comes in Handy when we are writing mock tests or inject a absctracted version
        /// </summary>
        /// <param name="logger"></param>
        public Logger(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message, object[]? args)
        {
            _logger.Log(LogLevel.Information, message, args);
        }
        public void LogWarning(string message, object[]? args)
        {
            _logger.Log(LogLevel.Warning, message, args);
        }
        public void LogError(string message, object[]? args)
        {
            _logger.Log(LogLevel.Error, message, args);
        }

        public void LogTrace(string message, object[]? args)
        {
            _logger.Log(LogLevel.Trace, message, args);
        }

        /// <summary>
        /// Private Method handling all the requets
        /// </summary>
        /// <param name="loglevel"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        private void Log(LogLevel loglevel, string message, object[]? args)
        {
            if (string.IsNullOrEmpty(message))
                _logger.Log(loglevel, _logMessageSplitString, null);
                _logger.Log(loglevel, message, args);
        }
    }
}