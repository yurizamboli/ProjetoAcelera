using ProjetoAcelera.Models;
// cuida de toda a lógica relacionada ao perfil dos usuários
namespace ProjetoAcelera.Services
{
    public class PerfilService
    {
        private UsuarioService usuarioService;

        public PerfilService(UsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
        }

        //atualiza os dados do perfil do usuário logado
        public void AtualizarPerfil(string nome, string bio, string facebook, string instagram, string foto)
        {
            var UsuarioLogado = usuarioService.UsuarioLogado;
            if (UsuarioLogado == null)
            {
                return;
            }
            
            // validações básicas para os campos do perfil
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new Exception("Nome não pode estar vazio.");
            }
            if (nome.Length > 50)
            {
                throw new Exception("Nome deve ter no máximo 50 caracteres.");
            }
            if (bio != null && bio.Length > 250)
            {
                throw new Exception("Bio deve ter no máximo 250 caracteres.");
            }
            if (!string.IsNullOrWhiteSpace(instagram) && !instagram.StartsWith("@"))
            {
                throw new Exception("Instagram deve começar com @");
            }
            UsuarioLogado.Nome = nome;
            var publicacaoService = new PublicacaoService();
            publicacaoService.AtualizarNomeAutor(UsuarioLogado.Email, nome);
            UsuarioLogado.Perfil.Bio = bio;
            UsuarioLogado.Perfil.Facebook = facebook;
            UsuarioLogado.Perfil.Instagram = instagram;
            UsuarioLogado.Perfil.FotoPerfil = foto;
        }


    }
}
