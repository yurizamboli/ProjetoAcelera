using ProjetoAcelera.Models;

// Métodos usados pelo administrador.
namespace ProjetoAcelera.Services
{
    public class AdminService
    {
        private UsuarioService usuarioService;
   
        public AdminService(UsuarioService service)
        {
            usuarioService = service;
        }

        // Verifica se o usuário logado é admin, caso contrário lança uma exceção.
        private void VerificarAdmin()
        {
            var user = usuarioService.UsuarioLogado;
            if (user == null || user.Cargo != "Admin")
            { 
                throw new Exception("Acesso negado");
            }
        }

        // Promove um usuário para o cargo de admin.
        public void PromoverParaAdmin(string email)
        {
            VerificarAdmin();
            var user = usuarioService.ObterTodos().FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.Cargo = "Admin";
            }
        }

        // Marca um usuário como destaque na tela dos artistas.
        public void TornarDestaque(string email)
        {
            VerificarAdmin();
            var usuario = usuarioService.ObterTodos().FirstOrDefault(u => u.Email == email);
            if (usuario == null)
            {
                throw new Exception("Usuário não encontrado.");
            }
            if (usuario.Perfil == null)
            {
                usuario.Perfil = new Perfil();
            }
            usuario.Perfil.Destaque = true;
        }

        // Banir um usuário, impedindo-o de acessar o sistema.
        public void BanirUsuario(string email)
        {
            VerificarAdmin();
            var user = usuarioService.ObterTodos().FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.Banido = true;
            }
        }
    }
}