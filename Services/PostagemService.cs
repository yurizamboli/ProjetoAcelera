using ProjetoAcelera.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoAcelera.Services
{
    public class PostagemService
    {
        private readonly List<Postagem> postagens = new();

        public void AdicionarPostagem(Postagem postagem)
        {
            if (postagem != null)
            {
                postagens.Add(postagem);
            }
        }

        public List<Postagem> ObterPendentes()
        {
            return postagens
                .Where(p => p.Status == "Aguardando aprovação")
                .OrderByDescending(p => p.DataCriacao)
                .ToList();
        }

        public List<Postagem> ObterTodos()
        {
            return postagens
                .OrderByDescending(p => p.DataCriacao)
                .ToList();
        }

        public void AprovarPostagem(Guid id)
        {
            var postagem = postagens.FirstOrDefault(p => p.Id == id);
            if (postagem != null)
                postagem.Status = "Aprovado";
        }

        public void ReprovarPostagem(Guid id)
        {
            var postagem = postagens.FirstOrDefault(p => p.Id == id);
            if (postagem != null)
                postagem.Status = "Reprovada";
        }
    }
}
