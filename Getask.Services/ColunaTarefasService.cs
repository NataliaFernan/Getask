using Getask.Repository;
using Getask.Repository.Interface;
using Getask.Repository.Models;
using Getask.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Getask.Services
{
    public class ColunaTarefasService : IColunaTarefasService
    {
        private readonly IColunaTarefasRepository _colunaTarefasRepository;
        private readonly ITarefaRepository _tarefaRepository;

        public ColunaTarefasService(IColunaTarefasRepository colunaTarefasRepository, ITarefaRepository tarefaRepository)
        {
            _colunaTarefasRepository = colunaTarefasRepository;
            _tarefaRepository = tarefaRepository;
        }

        public async Task<List<ColunaTarefas>> ObterColunasTarefasComTarefas()
        {
            return await _colunaTarefasRepository.ObterTodas();
        }

        public async Task<ColunaTarefas> CriarColunaTarefas(ColunaTarefas colunaTarefas)
        {
            return await _colunaTarefasRepository.Adicionar(colunaTarefas);
        }
        public async Task<ColunaTarefas> ObterPorId(int id)
        {
            return await _colunaTarefasRepository.ObterPorId(id);
        }
        public async Task AtualizarColunaTarefas(ColunaTarefas colunaTarefas)
        {
            await _colunaTarefasRepository.Atualizar(colunaTarefas);
        }

        public async Task CriarColunaAsync(string titulo)
        {
            await _colunaTarefasRepository.CriarColunaAsync(titulo);
        }

        public async Task MoverColunaAsync(int colunaId, int novaPosicao)
        {
            await _colunaTarefasRepository.MoverColunaAsync(colunaId,novaPosicao);
        }

        public async Task DeletarColunaAsync(int colunaId)
        {
            var colunas = await _colunaTarefasRepository.ObterTodas();
            var colunaParaDeletar = colunas.FirstOrDefault(c => c.Id == colunaId);

            if (colunaParaDeletar == null) return;

            var primeiraColuna = colunas.OrderBy(c => c.Posicao).FirstOrDefault();
            if (primeiraColuna != null && primeiraColuna.Id != colunaId)
            {
                foreach (var tarefa in colunaParaDeletar.Tarefas)
                {
                    tarefa.ColunaTarefaId = primeiraColuna.Id;
                }

                foreach (var tarefa in colunaParaDeletar.Tarefas.ToList())
                {
                    await _tarefaRepository.Atualizar(tarefa);
                }
            }

            await _colunaTarefasRepository.Remover(colunaParaDeletar.Id);

            var colunasRestantes = await _colunaTarefasRepository.ObterTodas();
            var colunasOrdenadas = colunasRestantes.OrderBy(c => c.Posicao).ToList();

            for (int i = 0; i < colunasOrdenadas.Count; i++)
            {
                colunasOrdenadas[i].Posicao = i;
            }

            foreach (var coluna in colunasOrdenadas)
            {
                await _colunaTarefasRepository.Atualizar(coluna);
            }
        }

    }
}
