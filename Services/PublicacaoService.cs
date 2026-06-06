using ProjetoAcelera.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// cuida de toda a lógica relacionada às publicações dos usuários
namespace ProjetoAcelera.Services
{
    class PublicacaoService
    {
        private UsuarioService usuarioService;
        
        public PublicacaoService()
        {
            this.usuarioService = App.UsuarioService;
        }

        // cria uma nova publicação para o usuário logado
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
                Status = "Aguardando aprovação", // publicação começa aguardando aprovação do admin
                DataPublicacao = DateTime.Now
            };
            usuarioLogado.Publicacoes.Add(novaPublicacao);
        }

        // remove uma publicação específica do usuário logado, identificada por seu ID
        public void RemoverPublicacao(Guid id)
        {
            var usuarioLogado = usuarioService.UsuarioLogado;
            if (usuarioLogado == null)
            {
                return;
            }
            var publicacao = usuarioLogado.Publicacoes.FirstOrDefault(p => p.Id == id);
            if (publicacao == null)
            {
                return;
            }
            usuarioLogado.Publicacoes.Remove(publicacao);
        }

        // alternar o status de curtida de uma publicação
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
            var publicacao = usuarios.Where(u => u.Publicacoes != null).SelectMany(u => u.Publicacoes).FirstOrDefault(p => p.Id == idPublicacao);
            if (publicacao == null)
            {
                return;
            }
            bool jaCurtiu = publicacao.CurtidoPor.Contains(usuarioLogado.Email);
            if (jaCurtiu)
            {
                publicacao.CurtidoPor.Remove(usuarioLogado.Email); // se o usuário ja curtiu, remove a curtida
            }
            else
            {
                publicacao.CurtidoPor.Add(usuarioLogado.Email); // se ainda n curtiu adiciona a curtida
            }
        }

        // verifica se o usuário logado já curtiu a publicação
        public bool UsuarioCurtiu(Publicacao publicacao)
        {
            var usuarioLogado = usuarioService.UsuarioLogado;
            if (usuarioLogado == null || publicacao == null)
            {
                return false;
            }
            return publicacao.CurtidoPor.Contains(usuarioLogado.Email);
        }

        // retorna as publicações do usuário logado
        public List<Publicacao> ObterPublicacoesPerfil()
        {
            var usuarioLogado = usuarioService.UsuarioLogado;
            if (usuarioLogado == null || usuarioLogado.Publicacoes == null)
            {
                return new List<Publicacao>();
            }
            //ordena pelas publicações mais recentes primeiro
            return usuarioLogado.Publicacoes.OrderByDescending(p => p.DataPublicacao).ToList();
        }

        // retorna as publicações aprovadas para mostrar no feed global
        public List<Publicacao> ObterFeedGlobal()
        {
            var usuarios = usuarioService.ObterTodos();
            if (usuarios == null)
            {
                return new List<Publicacao>();
            }
            // coloquei para as postagens aparecerem em ordem de envio, pois a aprovação pode demorar e ficar desordenada
            return usuarios.Where(u => u.Publicacoes != null).SelectMany(u => u.Publicacoes)
                .Where(p => p.Status == "Aprovado").OrderByDescending(p => p.DataAprovacao ?? p.DataPublicacao).ToList();
        }

        // retorna as publicações que estão aguardando aprovação para o admin revisar
        public List<Publicacao> ObterPendentes()
        {
            var usuarios = usuarioService.ObterTodos();
            if (usuarios == null)
            {
                return new List<Publicacao>();
            }
            return usuarios.Where(u => u != null && u.Publicacoes != null).SelectMany(u => u.Publicacoes)
                .Where(p => p != null && p.Status == "Aguardando aprovação").OrderByDescending(p => p.DataPublicacao).ToList();
        }

        // aprova uma publicaç]ao para ela aparecer no feed global
        public void AprovarPublicacao(Guid id)
        {
            var publicacao = usuarioService.ObterTodos().Where(u => u.Publicacoes != null).SelectMany(u => u.Publicacoes).FirstOrDefault(p => p.Id == id);
            if (publicacao != null)
            {
                publicacao.Status = "Aprovado";
                publicacao.DataAprovacao = DateTime.Now;
            }
        }

        // reprova uma publicação, mantendo ela no perfil do usuário mas sem aparecer no feed global
        public void ReprovarPublicacao(Guid id)
        {
            var publicacao = usuarioService.ObterTodos().Where(u => u.Publicacoes != null).SelectMany(u => u.Publicacoes).FirstOrDefault(p => p.Id == id);
            if (publicacao != null)
            {
                publicacao.Status = "Reprovada";
            }
        }

        // atualiza o nome do autor em todas as publicações quando o usuário muda seu nome no perfil
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

        // adiciona um comentário em uma publicação
        public void AdicionarComentario(Guid idPublicacao, string nomeAutor, string emailAutor, string conteudo)
        {
            var usuarios = usuarioService.ObterTodos();
            if (usuarios == null)
            {
                return;
            }
            var publicacao = usuarios.Where(u => u.Publicacoes != null).SelectMany(u => u.Publicacoes).FirstOrDefault(p => p.Id == idPublicacao);
            if (publicacao == null || !publicacao.ComentariosPermitidos)
            {
                return;
            }
            Comentario novoComentario = new Comentario
            {
                NomeAutor = nomeAutor,
                EmailAutor = emailAutor,
                Conteudo = conteudo,
                DataComentario = DateTime.Now
                // o status ja esta no como "aguardando aprovação" por padrão no construtor da classe Comentario
            };
            publicacao.Comentarios.Add(novoComentario);
        }

        // aprova um comentário pendente para ele aparecer na publicação
        public void AprovarComentario(Guid idComentario)
        {
            var comentario = usuarioService.ObterTodos().Where(u => u.Publicacoes != null).SelectMany(u => u.Publicacoes)
                .Where(p => p.Comentarios != null).SelectMany(p => p.Comentarios).FirstOrDefault(c => c.Id == idComentario);
            if (comentario != null)
            {
                comentario.Status = "Aprovado";
            }
        }

        // reprova um comentário, removendo ele da publicação
        public void ReprovarComentario(Guid idComentario)
        {
            var publicacoes = usuarioService.ObterTodos().Where(u => u.Publicacoes != null).SelectMany(u => u.Publicacoes);
            foreach (var publicacao in publicacoes)
            {
                var comentario = publicacao.Comentarios.FirstOrDefault(c => c.Id == idComentario);
                if (comentario != null)
                {
                    publicacao.Comentarios.Remove(comentario);
                    return;
                }
            }
        }
        
        //retorna as publicações que aparecem na home(feed global).
        public List<Publicacao> ObterPublicadasNaHome()
        {
            var usuarios = usuarioService.ObterTodos();
            if (usuarios == null)
            {
                return new List<Publicacao>();
            }
            // mais recentes primeiro.
            return usuarios.Where(u => u != null && u.Publicacoes != null).SelectMany(u => u.Publicacoes).Where(p => p != null && p.Status == "Aprovado")
                .OrderByDescending(p => p.DataAprovacao ?? p.DataPublicacao).ToList();
        }

        // remove uma publicação que estava aprovada no fee global. Ela continua no perfil do usuário mas com o status "Removida da Home" e não aparece mais no feed global.
        public void RemoverDaHome(Guid id)
        {
            var publicacao = usuarioService.ObterTodos().Where(u => u.Publicacoes != null).SelectMany(u => u.Publicacoes).FirstOrDefault(p => p.Id == id);
            if (publicacao != null)
            {
                publicacao.Status = "Removida da Home";
            }
        }
    }
}