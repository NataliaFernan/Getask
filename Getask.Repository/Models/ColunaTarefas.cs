namespace Getask.Repository.Models
{
    public class ColunaTarefas
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int Posicao { get; set; }
        public List<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
    }
}