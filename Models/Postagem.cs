using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAcelera.Models
{
    public class Postagem
    {
        public string Autor { get; set; }

        public string Texto { get; set; }

        public string Imagem { get; set; }

        public string Link { get; set; }

        public int Likes { get; set; }

        public int Comentarios { get; set; }

        public string FotoPerfil { get; set; }
    }
}
