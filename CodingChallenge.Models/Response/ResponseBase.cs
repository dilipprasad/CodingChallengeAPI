namespace CodingChallenge.Models.Response
{
    public class ResponseBase
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<Error> ErrorDetails { get; set; }
        public bool IsException { get; set; }
        public string Source { get; set; }
        public string Version { get; set; }
    }
}
