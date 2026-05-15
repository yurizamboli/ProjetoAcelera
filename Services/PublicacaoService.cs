using ProjetoAcelera.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAcelera.Services
{
    class PublicacaoService
    {
        private UsuarioService usuarioService;
        public PublicacaoService()
        {
            this.usuarioService = App.UsuarioService;
        }

        public void AdicionarPublicacao(string conteudo, string caminhoImagemOriginal)
        {
            AdicionarPublicacao(conteudo, caminhoImagemOriginal, string.Empty, true);
        }

        public void AdicionarPublicacao(string conteudo, string caminhoImagemOriginal, string caminhoVideoOriginal, bool comentariosPermitidos)
        {
            var usuarioLogado = usuarioService.UsuarioLogado;

            if (usuarioLogado == null)
            {
                return;
            }

            Publicacao novaPublicacao = new Publicacao
            {
                NomeAutor = usuarioLogado.Nome,
                EmailAutor = usuarioLogado.Email,
                Conteudo = conteudo,
                ImagemUrl = caminhoImagemOriginal,
                CaminhoVideo = caminhoVideoOriginal,
                ComentariosPermitidos = comentariosPermitidos,
                Visualizacoes = 0,
                Status = "Aguardando aprovação",
                DataPublicacao = DateTime.Now
            };

            usuarioLogado.Publicacoes.Add(novaPublicacao);
        }

        public void RemoverPublicacao(Guid id)
        {
            var usuarioLogado = usuarioService.UsuarioLogado;

            if (usuarioLogado == null)
            {
                return;
            }

            var publicacao = usuarioLogado.Publicacoes
                .FirstOrDefault(p => p.Id == id);

            if (publicacao == null)
            {
                return;
            }

            usuarioLogado.Publicacoes.Remove(publicacao);

        }
        public void AlternarCurtida(Guid idPublicacao)
        {
            var usuarioLogado = usuarioService.UsuarioLogado;

            if (usuarioLogado == null)
            {
                return;
            }

            var usuarios = usuarioService.ObterTodos();

            if (usuarios == null)
            {
                return;
            }

            var publicacao = usuarios
                .Where(u => u.Publicacoes != null)
                .SelectMany(u => u.Publicacoes)
                .FirstOrDefault(p => p.Id == idPublicacao);

            if (publicacao == null)
            {
                return;
            }

            bool jaCurtiu = publicacao.CurtidoPor.Contains(usuarioLogado.Email);

            if (jaCurtiu)
            {
                publicacao.CurtidoPor.Remove(usuarioLogado.Email);
            }
            else
            {
                publicacao.CurtidoPor.Add(usuarioLogado.Email);
            }
        }

        public bool UsuarioCurtiu(Publicacao publicacao)
        {
            var usuarioLogado = usuarioService.UsuarioLogado;

            if (usuarioLogado == null || publicacao == null)
            {
                return false;
            }

            return publicacao.CurtidoPor
                .Contains(usuarioLogado.Email);
        }

        public List<Publicacao> ObterPublicacoesPerfil()
        {
            var usuarioLogado = usuarioService.UsuarioLogado;
            if (usuarioLogado == null || usuarioLogado.Publicacoes == null)
            {
                return new List<Publicacao>();
            }

            return usuarioLogado.Publicacoes.OrderByDescending(p => p.DataPublicacao).ToList();
        }

        public List<Publicacao> ObterFeedGlobal()
        {
            var usuarios = usuarioService.ObterTodos();

            if (usuarios == null)
            {
                return new List<Publicacao>();
            }

            return usuarios
                .Where(u => u.Publicacoes != null)
                .SelectMany(u => u.Publicacoes)
                .Where(p => p.Status == "Aprovado")
                .OrderByDescending(p => p.DataPublicacao)
                .ToList();
        }
        public List<Publicacao> ObterPendentes()
        {
            var usuarios = usuarioService.ObterTodos();

            if (usuarios == null)
            {
                return new List<Publicacao>();
            }

            return usuarios
                .Where(u => u != null && u.Publicacoes != null)
                .SelectMany(u => u.Publicacoes)
                .Where(p => p != null && p.Status == "Aguardando aprovação")
                .OrderByDescending(p => p.DataPublicacao)
                .ToList();
        }


        public void AprovarPublicacao(Guid id)
        {
            var publicacao = usuarioService.ObterTodos()
                .Where(u => u.Publicacoes != null)
                .SelectMany(u => u.Publicacoes)
                .FirstOrDefault(p => p.Id == id);

            if (publicacao != null)
            {
                publicacao.Status = "Aprovado";
            }
        }
        public void ReprovarPublicacao(Guid id)
        {
            var usuarios = usuarioService.ObterTodos();
            if (usuarios == null)
            {
                return;
            }
            foreach (var usuario in usuarios)
            {
                if (usuario.Publicacoes == null)
                {
                    continue;
                }
                var publicacao = usuario.Publicacoes.FirstOrDefault(p => p.Id == id);
                if (publicacao != null)
                {
                    usuario.Publicacoes.Remove(publicacao);
                    return;
                }
            }
        }
        public void AtualizarNomeAutor(string email, string novoNome)
        {
            var usuarios = usuarioService.ObterTodos();

            if (usuarios == null)
            {
                return;
            }

            foreach (var usuario in usuarios)
            {
                if (usuario.Publicacoes == null)
                {
                    continue;
                }

                foreach (var publicacao in usuario.Publicacoes)
                {
                    if (publicacao.EmailAutor == email)
                    {
                        publicacao.NomeAutor = novoNome;
                    }
                }
            }
        }
    }
}