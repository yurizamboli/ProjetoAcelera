using ProjetoAcelera.Services;
using ProjetoAcelera.Views.LoginRegistro;
using ProjetoAcelera.Views.MainWindow;
using ProjetoAcelera.Views.Perfil.EditarPerfil;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
namespace ProjetoAcelera.Views.Perfil
{
    public partial class TelaPerfil : Page
    {
        private UsuarioService usuarioService;

        public TelaPerfil()
        {
            InitializeComponent();

            usuarioService = App.UsuarioService;

            CarregarPerfil();

            // abre obras por padrão
            framePerfil.Navigate(new ContainerObras());
            SelecionarAba(btnObras);
        }
        private void SelecionarAba(Button botaoSelecionado)
        {
            btnPosts.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F3E6C9"));
            btnObras.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F3E6C9"));
            btnGaleria.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F3E6C9"));

            botaoSelecionado.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D7C48E"));
        }
        private void CarregarPerfil()
        {
            var usuario = usuarioService.UsuarioLogado;

            if (usuario == null)
            {
                return;
            }

            txtNome.Text = usuario.Nome;

            if (usuario.Perfil != null)
            {
                txtBiografia.Text = usuario.Perfil.Bio;

                textBlockFacebook.Text =
                    usuario.Perfil.Facebook;

                textBlockInstagram.Text =
                    usuario.Perfil.Instagram;

                string caminhoFoto =
                    usuario.Perfil.FotoPerfil;

                string caminhoPadrao =
                    "pack://application:,,,/ImagemAcelera/AvatarPadrao.png";

                try
                {
                    if (!string.IsNullOrWhiteSpace(caminhoFoto)
                        && File.Exists(caminhoFoto))
                    {
                        imgPerfil.Source =
                            new BitmapImage(
                                new Uri(caminhoFoto));
                    }
                    else
                    {
                        imgPerfil.Source =
                            new BitmapImage(
                                new Uri(caminhoPadrao));
                    }
                }
                catch
                {
                    imgPerfil.Source =
                        new BitmapImage(
                            new Uri(caminhoPadrao));
                }
            }
        }

        private void Obras_Button(object sender, RoutedEventArgs e)
        {
            SelecionarAba(btnObras);
            framePerfil.Navigate(new ContainerObras());
        }

        private void Publicacoes_Button(object sender, RoutedEventArgs e)
        {
            SelecionarAba(btnPosts);
            framePerfil.Navigate(new ContainerPublicacoes());
        }
        private void Galeria_Button(object sender, RoutedEventArgs e)
        {
            SelecionarAba(btnGaleria);
            framePerfil.Navigate(new ContainerGaleria());
        }
        private void Logout_Button(
            object sender,
            RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Deseja realmente sair?",
                "Logout",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            App.UsuarioService.Logout();

            var main =
                Application.Current.MainWindow
                as TelaMainWindow;

            main?.AtualizarNavbar();

            NavigationService.Navigate(
                new TelaLoginRegistro());
        }

        private void EditarPerfil_Button(
            object sender,
            RoutedEventArgs e)
        {
            var tela = new TelaEditarPerfil();

            switch (tela.ShowDialog())
            {
                case true:

                    CarregarPerfil();

                    MessageBox.Show(
                        "Perfil atualizado!");

                    break;

                case false:
                case null:

                    MessageBox.Show(
                        "Edição cancelada!");

                    break;
            }
        }
    }
}