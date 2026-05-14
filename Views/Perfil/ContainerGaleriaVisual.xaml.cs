using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace ProjetoAcelera.Views.Perfil
{
    public partial class ContainerGaleriaVisual : Page
    {
        private Usuario usuario;

        public ContainerGaleriaVisual(Usuario user)
        {
            InitializeComponent();

            usuario = user;

            CarregarGaleria();
        }

        private void CarregarGaleria()
        {
            painelGaleria.Children.Clear();

            if (usuario.Publicacoes == null)
                return;

            var posts = usuario.Publicacoes
                .Where(p => p.Status == "Aprovado" && !string.IsNullOrWhiteSpace(p.ImagemUrl)).ToList();
            
           
            foreach (var post in posts)
            {
                painelGaleria.Children.Add(CriarImagem(post.ImagemUrl));
            }
        }

        private Border CriarImagem(string caminho)
        {
            Image img = new Image
            {
                Width = 180,
                Height = 180,
                Stretch = Stretch.UniformToFill
            };

            try
            {
                img.Source = new BitmapImage(new Uri(caminho));
            }
            catch
            {

            }        
            Border border = new Border
            {
                Width = 180,
                Height = 180,
                Margin = new Thickness(10),
                CornerRadius = new CornerRadius(12),
                ClipToBounds = true,
                BorderBrush = Brushes.DarkGoldenrod,
                BorderThickness = new Thickness(2),
                Child = img
            };
            border.MouseDown += (s, e) =>
            { 
                var janela = new JanelaImagemFull(caminho); janela.ShowDialog(); 
            };
            return border;
        }
    }
}