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
    }
}
     