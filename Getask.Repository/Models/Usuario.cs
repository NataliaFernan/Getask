namespace Getask.Repository.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Token { get; set; }
        public DateTime? DataExpiracaoToken { get; set; }
    }
}