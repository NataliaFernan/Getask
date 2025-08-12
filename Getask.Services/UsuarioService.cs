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
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<Usuario> ObterUsuarioPorEmailSenha(string email, string senha)
        {
            return await _usuarioRepository.ObterPorEmailSenha(email, senha);
        }

        public async Task<Usuario> ObterUsuarioPorToken(string token)
        {
            return await _usuarioRepository.ObterUsuarioPorToken(token);
        }

        public async Task<Usuario> ObterUsuarioPorEmail(string email)
        {
            return await _usuarioRepository.ObterUsuarioPorEmail(email);
        }

        public async Task AtualizarUsuario(Usuario usuario)
        {
           await _usuarioRepository.AtualizarUsuario(usuario);
        }

        public async Task AtualizarSenhaUsuario(Usuario usuario, string novaSenhaHash)
        {
           await _usuarioRepository.AtualizarSenhaUsuario(usuario, novaSenhaHash);
        }

    }
}