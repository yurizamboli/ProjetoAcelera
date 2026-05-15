using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjetoAcelera.Views.Perfil
{
    public partial class ContainerPostsVisual : Page
    {
        private Usuario usuario;

        public ContainerPostsVisual(Usuario user)
        {
            InitializeComponent();

            usuario = user;

            CarregarPosts();
        }

        private void CarregarPosts()
        {
            painelPosts.Children.Clear();

            if (usuario.Publicacoes == null)
                return;

            var posts = usuario.Publicacoes
     .OrderByDescending(p => p.DataPublicacao)
     .ToList();
            foreach (var post in posts)
            {
                painelPosts.Children.Add(CriarPost(post));
            }
        }

        private Border CriarPost(Publicacao post)
        {

            StackPanel container = new StackPanel();

            TextBlock texto = new TextBlock
            {
                Text = post.Conteudo,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 10),
                Foreground = Brushes.Black,
                FontSize = 14
            };

            container.Children.Add(texto);

            if (!string.IsNullOrWhiteSpace(post.ImagemUrl))
            {
                Border bordaImagem = new Border
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E1CF")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BDAE84")),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(12),
                    Padding = new Thickness(6),
                    Margin = new Thickness(0, 0, 0, 12)
                };
                Image img = new Image
                {
                    MaxHeight = 420,
                    Stretch = Stretch.Uniform,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                try
                {
                    img.Source = new BitmapImage(new Uri(post.ImagemUrl));
                    bordaImagem.Child = img;
                   
                }
                catch
                {

                }

                container.Children.Add(bordaImagem);
            }

            TextBlock likes = new TextBlock
            {
                Text = $"{post.Curtidas} curtidas",
                Margin = new Thickness(0, 10, 0, 0),
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.DarkGoldenrod
            };

            container.Children.Add(likes);

            return new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF7E1")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C9B27D")),
                CornerRadius = new CornerRadius(16),
                BorderThickness = new Thickness(2),
                Margin = new Thickness(0, 0, 0, 20),
                Padding = new Thickness(15),
                Child = container
            };
        }
    }
}