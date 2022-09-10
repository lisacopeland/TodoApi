using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TodoApi.Controllers;

[Authorize]
[Route("todos/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;

    public TodoController(ILogger<TodoController> logger)
    {
        _logger = logger;
    }

    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoItem))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoItem>> Get([FromQuery]string username, [FromQuery]string? id)
    {
        var result = await DBCall.Query<TodoItem>("todos", username, id);
        if (result != null)
        {
            return Ok(result);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromBody]TodoItem newTodoItem)
    {
        Console.WriteLine("hi from the post");

        TodoItem result = await DBCall.WriteAsync<TodoItem>("todos", newTodoItem);
        return Ok(result);
    }

   [HttpPut()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Put([FromBody]TodoItem changeTodoItem)
    {
        TodoItem result = await DBCall.WriteAsync<TodoItem>("todos", changeTodoItem);
        return Ok(result);
    }


    [HttpDelete()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromQuery]string username, [FromQuery]string id)
    {
        bool success = await DBCall.DeleteAsync("todos", username, id);
        if (success)
        {
            return Ok();
        }
        else
        {
            return BadRequest("Already exists");
        }
    }

}
