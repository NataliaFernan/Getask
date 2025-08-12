using Getask.Repository.Models;
using System.Threading.Tasks;

namespace Getask.Services.Interface
{
    public interface ITarefaService
    {
        Task AdicionarTarefaAsync(Tarefa tarefa);
        Task MoverTarefaAsync(int tarefaId, int novaColunaId, int novaPosicao);
        Task DeletarTarefaAsync(int id);
        Task AtualizarTarefaAsync(Tarefa tarefa);
        Task<Tarefa> ObterTarefaPorId(int id);
    }
}