using Getask.Repository.Interface;
using Getask.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getask.Repository
{
    public class ColunaTarefasRepository : IColunaTarefasRepository
    {
        private readonly GetaskDbContext _context;

        public ColunaTarefasRepository(GetaskDbContext context)
        {
            _context = context;
        }

        public async Task<ColunaTarefas> ObterPorId(int id)
        {
            return await _context.ColunasTarefas.FindAsync(id);
        }

        public async Task<List<ColunaTarefas>> ObterTodas()
        {
            return await _context.ColunasTarefas
                .Include(t => t.Tarefas)
                .OrderBy(c => c.Posicao)
                .ToListAsync();
        }

        public async Task<ColunaTarefas> Adicionar(ColunaTarefas colunaTarefas)
        {
            await _context.ColunasTarefas.AddAsync(colunaTarefas);
            await _context.SaveChangesAsync();

            return colunaTarefas;
        }

        public async Task Atualizar(ColunaTarefas colunaTarefas)
        {
            _context.ColunasTarefas.Update(colunaTarefas);
            await _context.SaveChangesAsync();
        }

        public async Task Remover(int id)
        {
            var colunaTarefas = await _context.ColunasTarefas.FindAsync(id);
            if (colunaTarefas != null)
            {
                _context.ColunasTarefas.Remove(colunaTarefas);
                await _context.SaveChangesAsync();
            }
        }
        public async Task CriarColunaAsync(string titulo)
        {
            var ultimaPosicao = await _context.ColunasTarefas.MaxAsync(c => (int?)c.Posicao) ?? 0;
            var novaColuna = new ColunaTarefas
            {
                Titulo = titulo,
                Posicao = ultimaPosicao + 1
            };
            await _context.ColunasTarefas.AddAsync(novaColuna);
            await _context.SaveChangesAsync();
        }

        public async Task MoverColunaAsync(int colunaId, int novaPosicao)
        {
            var colunas = await _context.ColunasTarefas.OrderBy(c => c.Posicao).ToListAsync();

            var colunaMovida = colunas.FirstOrDefault(c => c.Id == colunaId);
            if (colunaMovida == null)
            {
                throw new Exception("Coluna não encontrada.");
            }

            colunas.Remove(colunaMovida);

            colunas.Insert(Math.Clamp(novaPosicao, 0, colunas.Count), colunaMovida);
            for (int i = 0; i < colunas.Count; i++)
            {
                colunas[i].Posicao = i;
            }

            await _context.SaveChangesAsync();
        }
    }
}
