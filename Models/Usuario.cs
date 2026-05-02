using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAcelera.Models
{
    public class Usuario
    {
        public string Nome { get; set; }
        public string SenhaHash { get; set; }
        public string Email { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Cargo { get; set; }
        public List<Obra> Obras { get; set; }
        public Perfil Perfil { get; set; }
        public string TokenRecuperacao { get; set; }
        public DateTime? TokenExpiracao { get; set; }

    }
}
