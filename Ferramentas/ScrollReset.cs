using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

// cuida de resetar a posição dos scrolls para o topo, evitando que fiquem em posições estranhas quando o usuário navega entre telas.
namespace ProjetoAcelera.Ferramentas
{
    public static class ScrollReset
    {
        //procura um scrollviewer dentro da tela
        public static void ResetarScrolls(DependencyObject elemento)
        {
            if (elemento == null)
            {
                return;
            }

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>{Resetar(elemento);}), DispatcherPriority.ContextIdle);
        }

        // volta a rolagem para o topo, tanto vertical quanto horizontal
        private static void Resetar(DependencyObject elemento)
        {
            if (elemento == null)
            {
                return;
            }
            if (elemento is ScrollViewer scroll)
            {
                scroll.ScrollToVerticalOffset(0);
                scroll.ScrollToHorizontalOffset(0);
                scroll.ScrollToTop();
            }
            int totalFilhos = VisualTreeHelper.GetChildrenCount(elemento);
            for (int i = 0; i < totalFilhos; i++)
            {
                DependencyObject filho = VisualTreeHelper.GetChild(elemento, i);
                Resetar(filho);
            }
        }
    }
}
