using ProjetoAcelera.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

// cuida de toda a lógica relacionada aos usuários
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

        // gera o hash da senha usando SHA256, garantindo que as senhas sejam armazenadas de forma segura.
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
        // cadastra um novo usuário, realizando validações.
        public bool Cadastrar(string nome, string senha, string email)
        {
            List<string> erros = new List<string>();
            nome = nome.Trim();
            email = email.Trim().ToLower();
            senha = senha.Trim();

            // validações para garantir que os dados do usuário sejam válidos e seguros.
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
            // se houver erros, exibe uma mensagem concatenada e retorna false para indicar que o cadastro falhou.
            if (erros.Any()) 
            {
                MessageBox.Show(string.Join("\n", erros));
                return false;
            }
            //Agora todos os primeiros usuarios são adm
            bool primeiroUsuario = usuarios.Count == 0;
            string cargo = "Usuario";
            if (primeiroUsuario)
            {
                cargo = "Admin";
            }
            // cria um novo objeto Usuario com os dados fornecidos
            Usuario novoUsuario = new Usuario
            {
                Nome = nome,
                SenhaHash = GerarHash(senha),
                Email = email,
                DataCadastro = DateTime.Now,
                Cargo = cargo, // nivel basico
                AdminPrincipal = primeiroUsuario, // o primeiro usuario é o admin principal
                Obras = new List<Obra>(),
                Perfil = new Perfil 
                { 
                    Facebook = "",
                    Instagram = "",
                    Bio = "",            
                    FotoPerfil = ""
                } // perfil inicial vazio
            };
            usuarios.Add(novoUsuario);            
            return true;            
        }

        // método para verificar se a senha fornecida corresponde ao hash armazenado
        private bool VerificarSenha(string senha, string hash)
        {
            return GerarHash(senha) == hash;
        }
        
        // método para verificar se já existe um usuário com o email fornecido
        public bool EmailExiste(string email)
        {
            return usuarios.Any(u => u.Email == email);
        }
        
        // método para verificar se a senha atende aos critérios de segurança
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
            // retorna a lista de critérios que a senha não atende
            return senhaFaltou;
        }
        
        // classe estática para validações de campos
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
                // expressão regular para validar o formato do email
                string padrao = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, padrao, RegexOptions.IgnoreCase);
            }           
        }
        
        // método para realizar o login do usuário, verificando as credenciais
        public bool Login(string email, string senha)
        {
            //deixei igual o do cadastro
            email = email.Trim().ToLower();
            var usuario = usuarios.FirstOrDefault(u => u.Email == email);
            if (usuario == null)
            {
                return false;
            }
            //verifica se o usuário está banido
            if (usuario.Banido)
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

        //reliza o logout do usuário
        public void Logout()
        {
            UsuarioLogado = null;
        }

        //metodo para quando for passar pro arquivo app.xaml ((usado para salvar no json))
        public List<Usuario> ObterTodos()
        {
            return usuarios;
        }

        //gera um token de recuperação de senha, associando-o ao usuário e definindo uma expiração para o token.
        public string GerarTokenRecup(string email)
        {
            var usuario = usuarios.FirstOrDefault(u => u.Email == email);
            if (usuario == null) { return null; }
            string token = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
            usuario.TokenRecuperacao = token;
            usuario.TokenExpiracao = DateTime.Now.AddMinutes(10);
            return token;
        }

        // troca a senha do usuário, verificando o token de recuperação e sua validade antes de permitir a alteração da senha.
        public bool RedefinirSenha(string email, string token, string novaSenha)
        {
            //verifica se o email existe, se o token é válido e se não expirou
            var usuario = usuarios.FirstOrDefault(u => u.Email == email);
            if (usuario == null)
            { 
                return false; 
            }
            if (usuario.TokenRecuperacao != token)
            {
                return false;
            }
            if (usuario.TokenExpiracao < DateTime.Now)
            {
                return false;
            }
            // se tudo estiver correto, gera o hash da nova senha e atualiza o usuário, além de limpar o token de recuperação e sua expiração.
            usuario.SenhaHash = GerarHash(novaSenha);
            usuario.TokenRecuperacao = null;
            usuario.TokenExpiracao = null;
            return true;
        }
    }
}

