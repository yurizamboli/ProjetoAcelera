using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAcelera.Models
{
    public class Evento
    {
        public string Titulo { get; set; } = string.Empty;

        //tem que ver a data ainda, se vai colocar ou puxar do pc//
        public string Data { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Detalhes { get; set; } = string.Empty;
        public string Imagem { get; set; } = string.Empty;
    }
}
