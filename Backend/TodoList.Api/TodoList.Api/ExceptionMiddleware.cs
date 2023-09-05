using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TodoList.Api.Controllers;
using TodoList.Models;

namespace TodoList.Api
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TodoItemsController> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<TodoItemsController> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = ex switch
                {
                    (NotFoundException) => (int)HttpStatusCode.NotFound,
                    (ApiException) => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                var message = httpContext.Response.StatusCode == (int)HttpStatusCode.InternalServerError ? "Oops, unexpected error. Please contact support." : ex.Message;
                var response = JsonSerializer.Serialize(new ErrorMessage
                {
                    StatusCode = httpContext.Response.StatusCode,
                    Message = message
                });
                await httpContext.Response.WriteAsync(response);
            }
        }
    }
}
