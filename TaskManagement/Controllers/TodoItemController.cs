using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;

        public TodoItemController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var todoItems = await _todoItemService.GetAllTodoItemsAsync();
            return Ok(todoItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var todoItem = await _todoItemService.GetTodoItemByIdAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            return Ok(todoItem);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TodoItem todoItem)
        {
            var result = await _todoItemService.AddTodoItemAsync(todoItem);

            if (HttpContext?.Items["User"] is User user)
            {
                todoItem.UserId = user.Id;
            }

            return CreatedAtAction(nameof(Get), new { id = result }, todoItem);
        }

        [HttpPut]
        public async Task<IActionResult> Put( [FromBody] TodoItem todoItem)
        {
            var currentItem = await _todoItemService.GetTodoItemByIdAsync(1);
            if (currentItem == null) { return NotFound(); }

            if (HttpContext?.Items["User"] is User user)
            {
                todoItem.UserId = user.Id;
            }

            await _todoItemService.UpdateTodoItemAsync(todoItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var currentItem = await _todoItemService.GetTodoItemByIdAsync(1);
            if (currentItem == null) { return NotFound(); }

            await _todoItemService.DeleteTodoItemAsync(id);
            return NoContent();
        }
    }
}
