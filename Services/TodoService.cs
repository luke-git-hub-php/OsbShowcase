using Microsoft.EntityFrameworkCore;
using OsbShowcase.Context;
using OsbShowcase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OsbShowcase.Services
{
    public class TodoService
    {
        private readonly TodoContext _context;

        public TodoService(TodoContext context)
        {
            _context = context;
        }

        public TodoService()
        {
        }

        public async Task<Todo> CreateTodo(TodoDto data)
        {
            var todo = new Todo
            {
                Description = data.Description,
                CreatedAt = DateTime.Now,
            };

            await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();

            return todo;
        }

        public IList<Todo> GetTodos(long? todoId)
        {
            var todos = todoId == null
                ? _context.Todos.AsEnumerable()
                : _context.Todos.Where(x => x.Id == todoId).AsEnumerable();

            return todos.ToList();
        }

        public async Task<Todo> UpdateTodo(TodoDto data)
        {
            if (data.Id == null)
            {
                throw new ArgumentNullException("Campo 'Id' não pode ser nulo");
            }

            var todo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (todo == null)
            {
                throw new KeyNotFoundException($"Todo com id '{data.Id}' não encontrado.");
            }

            todo = todo.CopyWith(data.Description, data.CreatedAt, data.CompletedAt);
            await _context.SaveChangesAsync();

            return todo;
        }

        public async Task DeleteTodo(long? todoId)
        {
            if (todoId == null)
            {
                throw new Exception("Parâmetro 'todoId' não pode ser nulo.");
            }

            var todo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == todoId);
            if (todo == null)
            {
                throw new Exception($"Todo com id '{todoId}' não encontrado.");
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
        }
    }
}