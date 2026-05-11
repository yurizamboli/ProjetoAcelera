using Microsoft.Win32;
using ProjetoAcelera.Services;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjetoAcelera.Views.Perfil
{
    public partial class ContainerPublicacoes : Page
    {
        private PublicacaoService publicacaoService;
        private string caminhoImagemSelecionada = "";

        public ContainerPublicacoes()
        {
            InitializeComponent();

            publicacaoService = new PublicacaoService();

            CarregarPublicacoes();
        }

        private void AdicionarImagem_Button(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Imagens|*.png;*.jpg;*.jpeg";

            if (dialog.ShowDialog() == true)
            {
                caminhoImagemSelecionada = dialog.FileName;
                txtImagemSelecionada.Text = Path.GetFileName(dialog.FileName);
            }
        }

        private void CarregarPublicacoes()
        {
            painelPublicacoes.Children.Clear();

            var publicacoes = publicacaoService.ObterPublicacoesPerfil();

            foreach (var pub in publicacoes)
            {
                Border card = new Border
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF7E1")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C9B27D")),
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(16),
                    Margin = new Thickness(0, 0, 0, 18),
                    Padding = new Thickness(15)
                };

                StackPanel stack = new StackPanel();

                TextBlock autor = new TextBlock
                {
                    Text = pub.NomeAutor,
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F"))
                };

                TextBlock data = new TextBlock
                {
                    Text = pub.DataPublicacao.ToString("dd/MM/yyyy HH:mm"),
                    Foreground = Brushes.Gray,
                    FontSize = 11,
                    Margin = new Thickness(0, 2, 0, 10)
                };

                TextBlock conteudo = new TextBlock
                {
                    Text = pub.Conteudo,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 14,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2B2B2B")),
                    Margin = new Thickness(0, 5, 0, 12)
                };

                stack.Children.Add(autor);
                stack.Children.Add(data);
                stack.Children.Add(conteudo);

                if (!string.IsNullOrWhiteSpace(pub.ImagemUrl) && File.Exists(pub.ImagemUrl))
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

                    Image imagemPost = new Image
                    {
                        Stretch = Stretch.Uniform,
                        MaxHeight = 420,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };

                    try
                    {
                        imagemPost.Source = new BitmapImage(new Uri(pub.ImagemUrl, UriKind.Absolute));
                        bordaImagem.Child = imagemPost;
                        stack.Children.Add(bordaImagem);
                    }
                    catch { }
                }

                TextBlock curtidas = new TextBlock
                {
                    Text = $"❤️ {pub.Curtidas} curtidas",
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 8)
                };

                stack.Children.Add(curtidas);

                bool usuarioCurtiu = publicacaoService.UsuarioCurtiu(pub);

                StackPanel botoes = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Right
                };

                Button btnCurtir = new Button
                {
                    Content = usuarioCurtiu ? "❤️ Curtido" : "🤍 Curtir",
                    Width = 110,
                    Height = 32,
                    Margin = new Thickness(0, 0, 8, 0),
                    Background = usuarioCurtiu ? Brushes.IndianRed : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    BorderThickness = new Thickness(0)
                };

                btnCurtir.Click += (s, e) =>
                {
                    publicacaoService.AlternarCurtida(pub.Id);
                    CarregarPublicacoes();
                };

                Button btnRemover = new Button
                {
                    Content = "Remover",
                    Width = 90,
                    Height = 32,
                    Background = Brushes.DarkRed,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    BorderThickness = new Thickness(0)
                };

                btnRemover.Click += (s, e) =>
                {
                    var resultado = MessageBox.Show(
                        "Deseja remover esta publicação?",
                        "Remover publicação",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (resultado != MessageBoxResult.Yes)
                        return;

                    publicacaoService.RemoverPublicacao(pub.Id);
                    CarregarPublicacoes();
                };

                botoes.Children.Add(btnCurtir);
                botoes.Children.Add(btnRemover);

                stack.Children.Add(botoes);

                card.Child = stack;
                painelPublicacoes.Children.Add(card);
            }
        }

        private void Publicar_Button(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNovaPublicacao.Text))
            {
                return;
            }

            publicacaoService.AdicionarPublicacao(
                txtNovaPublicacao.Text,
                caminhoImagemSelecionada
            );

            txtNovaPublicacao.Clear();

            caminhoImagemSelecionada = "";
            txtImagemSelecionada.Text = "Clique para selecionar uma imagem";

            CarregarPublicacoes();
        }
    }
}