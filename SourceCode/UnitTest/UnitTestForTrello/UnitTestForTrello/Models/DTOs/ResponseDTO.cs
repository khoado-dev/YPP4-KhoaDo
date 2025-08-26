using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Tests
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; internal set; } = true;
        public object? Data { get; set; }
        public int StatusCode { get; internal set; }
    }
}