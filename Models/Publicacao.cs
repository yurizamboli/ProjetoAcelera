using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// guarda as informações de uma publicação feita por um usuário.
namespace ProjetoAcelera.Models
{
    public class Publicacao
    {
        public string NomeAutor { get; set; } = "";
        public string EmailAutor { get; set; } = "";
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Conteudo { get; set; } = "";
        public DateTime DataPublicacao { get; set; }
        public string ImagemUrl { get; set; } = "";
        public List<string> CurtidoPor { get; set; } = new List<string>();
        public string? CaminhoVideo { get; set; } // esse campo é para implementar a funcionalidade de vídeo, que pode ser um recurso futuro.
        public int Visualizacoes { get; set; } = 0; // esse campo é para implementar a funcionalidade de contagem de visualizações, que pode ser um recurso futuro.
        public bool ComentariosPermitidos { get; set; } = true; // permite ou não permite comentários na publicação.
        public string Status { get; set; } = "Aguardando aprovação"; // status da publicação, pode ser "Aguardando aprovação", "Aprovada" ou "Rejeitada".
        public DateTime? DataAprovacao { get; set; }
        public int Curtidas { get {return CurtidoPor.Count;} } // retorna a quantidade de curtidas, que é o número de pessoas que curtiram a publicação, ou seja, o tamanho da lista CurtidoPor.
        public List<Comentario> Comentarios { get; set; } = new List<Comentario>(); // lista de comentários feitos na publicação.
    }
}
