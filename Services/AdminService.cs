using ProjetoAcelera.Models;
using System;
using System.Linq;

namespace ProjetoAcelera.Services
{
    public class AdminService
    {
        private UsuarioService usuarioService;

        public AdminService(UsuarioService service)
        {
            usuarioService = service;
        }

        private void VerificarAdmin()
        {
            var user = usuarioService.UsuarioLogado;

            if (user == null || user.Cargo != "Admin")
                throw new Exception("Acesso negado");
        }

        public void PromoverParaAdmin(string email)
        {
            VerificarAdmin();

            var user = usuarioService.ObterTodos()
                .FirstOrDefault(u => u.Email == email);

            if (user != null)
                user.Cargo = "Admin";
        }

        public void TornarDestaque(string email)
        {
            VerificarAdmin();

            var user = usuarioService.ObterTodos()
                .FirstOrDefault(u => u.Email == email);

            if (user != null && user.Perfil != null)
                user.Perfil.Destaque = true;
        }
        public void BanirUsuario(string email)
        {
            VerificarAdmin();

            var user = usuarioService.ObterTodos()
                .FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                user.Banido = true;
            }
        }

    }
}