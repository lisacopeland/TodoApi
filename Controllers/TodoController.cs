using Microsoft.AspNetCore.Mvc;
using TodoApi;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;

    public TodoController(ILogger<TodoController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{username}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoItem[]))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoItem>> Get(string username)
    {
        Console.WriteLine($"Hi from get, the username is {username}");
        var result = await DBCall.Query<TodoItem>("todos", username);
        if (result != null)
        {
            return Ok(result);
        }
        else
        {
            return NotFound();
        }
    }


    [HttpGet("{username}, {id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoItem))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoItem>> Get(string username, string? id)
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

        bool success = await DBCall.WriteAsync<TodoItem>("todos", newTodoItem);
        if (success)
        {
            return Ok();
        }
        else
        {
            return BadRequest("Already exists");
        }
    }

   [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Put([FromBody]TodoItem changeTodoItem)
    {
        return Ok();
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(string Id)
    {
        return Ok();
    }    

}
