using ProjetoAcelera.Ferramentas;
using ProjetoAcelera.Models;
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
    public partial class ContainerPostsVisual : Page
    {
        private Usuario usuario;
        private PublicacaoService publicacaoService;

        public ContainerPostsVisual(Usuario user)
        {
            InitializeComponent();

            usuario = user;
            publicacaoService = new PublicacaoService();

            CarregarPosts();
        }

        private void CarregarPosts()
        {
            painelPosts.Children.Clear();

            if (usuario.Publicacoes == null)
            {
                return;
            }

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
            Border card = PublicacaoComponentesVisual.CriarCardPublicacao();

            StackPanel container = new StackPanel();

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
                string fotoAutor = usuario?.Perfil?.FotoPerfil ?? "";
                if (!string.IsNullOrWhiteSpace(fotoAutor) && File.Exists(fotoAutor))
                {
                    avatar.Source = new BitmapImage(new Uri(fotoAutor, UriKind.Absolute));
                }
                else
                {
                    avatar.Source = new BitmapImage(new Uri("/ImagemAcelera/AvatarPadrao.png", UriKind.Relative));
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

            TextBlock autor = PublicacaoComponentesVisual.CriarTextoAutor(post.NomeAutor);
            TextBlock data = PublicacaoComponentesVisual.CriarTextoData(post.DataPublicacao.ToString("dd/MM/yyyy HH:mm"));

            infoAutor.Children.Add(autor);
            infoAutor.Children.Add(data);
            topoPost.Children.Add(avatarBorder);
            topoPost.Children.Add(infoAutor);
            container.Children.Add(topoPost);
            container.Children.Add(PublicacaoComponentesVisual.CriarLinhaAzul(new Thickness(-15, 8, -15, 10)));

            // IMAGEM
            if (!string.IsNullOrWhiteSpace(post.ImagemUrl) && File.Exists(post.ImagemUrl))
            {
                Border bordaImagem = PublicacaoComponentesVisual.CriarMolduraImagem();
                Image img = new Image
                {
                    MaxHeight = 420,
                    Stretch = Stretch.Uniform,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Cursor = Cursors.Hand
                };

                try
                {
                    img.Source = new BitmapImage(new Uri(post.ImagemUrl, UriKind.RelativeOrAbsolute));

                    img.MouseDown += (s, e) =>
                    {
                        JanelaImagemFull janela = new JanelaImagemFull(post.ImagemUrl);
                        janela.ShowDialog();
                    };

                    bordaImagem.Child = img;
                    container.Children.Add(bordaImagem);
                    container.Children.Add(PublicacaoComponentesVisual.CriarLinhaAzul( new Thickness(-15, 8, -15, 10)));
                }
                catch
                {
                    //vazio
                }
            }

            // TEXTO
            if (!string.IsNullOrWhiteSpace(post.Conteudo))
            {
                container.Children.Add(PublicacaoComponentesVisual.CriarTextoConteudo(post.Conteudo));
            }
            container.Children.Add(PublicacaoComponentesVisual.CriarLinhaAzul( new Thickness(-15, 0, -15, 10)));

            // CURTIDAS / COMENTÁRIOS / CURTIR
            Grid areaStats = PublicacaoComponentesVisual.CriarAreaStats();
            StackPanel statsEsquerda = PublicacaoComponentesVisual.CriarStatsEsquerda();
            TextBlock curtidas = PublicacaoComponentesVisual.CriarTextoCurtidas(post.Curtidas);
            statsEsquerda.Children.Add(curtidas);
            Grid.SetColumn(statsEsquerda, 0);
            areaStats.Children.Add(statsEsquerda);
            bool usuarioCurtiu = publicacaoService.UsuarioCurtiu(post);

            Button btnCurtir = new Button
            {
                Content = usuarioCurtiu ? "Gostei" : "Curtir",
                Width = 90,
                Height = 30,
                Background = usuarioCurtiu ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B8860B")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
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
                publicacaoService.AlternarCurtida(post.Id);
                CarregarPosts();
            };

            Grid.SetColumn(btnCurtir, 1);
            areaStats.Children.Add(btnCurtir);
            container.Children.Add(areaStats);

            // ÁREA DE COMENTÁRIOS
            PublicacaoComponentesVisual.CriarAreaComentarios(post, statsEsquerda, container, false, false, id => { }, id => { });

            // CAMPO PARA COMENTAR
            StackPanel campoComentario = PublicacaoComponentesVisual.CriarCampoComentario(post, textoComentario =>
                {
                    var usuarioLogado = App.UsuarioService.UsuarioLogado;
                    if (usuarioLogado == null)
                    {
                        MessageBox.Show("Faça login para comentar.");
                        return;
                    }

                    publicacaoService.AdicionarComentario( post.Id, usuarioLogado.Nome, usuarioLogado.Email, textoComentario);

                    MessageBox.Show(
                        "Comentário enviado para análise do autor da publicação.",
                        "Comentário enviado",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                    CarregarPosts();
                }
            );

            container.Children.Add(campoComentario);
            card.Child = container;

            return card;
        }
    }
}