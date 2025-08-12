using Getask.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getask.Repository.Interface
{
    public interface IUsuarioRepository
    {
        Task<Usuario> ObterPorEmailSenha(string email, string senha);
        Task<Usuario> ObterUsuarioPorToken(string token);
        Task<Usuario> ObterUsuarioPorEmail(string email);
        Task AtualizarUsuario(Usuario usuario);
        Task AtualizarSenhaUsuario(Usuario usuario, string novaSenhaHash);
    }
}
