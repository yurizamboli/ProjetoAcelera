using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAcelera.Services
{
    internal class PerfilService
    {
        private UsuarioService usuarioService;

        public PerfilService(UsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
        }
        public void AtualizarPerfil(string facebook, string instagram, string bio)
        {
            var UsuarioLogado = usuarioService.UsuarioLogado;
            if (UsuarioLogado == null)
            {
                return;
            }

            UsuarioLogado.Perfil.Facebook = facebook;
            UsuarioLogado.Perfil.Instagram = instagram;
            UsuarioLogado.Perfil.Bio = bio;
        }


    }
}
