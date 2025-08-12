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
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly GetaskDbContext _context;

        public UsuarioRepository(GetaskDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> ObterPorEmailSenha(string email, string senha)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);
        }

        public async Task<Usuario> ObterUsuarioPorToken(string token)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Token == token && u.DataExpiracaoToken > DateTime.Now);
        }

        public async Task<Usuario> ObterUsuarioPorEmail(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AtualizarUsuario(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarSenhaUsuario(Usuario usuario, string novaSenhaHash)
        {
            usuario.Senha = novaSenhaHash;
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

    }
}
