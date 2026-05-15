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
            InicializarPostagem();
            CarregarArtistasDestaque();
            CarregarFeedPublicacoes();
        }

        private void CarregarArtistasDestaque()
        {
            try
            {
                wrapArtistasDestaque.Children.Clear();

                var usuarios = App.UsuarioService.ObterTodos()
                    .Where(u => !u.Banido)
                    .Take(4);

                foreach (var u in usuarios)
                {
                    Border b = new Border
                    {
                        Width = 70,
                        Height = 70,
                        CornerRadius = new CornerRadius(0),
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B8860B")),
                        BorderThickness = new Thickness(2),
                        Margin = new Thickness(10,0,10,0),
                        ClipToBounds = true
                    };

                    Image img = new Image { Stretch = Stretch.UniformToFill };
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(u?.Perfil?.FotoPerfil) && File.Exists(u.Perfil.FotoPerfil))
                            img.Source = new BitmapImage(new Uri(u.Perfil.FotoPerfil, UriKind.Absolute));
                        else
                            img.Source = new BitmapImage(new Uri("/ImagemAcelera/AvatarPadrao.png", UriKind.Relative));
                    }
                    catch
                    {
                        try { img.Source = new BitmapImage(new Uri("/ImagemAcelera/AvatarPadrao.png", UriKind.Relative)); } catch { }
                    }

                    b.Child = img;
                    wrapArtistasDestaque.Children.Add(b);
                }

                // composer avatar
                var usuario = App.UsuarioService.UsuarioLogado;
                if (usuario != null && !string.IsNullOrWhiteSpace(usuario.Perfil?.FotoPerfil) && File.Exists(usuario.Perfil.FotoPerfil))
                {
                    try { imgUsuarioPost.Source = new BitmapImage(new Uri(usuario.Perfil.FotoPerfil, UriKind.Absolute)); }
                    catch { }
                }
            }
            catch { }
        }

        private void InicializarPostagem()
        {
            var usuario = App.UsuarioService.UsuarioLogado;

            txtStatusPostagem.Text = string.Empty;
            txtAnexoStatus.Text = string.Empty;
            caminhoImagemPostagem = null;
            caminhoVideoPostagem = null;
            chkPermiteComentarios.IsChecked = true;

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
            publicacaoService.AdicionarPublicacao(
                txtPostTexto.Text.Trim(),
                caminhoImagemPostagem,
                caminhoVideoPostagem,
                chkPermiteComentarios.IsChecked == true);


            txtPostTexto.Clear();
            txtAnexoStatus.Text = "";
            txtStatusPostagem.Text = "Sua publicação está aguardando aprovação.";
            caminhoImagemPostagem = null;
            caminhoVideoPostagem = null;
            chkPermiteComentarios.IsChecked = true;

            CarregarFeedPublicacoes();
        }
        private string ObterFotoAutor(string emailAutor)
        {
            var usuario = App.UsuarioService.ObterTodos().FirstOrDefault(u => u.Email == emailAutor);

            return usuario?.Perfil?.FotoPerfil ?? "";
        }
        private void CarregarFeedPublicacoes()
        {
            painelFeedPublicacoes.Children.Clear();

            var publicacoes = publicacaoService.ObterFeedGlobal();

            foreach (var pub in publicacoes)
            {
                Border card = new Border
                {
                    MinWidth = 1190,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F3E6C9")),
                    CornerRadius = new CornerRadius(14),
                    Margin = new Thickness(0, 0, 0, 20),
                    Padding = new Thickness(15),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D6C28F")),
                    BorderThickness = new Thickness(1)
                };
                StackPanel stack = new StackPanel();
                StackPanel topoPost = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                
                Border avatarBorder = new Border
                {
                    Width = 45,
                    Height = 45,
                    CornerRadius = new CornerRadius(22.5),
                    ClipToBounds = true,
                    Background = Brushes.LightGray,
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                    BorderThickness = new Thickness(1)
                };

                Image avatar = new Image
                {
                    Stretch = Stretch.UniformToFill
                };

                try
                {
                    var usuarioAutor = App.UsuarioService.ObterTodos().FirstOrDefault(u => u.Email == pub.EmailAutor);

                    string fotoAutor =usuarioAutor?.Perfil?.FotoPerfil ?? "";

                    if (!string.IsNullOrWhiteSpace(fotoAutor) && File.Exists(fotoAutor))
                    {
                        avatar.Source = new BitmapImage(new Uri(fotoAutor, UriKind.Absolute));
                    }
                    else
                    {
                        avatar.Source = new BitmapImage(
                            new Uri("/ImagemAcelera/AvatarPadrao.png", UriKind.Relative));
                    }
                }
                catch
                {

                }

                avatarBorder.Child = avatar;

                StackPanel infoAutor = new StackPanel
                {
                    Margin = new Thickness(10, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center
                };

                TextBlock autor = new TextBlock
                {
                    Text = pub.NomeAutor,
                    FontSize = 15,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F"))
                };

                TextBlock data = new TextBlock
                {
                    Text = pub.DataPublicacao.ToString("dd/MM/yyyy HH:mm"),
                    FontSize = 11,
                    Foreground = Brushes.Gray
                };

                infoAutor.Children.Add(autor);
                infoAutor.Children.Add(data);

                topoPost.Children.Add(avatarBorder);
                topoPost.Children.Add(infoAutor);
                stack.Children.Add(topoPost);
                Border linhaAutor = new Border
                {
                    Height = 2,

                    Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#1F3A5F")),

                    Margin = new Thickness(0, 8, 0, 10)
                };

                stack.Children.Add(linhaAutor);

                if (!string.IsNullOrWhiteSpace(pub.ImagemUrl) && File.Exists(pub.ImagemUrl))
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
                        imagem.Source = new BitmapImage(new Uri(pub.ImagemUrl, UriKind.RelativeOrAbsolute));
                        imagem.MouseDown += (s, e) =>
                        {
                            var janela = new JanelaImagemFull(pub.ImagemUrl);
                            janela.ShowDialog();
                        };

                        borderImagem.Child = imagem;
                        stack.Children.Add(borderImagem);
                        Border linhaSeparadora = new Border
                        {
                            Height = 2,
                            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                            Margin = new Thickness(0, 8, 0, 10)
                        };

                        stack.Children.Add(linhaSeparadora);
                    }
                    catch
                    {

                    }
                }

                if (!string.IsNullOrWhiteSpace(pub.CaminhoVideo) && File.Exists(pub.CaminhoVideo))
                {
                    Border borderVideo = new Border
                    {
                        CornerRadius = new CornerRadius(10),
                        Background = Brushes.Black,
                        Padding = new Thickness(12),
                        Margin = new Thickness(0, 0, 0, 10)
                    };

                    StackPanel painelVideo = new StackPanel();
                    TextBlock labelVideo = new TextBlock
                    {
                        Text = "Vídeo anexado",
                        FontSize = 14,
                        FontWeight = FontWeights.Bold,
                        Foreground = Brushes.White,
                        Margin = new Thickness(0, 0, 0, 6)
                    };

                    Button btnAbrirVideo = new Button
                    {
                        Content = "Abrir vídeo",
                        Width = 120,
                        Height = 30,
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B8860B")),
                        Foreground = Brushes.White,
                        BorderThickness = new Thickness(0),
                        Cursor = Cursors.Hand
                    };

                    btnAbrirVideo.Click += (s, e) =>
                    {
                        try
                        {
                            Process.Start(new ProcessStartInfo(pub.CaminhoVideo) { UseShellExecute = true });
                        }
                        catch
                        {
                        }
                    };

                    painelVideo.Children.Add(labelVideo);
                    painelVideo.Children.Add(btnAbrirVideo);
                    borderVideo.Child = painelVideo;
                    stack.Children.Add(borderVideo);
                }

                if (!string.IsNullOrWhiteSpace(pub.Conteudo))
                {
                    TextBlock conteudo = new TextBlock
                    {
                        Text = pub.Conteudo,
                        FontSize = 14,
                        FontWeight = FontWeights.Bold,
                        TextWrapping = TextWrapping.Wrap,
                        Foreground = Brushes.Black,
                        Margin = new Thickness(0, 5, 0, 12)
                    };

                    stack.Children.Add(conteudo);
                }

                Border linhaTexto = new Border
                {
                    Height = 2,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                    Margin = new Thickness(0, 0, 0, 10)
                };

                stack.Children.Add(linhaTexto);

                StackPanel areaStats = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 4, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center
                };

                // Botão de curtir com coração
                Button btnCurtir = new Button
                {
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0),
                    Cursor = Cursors.Hand,
                    Padding = new Thickness(4),
                };

                bool usuarioCurtiu = publicacaoService.UsuarioCurtiu(pub);
                string coracao = usuarioCurtiu ? "♥" : "♡";
                TextBlock txtCurtir = new TextBlock
                {
                    Text = $"{coracao} {pub.Curtidas}",
                    FontWeight = FontWeights.Bold,
                    FontSize = 13,
                    Foreground = usuarioCurtiu ? Brushes.Red : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                    VerticalAlignment = VerticalAlignment.Center
                };

                btnCurtir.Content = txtCurtir;
                btnCurtir.Click += (s, e) =>
                {
                    publicacaoService.AlternarCurtida(pub.Id);
                    CarregarFeedPublicacoes();
                };

                TextBlock txtComentarios = new TextBlock
                {
                    Text = pub.ComentariosPermitidos ? "💬 Comentários permitidos" : "🚫 Comentários desativados",
                    FontWeight = FontWeights.Bold,
                    FontSize = 13,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(24, 0, 0, 0)
                };

                areaStats.Children.Add(btnCurtir);
                areaStats.Children.Add(txtComentarios);
                stack.Children.Add(areaStats);


                foreach (var comentario in pub.Comentarios.Where(c => c.Status == "Aprovado"))
                {
                    Border comentarioBorder = new Border
                    {
                        Background = new SolidColorBrush(
                            (Color)ColorConverter.ConvertFromString("#EFE4C8")),

                        CornerRadius = new CornerRadius(8),
                        Padding = new Thickness(10),
                        Margin = new Thickness(0, 8, 0, 0)
                    };

                    StackPanel comentarioStack = new StackPanel();

                    TextBlock nomeComentario = new TextBlock
                    {
                        Text = comentario.NomeAutor,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(
                            (Color)ColorConverter.ConvertFromString("#1F3A5F"))
                    };

                    TextBlock textoComentario = new TextBlock
                    {
                        Text = comentario.Conteudo,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 3, 0, 0),
                        Foreground = Brushes.Black
                    };

                    comentarioStack.Children.Add(nomeComentario);
                    comentarioStack.Children.Add(textoComentario);

                    comentarioBorder.Child = comentarioStack;
                    stack.Children.Add(comentarioBorder);
                }

                if (pub.ComentariosPermitidos)
                {
                    StackPanel areaComentario = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(0, 10, 0, 0)
                    };

                    TextBox txtComentario = new TextBox
                    {
                        Width = 350,
                        Height = 32,
                        FontSize = 13,
                        Padding = new Thickness(8)
                    };

                    Button btnComentar = new Button
                    {
                        Content = "Comentar",
                        Width = 100,
                        Height = 32,
                        Margin = new Thickness(10, 0, 0, 0),
                        Background = new SolidColorBrush(
                            (Color)ColorConverter.ConvertFromString("#1F3A5F")),
                        Foreground = Brushes.White,
                        BorderThickness = new Thickness(0),
                        Cursor = Cursors.Hand
                    };

                    btnComentar.Click += (s, e) =>
                    {
                        var usuarioLogado = App.UsuarioService.UsuarioLogado;

                        if (usuarioLogado == null)
                        {
                            MessageBox.Show("Faça login para comentar.");
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(txtComentario.Text))
                        {
                            return;
                        }

                        publicacaoService.AdicionarComentario(
                            pub.Id,
                            usuarioLogado.Nome,
                            usuarioLogado.Email,
                            txtComentario.Text.Trim()
                        );
                        MessageBox.Show(
                                        "Comentário enviado para análise do autor da publicação.",
                                        "Comentário enviado",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information
);

                        txtComentario.Clear();

                        CarregarFeedPublicacoes();
                    };

                    areaComentario.Children.Add(txtComentario);
                    areaComentario.Children.Add(btnComentar);
                    stack.Children.Add(areaComentario);
                }

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
