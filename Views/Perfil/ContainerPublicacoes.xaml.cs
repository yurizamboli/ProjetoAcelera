using Microsoft.Win32;
using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ProjetoAcelera.Ferramentas;

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
            var usuarios = App.UsuarioService.ObterTodos();
            foreach (var pub in publicacoes)
            {
                Border card = PublicacaoComponentesVisual.CriarCardPublicacao();
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
                string caminhoPadrao = "pack://application:,,,/ImagemAcelera/AvatarPadrao.png";
                try
                {
                    var usuarioAutor = usuarios.FirstOrDefault(u => u.Email == pub.EmailAutor);
                    string fotoAutor = usuarioAutor?.Perfil?.FotoPerfil ?? "";

                    if (!string.IsNullOrWhiteSpace(fotoAutor) && File.Exists(fotoAutor))
                    {
                        avatar.Source = AuxilioImagens.CarregarImgOtimizada(fotoAutor, 80);
                    }
                    else
                    {
                        avatar.Source = AuxilioImagens.CarregarImgOtimizada(caminhoPadrao, 80);
                    }
                }
                catch
                {
                    avatar.Source = AuxilioImagens.CarregarImgOtimizada(caminhoPadrao,80);
                }
                avatarBorder.Child = avatar;
                StackPanel infoAutor = new StackPanel
                {
                    Margin = new Thickness(10, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center
                };

                TextBlock autor = PublicacaoComponentesVisual.CriarTextoAutor(pub.NomeAutor);

                TextBlock data = PublicacaoComponentesVisual.CriarTextoData(
                    pub.DataPublicacao.ToString("dd/MM/yyyy HH:mm")
                );

                infoAutor.Children.Add(autor);
                infoAutor.Children.Add(data);
                topoPost.Children.Add(avatarBorder);
                topoPost.Children.Add(infoAutor);
                stack.Children.Add(topoPost);
                stack.Children.Add(PublicacaoComponentesVisual.CriarLinhaAzul(new Thickness(-15, 8, -15, 10)));
                // STATUS: aparece só se estiver no feed global
                TextBlock status = new TextBlock
                {
                    Text = "🌎 Postagem em destaque no feed global",
                    FontWeight = FontWeights.Bold,
                    FontSize = 12,
                    Foreground = Brushes.Green,
                    Margin = new Thickness(0, 0, 0, 8),
                    Visibility = pub.Status == "Aprovado" ? Visibility.Visible : Visibility.Collapsed
                };
                stack.Children.Add(status);

                // IMAGEM
                if (!string.IsNullOrWhiteSpace(pub.ImagemUrl) && File.Exists(pub.ImagemUrl))
                {
                    Border bordaImagem = PublicacaoComponentesVisual.CriarMolduraImagem();
                    Image imagemPost = new Image
                    {
                        Stretch = Stretch.Uniform,
                        MaxHeight = 420,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };

                    try
                    {
                        imagemPost.Source = AuxilioImagens.CarregarImgOtimizada(pub.ImagemUrl,800);
                        bordaImagem.Child = imagemPost;
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

                stack.Children.Add(PublicacaoComponentesVisual.CriarLinhaAzul(new Thickness(-15, 0, -15, 10))
                );

                Grid areaStats = PublicacaoComponentesVisual.CriarAreaStats();
                StackPanel statsEsquerda = PublicacaoComponentesVisual.CriarStatsEsquerda();
                TextBlock curtidas = PublicacaoComponentesVisual.CriarTextoCurtidas(pub.Curtidas);
                statsEsquerda.Children.Add(curtidas);

                Grid.SetColumn(statsEsquerda, 0);
                areaStats.Children.Add(statsEsquerda);

                PublicacaoComponentesVisual.CriarAreaComentarios(pub,statsEsquerda,stack,true,true,id =>
                {
                    publicacaoService.AprovarComentario(id);
                    CarregarPublicacoes();
                },
                id =>
                {
                    publicacaoService.ReprovarComentario(id);
                    CarregarPublicacoes();
                }
                );

                Button btnRemover = new Button
                {
                    Content = "Remover",
                    Width = 90,
                    Height = 30,
                    Background = Brushes.DarkRed,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    BorderThickness = new Thickness(0),
                    Cursor = Cursors.Hand,
                    VerticalAlignment = VerticalAlignment.Center
                };

                btnRemover.Click += (s, e) =>
                {
                    var resultado = MessageBox.Show(
                        "Deseja remover esta publicação?",
                        "Remover publicação",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (resultado != MessageBoxResult.Yes)
                    {
                        return;
                    }
                    publicacaoService.RemoverPublicacao(pub.Id);
                    CarregarPublicacoes();
                };

                Grid.SetColumn(btnRemover, 1);
                areaStats.Children.Add(btnRemover);
                stack.Children.Add(areaStats);
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
                caminhoImagemSelecionada,
                "",
                chkPermiteComentarios.IsChecked == true
            );

            txtNovaPublicacao.Clear();

            caminhoImagemSelecionada = "";
            txtImagemSelecionada.Text = "Clique para selecionar uma imagem";
            chkPermiteComentarios.IsChecked = true;

            CarregarPublicacoes();
        }
    }
}