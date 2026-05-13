using ProjetoAcelera.Models;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace ProjetoAcelera.Views.Perfil
{
    public partial class ContainerObrasVisual : Page
    {
        private Usuario usuario;

        public ContainerObrasVisual(Usuario user)
        {
            InitializeComponent();

            usuario = user;

            CarregarObras();
        }

        private void CarregarObras()
        {
            painelObras.Children.Clear();

            if (usuario.Obras == null)
                return;

            foreach (var obra in usuario.Obras)
            {
                painelObras.Children.Add(CriarCard(obra));
            }
        }

        private Border CriarCard(Obra obra)
        {
            StackPanel container = new StackPanel();

            Image img = new Image
            {
                Height = 150,
                Stretch = Stretch.UniformToFill
            };

            try
            {
                img.Source =
                    new BitmapImage(
                        new Uri(obra.Capa));
            }
            catch
            {

            }

            TextBlock titulo = new TextBlock
            {
                Text = obra.Titulo,
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(5)
            };

            container.Children.Add(img);
            container.Children.Add(titulo);

            return new Border
            {
                Width = 180,
                Height = 220,
                Margin = new Thickness(10),
                CornerRadius = new CornerRadius(12),
                BorderBrush = Brushes.DarkGoldenrod,
                BorderThickness = new Thickness(2),
                Background = Brushes.White,
                Child = container
            };
        }
    }
}
