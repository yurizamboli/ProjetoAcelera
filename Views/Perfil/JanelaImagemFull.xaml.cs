using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProjetoAcelera.Views.Perfil
{
    public partial class JanelaImagemFull : Window
    {
        public JanelaImagemFull(string caminhoImagem)
        {
            InitializeComponent();

            imgGaleria.Source = new BitmapImage(
                new Uri(caminhoImagem, UriKind.RelativeOrAbsolute));
        }

        private void Fechar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}