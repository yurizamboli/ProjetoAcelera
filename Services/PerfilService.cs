using ProjetoAcelera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAcelera.Services
{
    public class PerfilService
    {
        private UsuarioService usuarioService;

        public PerfilService(UsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
        }
        //o atualizar perfil vai ficar em uma tela de ediçao entao vai ser só 1 metodo
        public void AtualizarPerfil(string nome, string bio, string facebook, string instagram, string foto)
        {
            var UsuarioLogado = usuarioService.UsuarioLogado;
            if (UsuarioLogado == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(nome))
                throw new Exception("Nome não pode estar vazio.");

            if (nome.Length > 50)
                throw new Exception("Nome deve ter no máximo 50 caracteres.");

            if (bio != null && bio.Length > 200)
                throw new Exception("Bio deve ter no máximo 200 caracteres.");

            if (!string.IsNullOrWhiteSpace(instagram) && !instagram.StartsWith("@"))
                throw new Exception("Instagram deve começar com @");

            UsuarioLogado.Nome = nome;
            UsuarioLogado.Perfil.Bio = bio;
            UsuarioLogado.Perfil.Facebook = facebook;
            UsuarioLogado.Perfil.Instagram = instagram;
            UsuarioLogado.Perfil.FotoPerfil = foto;
        }
    

    public Perfil ObterPerfil()
        {
            var UsuarioLogado = usuarioService.UsuarioLogado;

            if (UsuarioLogado == null)
            {
                return null;
            }

            return UsuarioLogado.Perfil;

        }

    }


}
