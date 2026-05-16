using Microsoft.Win32;
using ProjetoAcelera.Ferramentas;
using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using ProjetoAcelera.Views.Admin;
using ProjetoAcelera.Views.Artistas;
using ProjetoAcelera.Views.Calendario;
using ProjetoAcelera.Views.Perfil;
using ProjetoAcelera.Views.Teste;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            CarregarFeedPublicacoes();
            CarregarArtistasRecentes();
            CarregarArtistasDestaque();
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


        //Carregar os artistas que mais postaram recentemente
        private void CarregarArtistasRecentes()
        {
            painelArtistasRecentes.Children.Clear();

            var artistas = App.UsuarioService
                .ObterTodos().Where(u => u.Publicacoes != null && u.Publicacoes.Count > 0 && !u.Banido).OrderByDescending(u => u.Publicacoes.Max(p => p.DataPublicacao)).Take(6).ToList(); //Olha, esse take X ai pega o numero de artistas para aparecer la, botei só 6 pra evitar coisa

            foreach (var artista in artistas)
            {
                var ultimoPost = artista.Publicacoes.OrderByDescending(p => p.DataPublicacao).FirstOrDefault();

                if (ultimoPost == null)
                    continue;

                Border card = new Border
                {
                    Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#F3E6C9")),

                    CornerRadius = new CornerRadius(0),
                    Margin = new Thickness(0, 0, 0, 12),
                    Padding = new Thickness(10),
                    Cursor = Cursors.Hand
                };

                StackPanel stack = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };

                Border avatarBorder = new Border
                {
                    Width = 55,
                    Height = 55,
                    CornerRadius = new CornerRadius(0),
                    ClipToBounds = true,
                    Background = Brushes.LightGray,
                    Margin = new Thickness(0, 0, 10, 0)
                };

                Image avatar = new Image
                {
                    Stretch = Stretch.UniformToFill
                };

                try
                {
                    if (!string.IsNullOrWhiteSpace(artista.Perfil?.FotoPerfil) && File.Exists(artista.Perfil.FotoPerfil))
                    {
                        avatar.Source = new BitmapImage(new Uri(artista.Perfil.FotoPerfil));
                    }
                    else
                    {
                        avatar.Source = new BitmapImage(new Uri("/ImagemAcelera/AvatarPadrao.png",UriKind.Relative));
                    }
                }
                catch
                {

                }

                avatarBorder.Child = avatar;

                StackPanel info = new StackPanel
                {
                    Width = 180
                };

                TextBlock nome = new TextBlock
                {
                    Text = artista.Nome,
                    FontWeight = FontWeights.Bold,
                    FontSize = 14,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F"))
                };

                string textoPost = ultimoPost.Conteudo;

                if (textoPost.Length > 55)
                {
                    textoPost = textoPost.Substring(0, 55) + "..."; //limitar o texto pra ficar bonitinho
                }

                TextBlock post = new TextBlock
                {
                    Text = textoPost,
                    FontSize = 12,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = Brushes.Black
                };

                info.Children.Add(nome);
                info.Children.Add(post);
                stack.Children.Add(avatarBorder);
                stack.Children.Add(info);

                card.Child = stack;

                card.MouseDown += (s, e) =>
                {
                    NavigationService.Navigate(new TelaPerfilVisual(artista));
                };

                painelArtistasRecentes.Children.Add(card);
            }
        }

        private void CarregarArtistasDestaque()
        {
            painelArtistasDestaque.Children.Clear();

            var artistas = App.UsuarioService
    .ObterTodos()
    .Where(u => u.Perfil != null && u.Perfil.Destaque && !u.Banido).Take(4).ToList();

            foreach (var artista in artistas)
            {
                Border avatarBorder = new Border
                {
                    Width = 90,
                    Height = 90,
                    CornerRadius = new CornerRadius(0),
                    Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#1F3A5F")),

                    BorderBrush = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#B8860B")),

                    BorderThickness = new Thickness(2),
                    Margin = new Thickness(10, 0, 10, 0),
                    ClipToBounds = true,
                    Cursor = Cursors.Hand
                };

                Image avatar = new Image
                {
                    Stretch = Stretch.UniformToFill
                };

                try
                {
                    if (!string.IsNullOrWhiteSpace(artista.Perfil?.FotoPerfil) && File.Exists(artista.Perfil.FotoPerfil))
                    {
                        avatar.Source = new BitmapImage(new Uri(artista.Perfil.FotoPerfil));
                    }
                    else
                    {
                        avatar.Source = new BitmapImage(new Uri("/ImagemAcelera/AvatarPadrao.png",UriKind.Relative));
                    }
                }
                catch
                {

                }

                avatarBorder.Child = avatar;

                avatarBorder.MouseDown += (s, e) =>
                {
                    NavigationService.Navigate(new TelaPerfilVisual(artista));
                };

                painelArtistasDestaque.Children.Add(avatarBorder);
            }
        }

        private void CarregarFeedPublicacoes()
        {
            painelFeedPublicacoes.Children.Clear();
            var publicacoes = publicacaoService.ObterFeedGlobal();

            foreach (var pub in publicacoes)
            {
                Border card = PublicacaoComponentesVisual.CriarCardPublicacao();
                card.MinWidth = 1190;
                card.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F3E6C9"));
                card.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D6C28F"));
                card.BorderThickness = new Thickness(1);
                card.CornerRadius = new CornerRadius(14);
                StackPanel stack = new StackPanel();

                // TOPO: autor + data
                StackPanel topoPost = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 0, 0, 10)
                };

                Border avatarBorder = PublicacaoComponentesVisual.CriarMolduraAvatar();
                Image avatar = new Image
                {
                    Stretch = Stretch.UniformToFill
                };
                try
                {
                    var usuarioAutor = App.UsuarioService.ObterTodos().FirstOrDefault(u => u.Email == pub.EmailAutor);
                    string fotoAutor = usuarioAutor?.Perfil?.FotoPerfil ?? "";

                    if (!string.IsNullOrWhiteSpace(fotoAutor) && File.Exists(fotoAutor))
                    {
                        avatar.Source = new BitmapImage(new Uri(fotoAutor, UriKind.Absolute));
                    }
                    else
                    {
                        avatar.Source = new BitmapImage( new Uri("/ImagemAcelera/AvatarPadrao.png", UriKind.Relative));
                    }
                }
                catch
                {
                    //vazio
                }
                avatarBorder.Child = avatar;

                StackPanel infoAutor = new StackPanel
                {
                    Margin = new Thickness(10, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center
                };

                TextBlock autor = PublicacaoComponentesVisual.CriarTextoAutor(pub.NomeAutor);
                TextBlock data = PublicacaoComponentesVisual.CriarTextoData(pub.DataPublicacao.ToString("dd/MM/yyyy HH:mm"));
                infoAutor.Children.Add(autor);
                infoAutor.Children.Add(data);
                topoPost.Children.Add(avatarBorder);
                topoPost.Children.Add(infoAutor);
                stack.Children.Add(topoPost);
                stack.Children.Add(
                    PublicacaoComponentesVisual.CriarLinhaAzul(new Thickness(-15, 8, -15, 10)));

                // IMAGEM
                if (!string.IsNullOrWhiteSpace(pub.ImagemUrl) && File.Exists(pub.ImagemUrl))
                {
                    Border bordaImagem = PublicacaoComponentesVisual.CriarMolduraImagem();
                    Image imagem = new Image
                    {
                        Stretch = Stretch.Uniform,
                        MaxHeight = 500,
                        Cursor = Cursors.Hand,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    try
                    {
                        imagem.Source = new BitmapImage(new Uri(pub.ImagemUrl, UriKind.RelativeOrAbsolute));
                        imagem.MouseDown += (s, e) =>
                        {
                            var janela = new JanelaImagemFull(pub.ImagemUrl);
                            janela.ShowDialog();
                        };

                        bordaImagem.Child = imagem;
                        stack.Children.Add(bordaImagem);
                        stack.Children.Add(PublicacaoComponentesVisual.CriarLinhaAzul(new Thickness(-15, 8, -15, 10)));
                    }
                    catch
                    {
                        //vazio
                    }
                }
                
                // TEXTO
                if (!string.IsNullOrWhiteSpace(pub.Conteudo))
                {
                    stack.Children.Add(PublicacaoComponentesVisual.CriarTextoConteudo(pub.Conteudo));
                }

                stack.Children.Add(PublicacaoComponentesVisual.CriarLinhaAzul(new Thickness(-15, 0, -15, 10)));

                // CURTIDAS / COMENTÁRIOS / CURTIR
                Grid areaStats = PublicacaoComponentesVisual.CriarAreaStats();
                StackPanel statsEsquerda = PublicacaoComponentesVisual.CriarStatsEsquerda();
                TextBlock curtidas = PublicacaoComponentesVisual.CriarTextoCurtidas(pub.Curtidas);
                statsEsquerda.Children.Add(curtidas);
                Grid.SetColumn(statsEsquerda, 0);
                areaStats.Children.Add(statsEsquerda);
                bool usuarioCurtiu = publicacaoService.UsuarioCurtiu(pub);

                Button btnCurtir = new Button
                {
                    Content = usuarioCurtiu ? "Curtido ❤️" : "Curtir",
                    Width = 110,
                    Height = 32,
                    Background = usuarioCurtiu? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B8860B")): new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    BorderThickness = new Thickness(0),
                    Cursor = Cursors.Hand,
                    VerticalAlignment = VerticalAlignment.Center
                };

                btnCurtir.Click += (s, e) =>
                {
                    var usuarioLogado = App.UsuarioService.UsuarioLogado;

                    if (usuarioLogado == null)
                    {
                        MessageBox.Show(
                            "Faça login para curtir esta publicação.",
                            "Login necessário",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );

                        return;
                    }
                    publicacaoService.AlternarCurtida(pub.Id);
                    CarregarFeedPublicacoes();
                };

                Grid.SetColumn(btnCurtir, 1);
                areaStats.Children.Add(btnCurtir);
                stack.Children.Add(areaStats);

                // ÁREA DE COMENTÁRIOS
                PublicacaoComponentesVisual.CriarAreaComentarios( pub, statsEsquerda, stack, false, false, id => { }, id => { });

                // CAMPO PARA COMENTAR
                StackPanel campoComentario = PublicacaoComponentesVisual.CriarCampoComentario( pub, textoComentario =>
                    {
                        var usuarioLogado = App.UsuarioService.UsuarioLogado;
                        if (usuarioLogado == null)
                        {
                            MessageBox.Show("Faça login para comentar.");
                            return;
                        }
                        publicacaoService.AdicionarComentario(pub.Id, usuarioLogado.Nome, usuarioLogado.Email, textoComentario);
                        MessageBox.Show(
                            "Comentário enviado para análise do autor da publicação.",
                            "Comentário enviado",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                        );
                        CarregarFeedPublicacoes();
                    }
                );
                stack.Children.Add(campoComentario);
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
