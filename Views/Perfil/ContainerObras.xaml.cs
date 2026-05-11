using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using ProjetoAcelera.Views.EditarObras;
using ProjetoAcelera.Views.Obras;
using ProjetoAcelera.Views.PopUpObras;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjetoAcelera.Views.Perfil
{
    public partial class ContainerObras : Page
    {
        private UsuarioService usuarioService;

        public ContainerObras()
        {
            InitializeComponent();

            usuarioService = App.UsuarioService;

            CarregarObras();
        }

        private void CarregarObras()
        {
            var usuario = usuarioService.UsuarioLogado;

            if (usuario == null)
            {
                return;
            }

            painelObras.Children.Clear();

            painelObras.Children.Add(CriarBotaoAdicionar());

            foreach (var obra in usuario.Obras)
            {
                painelObras.Children.Add(CriarCardObra(obra));
            }
        }

        private Border CriarBotaoAdicionar()
        {
            StackPanel stack = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            stack.Children.Add(new TextBlock
            {
                Text = "+",
                FontSize = 42,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                HorizontalAlignment = HorizontalAlignment.Center
            });

            stack.Children.Add(new TextBlock
            {
                Text = "Adicionar",
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                HorizontalAlignment = HorizontalAlignment.Center
            });

            Border border = new Border
            {
                Width = 140,
                Height = 190,
                Margin = new Thickness(8),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF7E1")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C9B27D")),
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(16),
                Child = stack,
                Cursor = Cursors.Hand
            };

            border.MouseDown += AdicionarObra_Click;

            return border;
        }

        private Border CriarCardObra(Obra obra)
        {
            StackPanel container = new StackPanel();

            Border imagemBox = new Border
            {
                Height = 140,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E1CF")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BDAE84")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(12),
                Margin = new Thickness(8, 8, 8, 5)
            };

            Image img = new Image
            {
                Stretch = Stretch.Uniform,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            try
            {
                img.Source = new BitmapImage(new Uri(obra.Capa, UriKind.RelativeOrAbsolute));
            }
            catch { }

            imagemBox.Child = img;

            TextBlock titulo = new TextBlock
            {
                Text = obra.Titulo,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 13,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                Margin = new Thickness(8, 4, 8, 8)
            };

            container.Children.Add(imagemBox);
            container.Children.Add(titulo);

            Border border = new Border
            {
                Width = 150,
                Height = 210,
                Margin = new Thickness(8),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF7E1")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C9B27D")),
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(16),
                Child = container,
                Cursor = Cursors.Hand
            };

            border.MouseDown += (s, e) =>
            {
                AbrirDetalhesObra(obra);
            };

            return border;
        }

        private void AdicionarObra_Click(
            object sender,
            MouseButtonEventArgs e)
        {
            var tela = new TelaAdicionarObra();

            tela.ShowDialog();

            CarregarObras();
        }

        private void AbrirDetalhesObra(Obra obra)
        {
            var tela = new ObrasPopUp(obra);

            tela.ShowDialog();

            CarregarObras();
        }
    }
}