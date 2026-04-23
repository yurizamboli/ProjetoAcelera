using ProjetoAcelera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAcelera.Services
{
    public class ObraService
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

        

        public void RemoverObra(string titulo)
        {
            var usuario = usuarioService.UsuarioLogado;

            var obra = usuario.Obras.FirstOrDefault(o => o.Titulo == titulo);

            if (obra != null)
            {
                usuario.Obras.Remove(obra);
            }
        }
        //Fiz o metodo
        public void EditarObra(string titulo, string novoTitulo, string novaDescricao, string novaCapa)
        {
            var usuario = usuarioService.UsuarioLogado;

            var obra = usuario.Obras.FirstOrDefault(o => o.Titulo == titulo);

            if (obra != null)
            {
                obra.Titulo = novoTitulo;
                obra.Descricao = novaDescricao;
                obra.Capa = novaCapa;
            }
        }

        //Fiz o metodo aqui
        public void FavoritarObra(string titulo)
        {
            var usuario = usuarioService.UsuarioLogado;

            var obra = usuario.Obras.FirstOrDefault(o => o.Titulo == titulo);

            if (obra != null)
            {
                obra.Favorito = !obra.Favorito;
            }
        }

        //Mudei pra lista de obras
        public List<Obra> ListarFavoritas()
        {
            var usuario = usuarioService.UsuarioLogado;

            return usuario.Obras.Where(o => o.Favorito).ToList();
        }
        //ADD outro metodo, depois verifica isso aqui
        public void AtualizarObra(Obra obraAtualizada)
        {
            var usuario = usuarioService.UsuarioLogado;

            if (usuario == null) return;

            var obra = usuario.Obras
                .FirstOrDefault(o => o.Titulo == obraAtualizada.Titulo);

            if (obra != null)
            {
                obra.Titulo = obraAtualizada.Titulo;
                obra.Descricao = obraAtualizada.Descricao;
                obra.Capa = obraAtualizada.Capa;
            }
        }
    }
}
