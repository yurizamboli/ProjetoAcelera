using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// guarda as informações principais do usuário.
namespace ProjetoAcelera.Models
{
    public class Usuario
    {
        public string Nome { get; set; } = "";
        public string SenhaHash { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime DataCadastro { get; set; }
        public string Cargo { get; set; } = ""; // define o cargo do usuário (ex: admin ou usuário comun)
        public List<Obra> Obras { get; set; } = new List<Obra>(); // lista de obras cadastradas pelo usuário
        public Perfil Perfil { get; set; } = new Perfil(); //informações do perfil do usuário
        public string TokenRecuperacao { get; set; } = ""; // token para recuperação de senha
        public DateTime? TokenExpiracao { get; set; } // data de expiração do token de recuperação de senha
        public bool Banido { get; set; } = false; // indica se o usuário está banido ou não
        public List<Publicacao> Publicacoes { get; set; } = new List<Publicacao>(); // lista de publicações feitas pelo usuário
        public string NomeCompleto { get { return Nome + " - " + Email; } } //pro adm pegar o nome e o email do usuario
        public bool AdminPrincipal { get; set; } = false;
    }
}
