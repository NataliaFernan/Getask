using Getask.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getask.Repository.Interface
{
    public interface ITarefaRepository
    {
        Task<Tarefa> ObterPorId(int id);
        Task<List<Tarefa>> ObterTodas();
        Task AtualizarLista(List<Tarefa> tarefas);
        Task<List<Tarefa>> ObterTarefasPorColunaId(int colunaId);
        Task Adicionar(Tarefa tarefa);
        Task Atualizar(Tarefa tarefa);
        Task Remover(int id);
    }
}
