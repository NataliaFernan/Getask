using Getask.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Getask.Repository.Interface
{
    public interface IColunaTarefasRepository
    {
        Task<ColunaTarefas> ObterPorId(int id);
        Task<List<ColunaTarefas>> ObterTodas();
        Task<ColunaTarefas> Adicionar(ColunaTarefas colunaTarefas);
        Task Atualizar(ColunaTarefas colunaTarefas);
        Task Remover(int id);
        Task CriarColunaAsync(string titulo);
        Task MoverColunaAsync(int colunaId, int novaPosicao);
    }
}
