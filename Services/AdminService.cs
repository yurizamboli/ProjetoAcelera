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

        public void PromoverParaAdmin(string nome)
        {
            VerificarAdmin();

            var user = usuarioService.ObterTodos()
                .FirstOrDefault(u => u.Nome == nome);

            if (user != null)
                user.Cargo = "Admin";
        }

        public void TornarDestaque(string nome)
        {
            VerificarAdmin();

            var user = usuarioService.ObterTodos()
                .FirstOrDefault(u => u.Nome == nome);

            if (user != null && user.Perfil != null)
                user.Perfil.Destaque = true;
        }
        public void RemoverUsuario(string nome)
        {
            var userLogado = usuarioService.UsuarioLogado;

            if (userLogado == null || userLogado.Cargo != "Admin")
                throw new Exception("Acesso negado");

            var lista = usuarioService.ObterTodos();

            var user = lista.FirstOrDefault(u => u.Nome == nome);

            if (user != null)
            {
                lista.Remove(user);
            }
        }

    }
}