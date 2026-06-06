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

