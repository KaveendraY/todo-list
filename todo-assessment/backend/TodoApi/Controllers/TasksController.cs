using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<TasksController> _logger;

        public TasksController(AppDbContext db, ILogger<TasksController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItemDto>>> Get([FromQuery] int limit = 5)
        {
            if (limit <= 0) limit = 5;

            var tasks = await _db.Tasks
                .AsNoTracking()
                .Where(t => !t.Completed)
                .OrderByDescending(t => t.CreatedAt)
                .Take(limit)
                .Select(t => new TaskItemDto(t))
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItemDto>> Create([FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var item = new TaskItem
            {
                Title = dto.Title.Trim(),
                Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim()
            };

            _db.Tasks.Add(item);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, new TaskItemDto(item));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskItemDto>> GetById(Guid id)
        {
            var t = await _db.Tasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (t == null) return NotFound();
            return new TaskItemDto(t);
        }

        [HttpPost("{id:guid}/complete")]
        public async Task<IActionResult> Complete(Guid id)
        {
            var t = await _db.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (t == null) return NotFound();
            if (t.Completed) return BadRequest(new { message = "Task already completed." });

            t.Completed = true;
            t.CompletedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
