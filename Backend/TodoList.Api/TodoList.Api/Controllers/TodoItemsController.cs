using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Database;
using TodoList.Models;
using TodoList.Services;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemsService _todoItemsService;

        public TodoItemsController(ITodoItemsService todoItemsService)
        {
            _todoItemsService = todoItemsService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TodoItem>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTodoItems()
        {
            var results = await _todoItemsService.GetAll();
            return Ok(results);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(TodoItem), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTodoItem(Guid id)
        {
            var result = await _todoItemsService.Get(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItem todoItem)
        {
            await _todoItemsService.Update(id, todoItem);
            return NoContent();
        } 

        [HttpPost]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostTodoItem(TodoItem todoItem)
        {
            await _todoItemsService.Add(todoItem);             
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }
    }
}
