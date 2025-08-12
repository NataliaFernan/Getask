using System.ComponentModel.DataAnnotations.Schema;

namespace Getask.Repository.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public int ColunaTarefaId { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public int Posicao { get; set; }

        [ForeignKey("ColunaTarefaId")]
        public ColunaTarefas ColunaTarefa { get; set; }
    }
}