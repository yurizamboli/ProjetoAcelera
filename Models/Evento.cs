using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAcelera.Models
{
    public class Evento
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Programacao { get; set; } = string.Empty;
        
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string Local { get; set; } = string.Empty;
        public bool Destaque { get; set; } = false;
        public string Imagem { get; set; } = string.Empty;
    }
}

