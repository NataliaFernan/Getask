using System.Threading.Tasks;

namespace Getask.Services.Interface
{
    public interface IEmailService
    {
        Task EnviarEmail(string toEmail, string subject, string message);
    }
}