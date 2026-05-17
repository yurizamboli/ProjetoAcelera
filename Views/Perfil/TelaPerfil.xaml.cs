using ProjetoAcelera.Ferramentas;
using ProjetoAcelera.Services;
using ProjetoAcelera.Views.LoginRegistro;
using ProjetoAcelera.Views.MainWindow;
using ProjetoAcelera.Views.Perfil.EditarPerfil;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ProjetoAcelera.Views.Admin;

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
            VerificarPermissaoAdmin();
            // abre obras por padrão
            framePerfil.Navigate(new ContainerPublicacoes());
            SelecionarAba(btnPosts);
        }
        private void SelecionarAba(Button botaoSelecionado)
        {
            btnPosts.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F3E6C9"));
            btnObras.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F3E6C9"));
            btnGaleria.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F3E6C9"));
            btnAdmin.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F3E6C9"));
            botaoSelecionado.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D7C48E"));
        }
        private void VerificarPermissaoAdmin()
        {
            var usuario = App.UsuarioService.UsuarioLogado;

            if (usuario != null && usuario.Cargo == "Admin")
            {
                btnAdmin.Visibility = Visibility.Visible;
            }
            else
            {
                btnAdmin.Visibility = Visibility.Collapsed;
            }
        }
        private void CarregarPerfil()
        {
            var usuario = usuarioService.UsuarioLogado;

            if (usuario == null)
            {
                return;
            }

            txtNome.Text = usuario.Nome;

            string caminhoPadrao ="pack://application:,,,/ImagemAcelera/AvatarPadrao.png";

            if (usuario.Perfil == null)
            {
                imgPerfil.Source = AuxilioImagens.CarregarImgOtimizada(caminhoPadrao,250);
                return;
            }

            txtBiografia.Text = usuario.Perfil.Bio;
            textBlockFacebook.Text = usuario.Perfil.Facebook;
            textBlockInstagram.Text = usuario.Perfil.Instagram;
            string caminhoFoto = usuario.Perfil.FotoPerfil;

            try
            {
                if (!string.IsNullOrWhiteSpace(caminhoFoto) && File.Exists(caminhoFoto))
                {
                    imgPerfil.Source = AuxilioImagens.CarregarImgOtimizada(caminhoFoto,250);
                }
                else
                {
                    imgPerfil.Source = AuxilioImagens.CarregarImgOtimizada(caminhoPadrao,250);
                }
            }
            catch
            {
                imgPerfil.Source = AuxilioImagens.CarregarImgOtimizada(caminhoPadrao, 250);
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
        private void Admin_Button(object sender, RoutedEventArgs e)
        {
            SelecionarAba(btnAdmin);
            framePerfil.Navigate(new ContainerPainelAdmin());
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