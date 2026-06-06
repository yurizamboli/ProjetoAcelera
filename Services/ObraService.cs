using ProjetoAcelera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// cuida de toda a lógica relacionada às obras dos usuários.
namespace ProjetoAcelera.Services
{
    public class ObraService
    {
        private UsuarioService usuarioService;
       
        public ObraService()
        {
            this.usuarioService = App.UsuarioService;
        }

        // Adiciona uma nova obra à lista de obras do usuário logado, criando um novo objeto Obra com os dados fornecidos
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

        // Remove uma obra da lista de obras do usuário logado
        public void RemoverObra(string titulo)
        {
            var usuario = usuarioService.UsuarioLogado;
            var obra = usuario.Obras.FirstOrDefault(o => o.Titulo == titulo);
            if (obra != null)
            {
                usuario.Obras.Remove(obra);
            }
        }

        // Altera o status de favorito de uma obra
        public void FavoritarObra(string titulo)
        {
            var usuario = usuarioService.UsuarioLogado;
            var obra = usuario.Obras.FirstOrDefault(o => o.Titulo == titulo);
            if (obra != null)
            {
                obra.Favorito = !obra.Favorito;
            }
        }

        // Retorna uma lista de obras que estão marcadas como favoritas
        public List<Obra> ListarFavoritas()
        {
            var usuario = usuarioService.UsuarioLogado;
            return usuario.Obras.Where(o => o.Favorito).ToList();
        }

        //atualiza uma obra usando os novos dados enviados
        public void AtualizarObra(Obra obraAtualizada)
        {
            var usuario = usuarioService.UsuarioLogado;
            if (usuario == null) return;
            var obra = usuario.Obras.FirstOrDefault(o => o.Titulo == obraAtualizada.Titulo);

            if (obra != null)
            {
                obra.Titulo = obraAtualizada.Titulo;
                obra.Descricao = obraAtualizada.Descricao;
                obra.Capa = obraAtualizada.Capa;
            }
        }
    }
}
