using ProjetoAcelera.Models;
using System.Collections.Generic;

namespace ProjetoAcelera.Services
{
    public class PostagemService
    {
        private List<Postagem> posts = new List<Postagem>();

        public List<Postagem> ObterPosts()
        {
            return posts;
        }

        public void Adicionar(Postagem post)
        {
            posts.Add(post);
        }
    }
}