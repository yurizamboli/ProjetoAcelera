using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using ProjetoAcelera.Views.EditarObras;
using ProjetoAcelera.Views.PopUpObras;
using ProjetoAcelera.Views.Perfil;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ProjetoAcelera.Views.Obras;
using ProjetoAcelera.Views.Perfil.EditarPerfil;
using System.IO;
using ProjetoAcelera.Views.LoginRegistro;

namespace ProjetoAcelera.Views.Perfil
{
    public partial class TelaPerfil : Window
    {
        private UsuarioService usuarioService;

        public TelaPerfil()
        {
            InitializeComponent();
            this.usuarioService = App.UsuarioService;

            CarregarPerfil();
            CarregarObras();
        }
        //OBRAS
        private void CarregarObras()
        {
            var usuario = usuarioService.UsuarioLogado;

            if (usuario == null) return;

            painelObras.Children.Clear();

            painelObras.Children.Add(CriarBotaoAdicionar());

            foreach (var obra in usuario.Obras)
            {
                painelObras.Children.Add(CriarCardObra(obra));
            }
        }
        //NOMEZINHO LA NA CONTA
        private void CarregarPerfil()
        {
            var usuario = usuarioService.UsuarioLogado;
            if (usuario == null) return;

            txtNome.Text = usuario.Nome;
            if (usuario.Perfil != null) 
            {
                txtBiografia.Text = usuario.Perfil.Bio;
                textBlockFacebook.Text = usuario.Perfil.Facebook;
                textBlockInstagram.Text = usuario.Perfil.Instagram;
                string caminhoFoto = usuario.Perfil.FotoPerfil;
                // se nao for nas propriedades da imagem e colocar  build action = resource , copy to output directory = do not copy , vai crashar
                string caminhoPadrao = "pack://application:,,,/ImagemAcelera/AvatarPadrao.png";
                try
                {
                    if (!string.IsNullOrWhiteSpace(caminhoFoto) && File.Exists(caminhoFoto))
                    {
                        imgPerfil.Source = new BitmapImage(new Uri(caminhoFoto, UriKind.Absolute));
                    }
                    else
                    {
                        imgPerfil.Source = new BitmapImage(new Uri(caminhoPadrao));
                    }
                }
                catch
                {
                    imgPerfil.Source = new BitmapImage(new Uri(caminhoPadrao));
                }
            }
        }

        private Border CriarBotaoAdicionar()
        {
            Border border = new Border
            {
                Width = 120,
                Height = 160,
                Margin = new Thickness(5),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EAEAEA")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(10)
            };

            border.MouseDown += AdicionarObra_Click;

            border.Child = new TextBlock
            {
                Text = "+",
                FontSize = 40,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            return border;
        }

        private Border CriarCardObra(Obra obra)
        {
            StackPanel container = new StackPanel();

            // IMAGEM
            Image img = new Image
            {
                Height = 120,
                Stretch = Stretch.UniformToFill
            };

            try
            {
                img.Source = new BitmapImage(new Uri(obra.Capa));
            }
            catch { }

            // TÍTULO
            TextBlock titulo = new TextBlock
            {
                Text = obra.Titulo,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 12
            };

            // BOTÃO FAVORITO, ARRUMAR ISSO AQUI DEPOIS, AS ESTRELAS TÃO MUITO FEIAS, MAS FUNCIONA
            Button favBtn = new Button
            {
                Content = obra.Favorito ? "⭐" : "☆",
                Width = 30,
                Height = 25,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            favBtn.Click += (s, e) =>
            {
                var service = new ObraService(App.UsuarioService);
                service.FavoritarObra(obra.Titulo);

                CarregarObras();
            };

            // BOTÃO DE REMOVER ACHAR UM EMOJI BONITINHO E DEIXAR IGUAL O DE FAVORITAR
            Button delBtn = new Button
            {
                Content = "Deletar",
                Width = 70,
                Height = 30,
                Background = Brushes.LightCoral,
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                Cursor = System.Windows.Input.Cursors.Hand,
                FontSize = 12
            };

            delBtn.Click += (s, e) =>
            {
                var service = new ObraService(App.UsuarioService);
                service.RemoverObra(obra.Titulo);

                CarregarObras();
            };

            // BOTÕES (FAVORITO + DELETE)
            StackPanel actions = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            actions.Children.Add(favBtn);
            actions.Children.Add(delBtn);

            // MONTAGEM FINAL
            container.Children.Add(img);
            container.Children.Add(titulo);
            container.Children.Add(actions);

            Border border = new Border
            {
                Width = 120,
                Height = 160,
                Margin = new Thickness(5),
                CornerRadius = new CornerRadius(10),
                BorderBrush = Brushes.DarkBlue,
                BorderThickness = new Thickness(2),
                Child = container
            };

            border.MouseDown += (s, e) => AbrirDetalhesObra(obra);

            return border;
        }

        private void AdicionarObra_Click(object sender, MouseButtonEventArgs e)
        {
            var tela = new TelaAdicionarObra();
            tela.ShowDialog();

            CarregarObras();
        }

        private void Logout_Button(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
            "Deseja realmente sair?",
            "Logout",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) { return; }
            App.UsuarioService.Logout();
            var login = new TelaLoginRegistro();
            login.Show();
            this.Close();
        }

        private void AbrirDetalhesObra(Obra obra)
        {
            var tela = new ObrasPopUp(obra);
            tela.ShowDialog();

            CarregarObras();
        }

        private void EditarPerfil_Button(object sender, RoutedEventArgs e)
        {
            var tela = new TelaEditarPerfil();
            switch (tela.ShowDialog())
            {
                case true:
                    CarregarPerfil();
                    MessageBox.Show("Perfil atualizado!");
                    break;
                case false:
                case null:
                    MessageBox.Show("Edição cancelada!");
                    break;
            }
        }

        private void txtNome_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Voltar_Click(object sender, RoutedEventArgs e)
        {
            var tela = new Views.Teste.Dashboard();
            tela.Show();
            this.Close();
        }
    }
}