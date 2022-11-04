namespace CodingChallenge.Logging.Interface
{
    public interface ILogging<T>
    {
        void LogInfo(string message, object[]? args);

        void LogWarning(string message, object[]? args);

        void LogError(string message, object[]? args);

        void LogTrace(string message, object[]? args);

    }
}