using Getask.Repository.Interface;
using Getask.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace Getask.Repository
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly GetaskDbContext _context;

        public TarefaRepository(GetaskDbContext context)
        {
            _context = context;
        }

        public async Task<Tarefa> ObterPorId(int id)
        {
            return await _context.Tarefas.FindAsync(id);
        }

        public async Task<List<Tarefa>> ObterTodas()
        {
            return await _context.Tarefas.ToListAsync();
        }

        public async Task Adicionar(Tarefa tarefa)
        {
            await _context.Tarefas.AddAsync(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(Tarefa tarefa)
        {
            _context.Tarefas.Update(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task Remover(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa != null)
            {
                _context.Tarefas.Remove(tarefa);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Tarefa>> ObterTarefasPorColunaId(int colunaId)
        {
            return await _context.Tarefas
                                 .Where(t => t.ColunaTarefaId == colunaId)
                                 .OrderBy(t => t.Posicao)
                                 .ToListAsync();
        }

        public async Task AtualizarLista(List<Tarefa> tarefas)
        {
            _context.Tarefas.UpdateRange(tarefas);
            await _context.SaveChangesAsync();
        }
    }
}