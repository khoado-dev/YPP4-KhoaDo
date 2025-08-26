using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Tests
{
    public class RequestDTO
    {
        public RequestMethod Method { get; set; } = RequestMethod.GET;
        public required string Path { get; set; }
        public Dictionary<string, string> Headers { get; internal set; }
        public Dictionary<string, string> Query { get; internal set; }
        public string Body { get; internal set; }
    }
}