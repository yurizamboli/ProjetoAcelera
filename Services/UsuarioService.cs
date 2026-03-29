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
            if (EmailExiste(email)) 
            {
                erros.Add("Já existe um usúario com email.");
            }
            if (!SenhaForte(senha)) 
            {
                erros.Add("Senha fraca.Deve ter pelo menos 8 caracteres, incluir maiúscula, minúscula, número e caractere especial.");
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
            string senhaHash = GerarHash(senha);
            Usuario novoUsuario = new Usuario
            {
                Nome = nome,
                SenhaHash = senhaHash,
                Email = email,
                DataCadastro = DateTime.Now
            };
            usuarios.Add(novoUsuario);
            arquivoService.Salvar(usuarios);
        }

        private bool VerificarSenha(string senha, string hash)
        {
            return GerarHash(senha) == hash;
        }
        public bool EmailExiste(string email)
        {
            return usuarios.Any(u => u.Email == email);

        }
        public bool SenhaForte(string senha)
        {
            if (senha.Length < 8)
            {
                return false;
            }
            bool Maiuscula = senha.Any(char.IsUpper);
            bool Minuscula = senha.Any(char.IsLower);
            bool Numero = senha.Any(char.IsDigit);
            bool CEspecial = senha.Any(c => !char.IsLetterOrDigit(c));

            return Maiuscula && Minuscula && Numero && CEspecial;
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

    }
}
