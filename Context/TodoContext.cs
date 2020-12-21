using Microsoft.EntityFrameworkCore;
using OsbShowcase.Models;

namespace OsbShowcase.Context
{
    /// <summary>
    /// Contexto de Todo's.
    /// </summary>
    public class TodoContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DbSet<Todo> Todos { get; set; }

        /// <summary>
        /// Construtor que recebe objeto de opções de construção do contexto.
        /// </summary>
        /// <param name="options">Opções</param>
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Define configurações das entidades desse contexto no banco.
        /// </summary>
        /// <param name="builder">Construtor</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Todo>()
                .HasIndex(x => new { x.Description, x.CompletedAt })
                .IsUnique();
        }
    }
}