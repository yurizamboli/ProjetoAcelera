using ProjetoAcelera.Ferramentas;
using ProjetoAcelera.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using ProjetoAcelera.Views.PopUpObras;

namespace ProjetoAcelera.Views.Perfil
{
    public partial class ContainerObrasVisual : Page
    {
        private Usuario usuario;
        private int quantidadeObrasExibidas = 8;
        private const int quantidadeCarregarMais = 8;
        public ContainerObrasVisual(Usuario user)
        {
            InitializeComponent();

            usuario = user;

            CarregarObras();
        }

        private void CarregarObras()
        {
            painelObras.Children.Clear();

            if (usuario?.Obras == null)
            {
                btnCarregarMais.Visibility = Visibility.Collapsed;
                return; 
            }
            var todasObras =usuario.Obras.OrderByDescending(o => o.Favorito).ToList();
            var obrasExibidas = todasObras.Take(quantidadeObrasExibidas).ToList();
            foreach (var obra in obrasExibidas)
            {
                painelObras.Children.Add(CriarCard(obra));
            }
            btnCarregarMais.Visibility = quantidadeObrasExibidas < todasObras.Count ? Visibility.Visible : Visibility.Collapsed;
        }

        private Border CriarCard(Obra obra)
        {
            

            StackPanel container = new StackPanel();

            Grid gridImagem = new Grid();

            Border imagemBox = new Border
            {
                Height = 150,
                Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#E8E1CF")),

                BorderBrush = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#BDAE84")),

                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(12),
                Margin = new Thickness(8, 8, 8, 5),
                ClipToBounds = true
            };

            Image img = new Image
            {
                Stretch = Stretch.Uniform,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            try
            {
                if (!string.IsNullOrWhiteSpace(obra.Capa))
                {
                    img.Source =
                        AuxilioImagens.CarregarImgOtimizada(
                            obra.Capa,
                            300);
                }
            }
            catch
            {
                // vazio
            }

            gridImagem.Children.Add(img);

            // favorito
            if (obra.Favorito)
            {
                Border estrela = new Border
                {
                    Width = 32,
                    Height = 32,
                    CornerRadius = new CornerRadius(16),
                    Background = new SolidColorBrush(
                        Color.FromArgb(170, 0, 0, 0)),

                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(8)
                };

                TextBlock txtEstrela = new TextBlock
                {
                    Text = "★",
                    FontSize = 18,
                    Foreground = Brushes.Gold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Center
                };

                estrela.Child = txtEstrela;

                gridImagem.Children.Add(estrela);
            }

            imagemBox.Child = gridImagem;

            TextBlock titulo = new TextBlock
            {
                Text = obra.Titulo,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 13,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#1F3A5F")),

                Margin = new Thickness(8, 4, 8, 8)
            };

            container.Children.Add(imagemBox);
            container.Children.Add(titulo);

            Border border = new Border
            {
                Width = 180,
                Height = 240,
                Margin = new Thickness(10),
                CornerRadius = new CornerRadius(16),

                BorderBrush = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#C9B27D")),

                BorderThickness = new Thickness(2),

                Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#FFF7E1")),

                Child = container
            };

            border.Cursor = Cursors.Hand;

            border.MouseDown += (s, e) =>
            {
                ObrasVisualPopUp tela = new ObrasVisualPopUp(obra);

                tela.ShowDialog();
            };

            if (obra.Favorito)
            {
                Border estrela = new Border
                {
                    Width = 32,
                    Height = 32,
                    CornerRadius = new CornerRadius(16),
                    Background = new SolidColorBrush(
                        Color.FromArgb(170, 0, 0, 0)),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(8)
                };

                TextBlock txtEstrela = new TextBlock
                {
                    Text = "★",
                    FontSize = 18,
                    Foreground = Brushes.Gold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Center
                };

                estrela.Child = txtEstrela;

                gridImagem.Children.Add(estrela);
            }

            return border;
        }
        private void BtnCarregarMais_Click(object sender, RoutedEventArgs e)
        {
            quantidadeObrasExibidas += quantidadeCarregarMais;
            CarregarObras();
        }
    }
}
