using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

// cuida de carregar as imagens de forma otimizada, para evitar problemas de desempenho e consumo excessivo de memória.
namespace ProjetoAcelera.Ferramentas
{
    public static class AuxilioImagens
    {
        // carrega a imagem usando um tamanho menor , para não pesar tanto na tela
        public static BitmapImage CarregarImgOtimizada(string caminho, int largura = 500)
        {
            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();
            // verifica se a imagem vem do pacote de recursos ou de uma pasta do computador
            if (caminho.StartsWith("pack://"))
            {
                bitmap.UriSource = new Uri(caminho, UriKind.Absolute);
            }
            else if (File.Exists(caminho))
            {
                bitmap.UriSource = new Uri(caminho, UriKind.Absolute);
            }
            else
            {
                bitmap.UriSource = new Uri(caminho, UriKind.RelativeOrAbsolute);
            }
            // define a opção de cache para carregar a imagem completamente na memória, evitando problemas de bloqueio de arquivos e melhorando o desempenho
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.DecodePixelWidth = largura;
            bitmap.EndInit();
            bitmap.Freeze();
            // libera a imagem da memória quando não for mais necessária
            return bitmap;
        }
    }
}
