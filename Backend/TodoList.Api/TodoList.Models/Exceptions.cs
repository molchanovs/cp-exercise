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
}