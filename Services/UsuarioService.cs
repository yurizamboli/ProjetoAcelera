using ProjetoAcelera.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace ProjetoAcelera.Services
{
    public class UsuarioService
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

        public bool Cadastrar(string nome, string senha, string email)
        {
            //Mudei umas coisas yuri, Agora é MensagemBox e mudei para boolean, para poder usar no cadastro do usuario
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
                MessageBox.Show(string.Join("\n", erros));
                return false;
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
            
            return true;
            
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
            //deixei igual o do cadastro
            email = email.Trim().ToLower();
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

        public void Logout()
        {
            UsuarioLogado = null;
        }

        //exibir na tela todos
        public List<Usuario> ListarUsuarios() 
        {
            return usuarios.OrderBy(u => u.Nome).ToList();
        }

        //metodo para quando for passar pro arquivo app.xaml ((usado para salvar no json))
        public List<Usuario> ObterTodos()
        {
            return usuarios;
        }
       
        //busca pelo nome
        public List<Usuario> BuscarPorNome(string nome) 
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                return usuarios;
            }
            return usuarios.Where(u => u.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        public string GerarTokenRecup(string email)
        {
            var usuario = usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null) { return null; }

            string token = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();

            usuario.TokenRecuperacao = token;
            usuario.TokenExpiracao = DateTime.Now.AddMinutes(10);

            return token;
        }

        public bool RedefinirSenha(string email, string token, string novaSenha)
        {
            var usuario = usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null)
                return false;

            if (usuario.TokenRecuperacao != token)
                return false;

            if (usuario.TokenExpiracao < DateTime.Now)
                return false;

            usuario.SenhaHash = GerarHash(novaSenha);

            usuario.TokenRecuperacao = null;
            usuario.TokenExpiracao = null;

            return true;
        }
    }

}

