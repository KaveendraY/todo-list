using System;
using TodoApi.Models;

namespace TodoApi.Dtos
{
    public class TaskItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Completed { get; set; }
        public DateTime CreatedAt { get; set; }

        public TaskItemDto() { }

        public TaskItemDto(TaskItem t)
        {
            Id = t.Id;
            Title = t.Title;
            Description = t.Description;
            Completed = t.Completed;
            CreatedAt = t.CreatedAt;
        }
    }
}
