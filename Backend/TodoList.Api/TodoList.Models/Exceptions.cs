namespace TodoList.Models
{
    public class ApiException : Exception
    {
        public ApiException(string Message = "Invalid request.") : base(Message) { }
    }
    public class NotFoundException : Exception
    {
        public NotFoundException(string Message = "Not found.") : base(Message) { }
    }

    public class ErrorMessage
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}