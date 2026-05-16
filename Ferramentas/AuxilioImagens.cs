using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProjetoAcelera.Ferramentas
{
    public static class AuxilioImagens
    {
        public static BitmapImage CarregarImgOtimizada(string caminho, int largura = 500)
        {
            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();

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

            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.DecodePixelWidth = largura;
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }
    }
}
