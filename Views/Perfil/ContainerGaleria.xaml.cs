using ProjetoAcelera.Services;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjetoAcelera.Views.Perfil
{
    public partial class ContainerGaleria : Page
    {
        private PublicacaoService publicacaoService;

        public ContainerGaleria()
        {
            InitializeComponent();

            publicacaoService = new PublicacaoService();

            CarregarGaleria();
        }

        private void CarregarGaleria()
        {
            painelGaleria.Children.Clear();

            var publicacoes = publicacaoService
                            .ObterPublicacoesPerfil()
                            .Where(p =>
                                p.Status == "Aprovado" &&
                                !string.IsNullOrWhiteSpace(p.ImagemUrl) &&
                                File.Exists(p.ImagemUrl))
                            .ToList();

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
                    imagem.Source = new BitmapImage(
                        new Uri(pub.ImagemUrl, UriKind.RelativeOrAbsolute));
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
        }
    }
}