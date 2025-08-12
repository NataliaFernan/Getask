using Getask.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace Getask.Repository
{
    public class GetaskDbContext : DbContext
    {
        public GetaskDbContext(DbContextOptions<GetaskDbContext> options) : base(options) { }

        public DbSet<ColunaTarefas> ColunasTarefas { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }

}