using Getask.Repository.Models;
using Getask.Services.Interface;
using Getask.Repository;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using Getask.Repository.Interface;
using System.Collections.Generic;

namespace Getask.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IColunaTarefasRepository _colunaRepository;

        public TarefaService(ITarefaRepository tarefaRepository, IColunaTarefasRepository colunaRepository)
        {
            _tarefaRepository = tarefaRepository;
            _colunaRepository = colunaRepository;
        }

        public async Task AdicionarTarefaAsync(Tarefa tarefa)
        {
            tarefa.DataAtualizacao = DateTime.UtcNow;
            await _tarefaRepository.Adicionar(tarefa);
        }

        public async Task AtualizarTarefaAsync(Tarefa tarefa)
        {
            await _tarefaRepository.Atualizar(tarefa);
        }

        public async Task<Tarefa> ObterTarefaPorId(int id)
        {
            return await _tarefaRepository.ObterPorId(id);
        }

        public async Task MoverTarefaAsync(int tarefaId, int novaColunaId, int novaPosicao)
        {
            var tarefa = await _tarefaRepository.ObterPorId(tarefaId);
            if (tarefa == null)
            {
                throw new Exception("Tarefa não encontrada.");
            }

            var colunaOrigemId = tarefa.ColunaTarefaId;

            if (colunaOrigemId == novaColunaId)
            {
                var tarefas = (await _tarefaRepository.ObterTarefasPorColunaId(novaColunaId))
                                    .Where(t => t.Id != tarefaId)
                                    .ToList();

                if (novaPosicao < 0) novaPosicao = 0;
                if (novaPosicao > tarefas.Count) novaPosicao = tarefas.Count;

                tarefas.Insert(novaPosicao, tarefa);

                for (int i = 0; i < tarefas.Count; i++)
                {
                    tarefas[i].Posicao = i;
                }

                await _tarefaRepository.AtualizarLista(tarefas);
            }
            else
            {
                var tarefasOrigem = (await _tarefaRepository.ObterTarefasPorColunaId(colunaOrigemId))
                                        .Where(t => t.Id != tarefaId)
                                        .ToList();

                for (int i = 0; i < tarefasOrigem.Count; i++)
                {
                    tarefasOrigem[i].Posicao = i;
                }

                var tarefasDestino = (await _tarefaRepository.ObterTarefasPorColunaId(novaColunaId))
                                        .ToList();

                if (novaPosicao < 0) novaPosicao = 0;
                if (novaPosicao > tarefasDestino.Count) novaPosicao = tarefasDestino.Count;

                tarefa.ColunaTarefaId = novaColunaId;
                tarefasDestino.Insert(novaPosicao, tarefa);

                for (int i = 0; i < tarefasDestino.Count; i++)
                {
                    tarefasDestino[i].Posicao = i;
                }

                var todasTarefasParaAtualizar = new List<Tarefa>();
                todasTarefasParaAtualizar.AddRange(tarefasOrigem);
                todasTarefasParaAtualizar.AddRange(tarefasDestino);

                await _tarefaRepository.AtualizarLista(todasTarefasParaAtualizar);
            }
        }

        public async Task DeletarTarefaAsync(int id)
        {
            var tarefa = await _tarefaRepository.ObterPorId(id);
            if (tarefa == null)
            {
                throw new Exception("Tarefa não encontrada.");
            }

            var colunaId = tarefa.ColunaTarefaId;

            await _tarefaRepository.Remover(id);

            var tarefasRestantes = await _tarefaRepository.ObterTarefasPorColunaId(colunaId);
            for (int i = 0; i < tarefasRestantes.Count; i++)
            {
                tarefasRestantes[i].Posicao = i;
            }
            await _tarefaRepository.AtualizarLista(tarefasRestantes);      
        }
    }
}