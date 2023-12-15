using TodoApi.Models;
using Microsoft.AspNetCore.Mvc;
namespace TodoApi.Controllers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

[ApiController]
// [Authorize]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;

    private readonly TodoContext _TodoContext;

    public TodoController(ILogger<TodoController> logger, TodoContext todoContext)
    {
        _logger = logger;
        this._TodoContext = todoContext;
    }

    [HttpGet]
    public async Task<ActionResult<TodoItem>> GetAllAsync()
    {
        var items = await _TodoContext.TodoItems.ToListAsync();
        if (items == null) return BadRequest();
        return Ok(items);
    }

    [HttpPost]
    public async Task<TodoItem> CreateAsync([FromBody] TodoItem item)
    {
        _TodoContext.Add(item);
        await _TodoContext.SaveChangesAsync();
        return item;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> ShowAsync(int id)
    {
        if (id <= 0) return BadRequest($"{id} provided invalid");
        var item = await _TodoContext.FindAsync<TodoItem>(id);
        if (item == null) return NotFound($"User {id} not found");

        var response = new ObjectResult(item)
        {
            StatusCode = 200,
            Value = new
            {
                Message = "Requisição feita com sucesso",
                Data = item
            },
        };
        return response;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TodoItem>> UpdateAsync(int id, [FromBody] TodoItem _item)
    {
        if (id <= 0 || _item == null)
        {
            return BadRequest();
        }

        var existingItem = await _TodoContext.FindAsync<TodoItem>(id);

        if (existingItem == null)
        {
            return NotFound();
        }

        if (id != existingItem.Id)
        {
            return BadRequest("Mismatched IDs");
        }

        // Update properties of existingItem with values from _item
        existingItem.Task = _item.Task;

        try
        {
            _TodoContext.Update(existingItem);
            await _TodoContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            // Handle concurrency conflict
            return Conflict("Concurrency conflict");
        }

        return Ok(existingItem);
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult<TodoItem>> DeleteAsync(int id)
    {
        if (id <= 0) return BadRequest();
        var item = await _TodoContext.FindAsync<TodoItem>(id);
        if (item == null) return NotFound();

        _TodoContext.Remove(item);
        await _TodoContext.SaveChangesAsync();
        return Ok("User deleted with success");
    }

}
