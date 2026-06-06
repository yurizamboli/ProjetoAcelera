using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// guarda as informações de um evento que pode aparecer na programação(calendário) ou na página principal.
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
        // indica se o evento é destaque ou não, ou seja, se ele deve aparecer na página principal ou apenas na programação.
        public bool Destaque { get; set; } = false;
        public string Imagem { get; set; } = string.Empty;
    }
}

