using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoAcelera.Models;
using System.Text.Json;
using System.Data.SqlTypes;
using System.IO;

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
                string pastaPerfil = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "ImagemAcelera",
                    "Perfil"
                );

                if (!Directory.Exists(pastaPerfil))
                    Directory.CreateDirectory(pastaPerfil);

                foreach (var usuario in usuarios)
                {
                    if (usuario?.Perfil?.FotoPerfil == null)
                        continue;

                    string caminhoImagem = usuario.Perfil.FotoPerfil;           
                    if (File.Exists(caminhoImagem))
                    {
                        string nomeArquivo = Guid.NewGuid() + Path.GetExtension(caminhoImagem);
                        string destinoFinal = Path.Combine(pastaPerfil, nomeArquivo);

                        File.Copy(caminhoImagem, destinoFinal, true);
                        usuario.Perfil.FotoPerfil = destinoFinal;
                    }
                }
                Salvar(usuarios);
            }
            catch
            {
                // msg de erro
            }
        }
    }
}




     