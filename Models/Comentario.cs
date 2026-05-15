using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;


namespace ProjetoAcelera.Models
{
    public class Comentario
    {
        public Guid Id { get; set; } = Guid.NewGuid();  

        public string NomeAutor { get; set; } = "";

        public string EmailAutor { get; set; } = "";

        public string Conteudo { get; set; } = "";

        public DateTime DataComentario { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Aguardando aprovação";
    }
}
