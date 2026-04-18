using ProjetoAcelera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAcelera.Services
{
    internal class ObraService
    {
        private UsuarioService usuarioService;

        public ObraService(UsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
        }


        public void AdicionarObra(string titulo, string descricao, string capa)
        {
            var UsuarioLogado = usuarioService.UsuarioLogado;

            if (UsuarioLogado == null)
            {
                return;
            }
            Obra novaObra = new Obra
            {
                Titulo = titulo,
                Descricao = descricao,
                Capa = capa
            };
            UsuarioLogado.Obras.Add(novaObra);
        }

        public void ListarObras()
        {
            var UsuarioLogado = usuarioService.UsuarioLogado;

            if (UsuarioLogado == null)
            {
                return;
            }

            foreach (var obra in UsuarioLogado.Obras)
            {
                Console.WriteLine($"Título: {obra.Titulo}");
                Console.WriteLine($"Descrição: {obra.Descricao}");
                Console.WriteLine($"Capa: {obra.Capa}");
                Console.WriteLine("------------------");
            }
        }




    }
}
