using System;

namespace OsbShowcase.Models
{
    public class Todo
    {
        public long? Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool Completed { get => CompletedAt != null; }

        public Todo CopyWith(string description, DateTime? createdAt, DateTime? completedAt)
        {
            return new Todo()
            {
                Id = Id,
                Description = description ?? Description,
                CreatedAt = createdAt ?? CreatedAt,
                CompletedAt = completedAt ?? CompletedAt
            };
        }

        public Todo TodoDto(TodoDTO data){

             var todo = new TodoDTO
             {
                Id = data.Id,
                Description = data.Description,
                CreatedAt = data.CreatedAt,
                CompletedAt = data.CompletedAt,
                Completed = data.Completed
             };
             
             return todo;
        }   
    }
}