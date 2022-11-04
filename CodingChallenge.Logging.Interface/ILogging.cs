namespace CodingChallenge.Logging.Interface
{
    public interface ILogging<T> where T: class
    {
        void LogInfo(string message, object[]? args);

        void LogWarning(string message, object[]? args);

        void LogError(string message, object[]? args);

        void LogTrace(string message, object[]? args);

    }
}