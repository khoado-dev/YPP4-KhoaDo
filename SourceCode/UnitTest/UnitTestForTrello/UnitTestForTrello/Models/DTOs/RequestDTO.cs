using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Tests
{
    public class RequestDTO
    {
        public RequestMethod Method { get; set; } = RequestMethod.GET;
        public required string Path { get; set; }
    }
}