using System;

namespace ProjetoAcelera.Models
{
    public class Postagem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string AutorEmail { get; set; } = string.Empty;
        public string AutorNome { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public string? CaminhoImagem { get; set; }
        public string? CaminhoVideo { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
