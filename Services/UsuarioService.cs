using ProjetoAcelera.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjetoAcelera.Services
{
    internal class UsuarioService
    {
        private List<Usuario> usuarios;
        private ArquivoService arquivoService;

        public Usuario UsuarioLogado { get; private set; }

        public UsuarioService() 
        {
            arquivoService = new ArquivoService();

            //carrega o json
            usuarios = arquivoService.Carregar();
        }

        private string GerarHash(string senha) 
        {
            using (var sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(senha));

                var convert = new StringBuilder();
                foreach (byte b in bytes) 
                {
                    convert.Append(b.ToString("x2"));
                }
                return convert.ToString();
            }
        }

        public void Cadastrar(string nome, string senha, string email) 
        {
            List<string> erros = new List<string>();
            nome = nome.Trim();
            email = email.Trim().ToLower();
            senha = senha.Trim();

            if (!Validacoes.ValidaNome(nome)) 
            {
                erros.Add("Nome inválido.");
            }
            if (!Validacoes.ValidaEmail(email))
            {
                erros.Add("Email inválido.");
            }
            if (!Validacoes.ValidaSenha(senha))
            {
                erros.Add("Senha não pode ser vazia.");
            }
            else
            {
                var errosSenha = SenhaForte(senha);

                if (errosSenha.Any())
                {
                    erros.Add("Senha fraca. Ela deve conter:");
                    foreach (var erro in errosSenha)
                    {
                        erros.Add(erro);
                    }
                }
            }
            if (EmailExiste(email)) 
            {
                erros.Add("Já existe um usúario com esse email.");
            }
            
            if (erros.Any()) 
            {
                Console.WriteLine("Não foi possível cadastrar:");
                foreach (var erro in erros) 
                {
                    Console.WriteLine("- " + erro);                
                }
                return;
            }

            Usuario novoUsuario = new Usuario
            {
                Nome = nome,
                SenhaHash = GerarHash(senha),
                Email = email,
                DataCadastro = DateTime.Now,
                Cargo = "Usuario", // nivel basico
                Obras = new List<Obra>(),
                Perfil = new Perfil 
                { 
                    Facebook = "",
                    Instagram = "",
                    Bio = "",            
                    FotoPerfil = ""
                }
            };
            usuarios.Add(novoUsuario);
            
        }

        private bool VerificarSenha(string senha, string hash)
        {
            return GerarHash(senha) == hash;
        }
        public bool EmailExiste(string email)
        {
            return usuarios.Any(u => u.Email == email);

        }
        public List<string> SenhaForte(string senha)
        {
            List<string> senhaFaltou = new List<string>();

            if (senha.Length < 8)
            {
                senhaFaltou.Add("pelo menos 8 caracteres.");
            }
            if (!senha.Any(char.IsUpper)) {
                senhaFaltou.Add("letra maiúscula.");
            }
            if (!senha.Any(char.IsLower))
            {
                senhaFaltou.Add("letra minúscula.");
            }
            if (!senha.Any(char.IsDigit))
            {
                senhaFaltou.Add("número.");
            }
            if (!senha.Any(c => !char.IsLetterOrDigit(c)))
            {
                senhaFaltou.Add("caractere especial.");
            }
            return senhaFaltou;
        }
        public static class Validacoes 
        {
            public static bool ValidaNome(string nome) 
            {
                return !string.IsNullOrWhiteSpace(nome);
            }
            public static bool ValidaSenha(string senha)
            {
                return !string.IsNullOrWhiteSpace(senha);
            }
            public static bool ValidaEmail(string email)
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return false; 
                }
                string padrao = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, padrao, RegexOptions.IgnoreCase);
            }           

        }
        
        public bool Login(string email, string senha)
        {
            email = email.ToLower();
            var usuario = usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null)
            {
                return false;
            }

            if (!VerificarSenha(senha, usuario.SenhaHash))
            {
                return false;
            }
            UsuarioLogado = usuario;
            return true;
        }

        //metodo para quando for passar pro arquivo app.xaml o carregamento e salvamento
        public List<Usuario> ObterTodos() {
            return usuarios;
        }




    }

}

