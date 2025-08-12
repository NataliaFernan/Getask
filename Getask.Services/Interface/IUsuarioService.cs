using Getask.Repository.Models;
using System.Threading.Tasks;

namespace Getask.Services.Interface
{
    public interface IUsuarioService
    {
        Task<Usuario> ObterUsuarioPorEmailSenha(string email, string senha);
        Task<Usuario> ObterUsuarioPorEmail(string email);
        Task<Usuario> ObterUsuarioPorToken(string token);
        Task AtualizarUsuario(Usuario usuario);
        Task AtualizarSenhaUsuario(Usuario usuario, string novaSenhaHash);
    }
}