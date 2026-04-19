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
        public void AtualizarPerfil(string facebook, string instagram, string bio,string foto)
        {
            var UsuarioLogado = usuarioService.UsuarioLogado;
            if (UsuarioLogado == null)
            {
                return;
            }

            UsuarioLogado.Perfil.Facebook = facebook;
            UsuarioLogado.Perfil.Instagram = instagram;
            UsuarioLogado.Perfil.Bio = bio;
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
