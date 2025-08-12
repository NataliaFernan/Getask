using Getask.Repository.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Getask.Services.Interface
{
    public interface IColunaTarefasService
    {
        Task<List<ColunaTarefas>> ObterColunasTarefasComTarefas();
        Task CriarColunaAsync(string titulo);
        Task AtualizarColunaTarefas(ColunaTarefas colunaTarefas);
        Task<ColunaTarefas> ObterPorId(int id);
        Task MoverColunaAsync(int colunaId, int novaPosicao);
        Task DeletarColunaAsync(int colunaId);
    }
}
