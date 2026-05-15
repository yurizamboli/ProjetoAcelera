using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string? CaminhoVideo { get; set; }
        public int Visualizacoes { get; set; } = 0;
        public bool ComentariosPermitidos { get; set; } = true;
        public string Status { get; set; } = "Aguardando aprovação";

        public int Curtidas
        {
            get
            {
                return CurtidoPor.Count;
            }
        }

        public List<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}
