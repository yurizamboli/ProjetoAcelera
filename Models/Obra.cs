using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// guarda as informações de uma obra cadastrada pelo usuário.
namespace ProjetoAcelera.Models
{
        public class Obra
        {
            public string Titulo { get; set; }
            public string Descricao { get; set; }
            public string Capa { get; set; }        
            public bool Favorito { get; set; }       
        }
    }

