using ProjetoAcelera.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ProjetoAcelera.Services
{
    public class ArquivoService
    {
        private string caminhoArquivo = "usuarios.json";


        //carrega os usuariod do arquivo json e retorna uma lista de usuarios
        public List<Usuario> Carregar()
        {
            try 
            {
                // se n existir arquivo, retorna lista vazia
                if (!File.Exists(caminhoArquivo)) 
                {
                    return new List<Usuario>();
                }
                string json = File.ReadAllText(caminhoArquivo);

                //se estiver vazio evita erro
                if (string.IsNullOrWhiteSpace(json)) 
                {
                    return new List<Usuario>();
                }

                return JsonSerializer.Deserialize<List<Usuario>>(json);
            }
            catch 
            {
                //se der erro , evita travar.
                return new List<Usuario>();
            }
        }

        // recebe a lista dos usuarios para salvar no json
        public void Salvar(List<Usuario> usuarios) 
        {
            try 
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(usuarios, options);

                File.WriteAllText(caminhoArquivo, json);
            }
            catch 
            {
                //posso colocar alguma mensagem aqui de erro
            }
        
        
        }



        public void SalvarUsuariosComImagens(List<Usuario> usuarios)
        {
            try
            {
                string pastaPerfil = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"ImagemAcelera","Perfil");
                string pastaPublicacoes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"ImagemAcelera","Publicacoes");

                Directory.CreateDirectory(pastaPerfil);
                Directory.CreateDirectory(pastaPublicacoes);

                foreach (var usuario in usuarios)
                {
                    // FOTO DE PERFIL
                    if (!string.IsNullOrWhiteSpace(usuario?.Perfil?.FotoPerfil))
                    {
                        usuario.Perfil.FotoPerfil = CopiarImagemSeNecessario(usuario.Perfil.FotoPerfil,pastaPerfil);
                    }

                    // IMAGENS DAS PUBLICAÇÕES
                    if (usuario?.Publicacoes != null)
                    {
                        foreach (var publicacao in usuario.Publicacoes)
                        {
                            if (!string.IsNullOrWhiteSpace(publicacao.ImagemUrl))
                            {
                                publicacao.ImagemUrl = CopiarImagemSeNecessario(
                                    publicacao.ImagemUrl,
                                    pastaPublicacoes
                                );
                            }
                        }
                    }
                }

                Salvar(usuarios);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar usuários com imagens: " + ex.Message);
            }
        }
        private string CopiarImagemSeNecessario(string caminhoImagem, string pastaDestino)
        {
            if (string.IsNullOrWhiteSpace(caminhoImagem))
                return caminhoImagem;

            if (!File.Exists(caminhoImagem))
                return caminhoImagem;

            Directory.CreateDirectory(pastaDestino);

            string caminhoCompletoImagem = Path.GetFullPath(caminhoImagem);
            string caminhoCompletoPasta = Path.GetFullPath(pastaDestino);

            // Se a imagem ta dentro da pasta de destino, n copia de novo
            if (caminhoCompletoImagem.StartsWith(caminhoCompletoPasta))
            {
                return caminhoImagem;
            }

            string nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(caminhoImagem);
            string destinoFinal = Path.Combine(pastaDestino, nomeArquivo);
            File.Copy(caminhoImagem, destinoFinal, true);

            return destinoFinal;
        }
    }
}




     