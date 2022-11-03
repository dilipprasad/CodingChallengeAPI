namespace CodingChallenge.Logging.Interface
{
    public interface ILogging
    {
        void LogInfo(string message, object[]? args);

        void LogWarning(string message, object[]? args);

        void LogError(string message, object[]? args);

    }
}