using ProjetoAcelera.Ferramentas;
using ProjetoAcelera.Services;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace ProjetoAcelera.Views.Perfil
{
    public partial class ContainerGaleria : Page
    {
        private PublicacaoService publicacaoService;
        private int quantidadeImagensExibidas = 12;
        private const int quantidadeCarregarMais = 12;
        public ContainerGaleria()
        {
            InitializeComponent();

            publicacaoService = new PublicacaoService();

            CarregarGaleria();
        }

        private void CarregarGaleria()
        {
            painelGaleria.Children.Clear();

            var todasPublicacoes = publicacaoService.ObterPublicacoesPerfil().Where(p =>!string.IsNullOrWhiteSpace(p.ImagemUrl) &&
                    File.Exists(p.ImagemUrl)).OrderByDescending(p => p.DataPublicacao).ToList();

            var publicacoes = todasPublicacoes.Take(quantidadeImagensExibidas).ToList();

            foreach (var pub in publicacoes)
            {
                Border card = new Border
                {
                    Width = 220,
                    Height = 220,
                    Margin = new Thickness(8),
                    CornerRadius = new CornerRadius(12),
                    ClipToBounds = true,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E1CF")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C9B27D")),
                    BorderThickness = new Thickness(2),
                    Cursor = Cursors.Hand
                };

                Image imagem = new Image
                {
                    Stretch = Stretch.UniformToFill,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                try
                {
                    imagem.Source = AuxilioImagens.CarregarImgOtimizada(pub.ImagemUrl, 400);
                }
                catch
                {
                    continue;
                }

                card.Child = imagem;

                card.MouseDown += (s, e) =>
                {
                    var janela = new JanelaImagemFull(pub.ImagemUrl);
                    janela.ShowDialog();
                };

                painelGaleria.Children.Add(card);
            }

            btnCarregarMais.Visibility = quantidadeImagensExibidas < todasPublicacoes.Count ? Visibility.Visible: Visibility.Collapsed;

            }

        private void BtnCarregarMais_Click(object sender, RoutedEventArgs e)
        {
            quantidadeImagensExibidas += quantidadeCarregarMais;
            CarregarGaleria();
        }
    }
}
 