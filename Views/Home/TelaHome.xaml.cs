using Microsoft.Win32;
using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using ProjetoAcelera.Views.Admin;
using ProjetoAcelera.Views.Artistas;
using ProjetoAcelera.Views.Calendario;
using ProjetoAcelera.Views.Perfil;
using ProjetoAcelera.Views.Teste;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjetoAcelera.Views.Home
{
    public partial class TelaHome : Page
    {
        // Índice do evento atual no carrossel
        private int indiceAtual = 0;
        private string? caminhoImagemPostagem;
        private string? caminhoVideoPostagem;

        private EventoService eventoService;
        private PublicacaoService publicacaoService;
        private List<Evento> listaEventos = new List<Evento>();

        public TelaHome()
        {
            InitializeComponent();
           

            eventoService = new EventoService();
            publicacaoService = new PublicacaoService();
            listaEventos = eventoService.ObterEvento();

            if (listaEventos == null || listaEventos.Count == 0)
            {
                MessageBox.Show("Nenhum evento encontrado.",
                                "Aviso",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }
            MostrarEvento();
            InicializarPostagem();
            CarregarFeedPublicacoes();
        }

        private void InicializarPostagem()
        {
            var usuario = App.UsuarioService.UsuarioLogado;

            txtStatusPostagem.Text = string.Empty;
            txtAnexoStatus.Text = string.Empty;
            caminhoImagemPostagem = null;
            caminhoVideoPostagem = null;

            if (usuario != null)
            {
                txtUsuarioPostagem.Text = $"Olá, {usuario.Nome}";
            }
            else
            {
                txtUsuarioPostagem.Text = "Faça login para publicar uma postagem.";
            }
        }

        private void MostrarEvento()
        {
            var evento = listaEventos[indiceAtual];

            txtTitulo.Text = evento.Titulo;
            txtData.Text = evento.Data;
            txtDescricao.Text = evento.Descricao;
            txtDetalhes.Text = evento.Detalhes;

            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(evento.Imagem, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                imgEvento.Source = bitmap;
            }
            catch
            {
                try
                {
                    imgEvento.Source = new BitmapImage(new Uri("/ImagemAcelera/evento1.png", UriKind.Relative));
                }
                catch { }
            }
        }

        private void AdicionarFoto_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Imagens|*.jpg;*.jpeg;*.png;*.gif"
            };

            if (dialog.ShowDialog() == true)
            {
                caminhoImagemPostagem = dialog.FileName;
                caminhoVideoPostagem = null;
                txtAnexoStatus.Text = $"Imagem selecionada: {Path.GetFileName(caminhoImagemPostagem)}";
                txtStatusPostagem.Text = string.Empty;
            }
        }

        private void AdicionarVideo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Vídeos|*.mp4;*.mov;*.avi;*.wmv"
            };

            if (dialog.ShowDialog() == true)
            {
                caminhoVideoPostagem = dialog.FileName;
                caminhoImagemPostagem = null;
                txtAnexoStatus.Text = $"Vídeo selecionado: {Path.GetFileName(caminhoVideoPostagem)}";
                txtStatusPostagem.Text = string.Empty;
            }
        }

        private void Postar_Click(object sender, RoutedEventArgs e)
        {
            var usuario = App.UsuarioService.UsuarioLogado;

            if (usuario == null)
            {
                MessageBox.Show(
                    "Faça login para publicar.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(txtPostTexto.Text))
            {
                MessageBox.Show(
                    "Digite algo antes de publicar.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            publicacaoService.AdicionarPublicacao(txtPostTexto.Text.Trim(),caminhoImagemPostagem);
            txtPostTexto.Clear();
            txtAnexoStatus.Text = "";
            txtStatusPostagem.Text = "Sua publicação está aguardando aprovação.";
            caminhoImagemPostagem = null;
            caminhoVideoPostagem = null;

            CarregarFeedPublicacoes();
        }
        private void CarregarFeedPublicacoes()
        {
            painelFeedPublicacoes.Children.Clear();

            var publicacoes = publicacaoService.ObterFeedGlobal();

            foreach (var pub in publicacoes)
            {
                Border card = new Border
                {
                    Background = Brushes.White,
                    CornerRadius = new CornerRadius(14),
                    Margin = new Thickness(0, 0, 0, 20),
                    Padding = new Thickness(15),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D6C28F")),
                    BorderThickness = new Thickness(1)
                };
                StackPanel stack = new StackPanel();
                TextBlock autor = new TextBlock
                {
                    Text = pub.NomeAutor,
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F"))
                };

                TextBlock data = new TextBlock
                {
                    Text = pub.DataPublicacao.ToString("dd/MM/yyyy HH:mm"),
                    FontSize = 11,
                    Foreground = Brushes.Gray,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                TextBlock conteudo = new TextBlock
                {
                    Text = pub.Conteudo,
                    FontSize = 14,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = Brushes.Black,
                    Margin = new Thickness(0, 0, 0, 12)
                };
                stack.Children.Add(autor);
                stack.Children.Add(data);
                stack.Children.Add(conteudo);

                if (!string.IsNullOrWhiteSpace(pub.ImagemUrl)&& File.Exists(pub.ImagemUrl))
                {
                    Border borderImagem = new Border
                    {
                        CornerRadius = new CornerRadius(10),
                        ClipToBounds = true,
                        Margin = new Thickness(0, 0, 0, 10),
                        Background = Brushes.Black
                    };
                    Image imagem = new Image
                    {
                        Stretch = Stretch.Uniform,
                        MaxHeight = 500,
                        Cursor = Cursors.Hand
                    };
                    try
                    {
                        imagem.Source = new BitmapImage(new Uri(pub.ImagemUrl,UriKind.RelativeOrAbsolute));

                        imagem.MouseDown += (s, e) =>
                        {
                            var janela = new JanelaImagemFull(pub.ImagemUrl);
                            janela.ShowDialog();
                        };

                        borderImagem.Child = imagem;
                        stack.Children.Add(borderImagem);
                    }
                    catch
                    {

                    }
                }
                Grid areaCurtidas = new Grid
                {
                    Margin = new Thickness(0, 4, 0, 0)
                };

                areaCurtidas.ColumnDefinitions.Add(new ColumnDefinition());
                areaCurtidas.ColumnDefinitions.Add(new ColumnDefinition{Width = GridLength.Auto});

                TextBlock txtCurtidas = new TextBlock
                {
                    Text = $"❤️ {pub.Curtidas} curtidas",
                    FontWeight = FontWeights.Bold,
                    FontSize = 13,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                    VerticalAlignment = VerticalAlignment.Center
                };

                Grid.SetColumn(txtCurtidas, 0);
                bool usuarioCurtiu = publicacaoService.UsuarioCurtiu(pub);
                Button btnCurtir = new Button
                {
                    Content = usuarioCurtiu? "Gostei" : "Curtir",
                    Width = 95,
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Background = usuarioCurtiu ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B8860B")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    BorderThickness = new Thickness(0),
                    Cursor = Cursors.Hand
                };

                btnCurtir.Click += (s, e) =>{publicacaoService.AlternarCurtida(pub.Id); CarregarFeedPublicacoes();};

                Grid.SetColumn(btnCurtir, 1);
                areaCurtidas.Children.Add(txtCurtidas);
                areaCurtidas.Children.Add(btnCurtir);
                stack.Children.Add(areaCurtidas);
                card.Child = stack;
                painelFeedPublicacoes.Children.Add(card);
            }
        } 

        private void BtnProximo_Click(object sender, RoutedEventArgs e)
        {
            indiceAtual++;

            if (indiceAtual >= listaEventos.Count)
                indiceAtual = 0;

            MostrarEvento();
        }

        private void BtnAnterior_Click(object sender, RoutedEventArgs e)
        {
            indiceAtual--;
            if (indiceAtual < 0)
                indiceAtual = listaEventos.Count - 1;

            MostrarEvento();
        }


        private void Programacao_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Views.Calendario.Calendario());
        }

        private void Artistas_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Views.Artistas.TelaArtista());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NossaCidade_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Views.Teste.Dashboard());
        }
    }
}
