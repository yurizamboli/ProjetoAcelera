using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAcelera.Models
{
    public class Usuario
    {
        //Compilador tava enchendo o saco pra colocar = "";
        public string Nome { get; set; } = "";
        public string SenhaHash { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime DataCadastro { get; set; }
        public string Cargo { get; set; } = "";
        public List<Obra> Obras { get; set; } = new List<Obra>();
        public Perfil Perfil { get; set; } = new Perfil();
        public string TokenRecuperacao { get; set; } = "";
        public DateTime? TokenExpiracao { get; set; }
        public bool Banido { get; set; }
        
        public List<Publicacao> Publicacoes { get; set; } = new List<Publicacao>();


        //pro adm pegar o nome e o email do usuario
        public string NomeCompleto
        {
            get
            {
                return Nome + " - " + Email;
            }
        }
        public bool AdminPrincipal { get; set; } = false;
    }
}
