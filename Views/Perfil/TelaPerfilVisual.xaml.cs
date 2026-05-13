using ProjetoAcelera.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjetoAcelera.Views.Perfil
{
    public partial class TelaPerfilVisual : Page
    {
        private Usuario usuario;

        public TelaPerfilVisual(Usuario user)
        {
            InitializeComponent();

            usuario = user;

            CarregarPerfil();

            framePerfilVisual.Navigate(new ContainerObrasVisual(usuario));

            SelecionarAba(btnObras);
        }

        private void SelecionarAba(Button botaoSelecionado)
        {
            btnPosts.Foreground =
                new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#F3E6C9"));

            btnObras.Foreground =
                new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#F3E6C9"));

            btnGaleria.Foreground =
                new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#F3E6C9"));

            botaoSelecionado.Foreground =
                new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#D7C48E"));
        }

        private void CarregarPerfil()
        {
            txtNome.Text = usuario.Nome;

            txtBio.Text =
                usuario.Perfil?.Bio;

            txtFacebook.Text =
                usuario.Perfil?.Facebook;

            txtInstagram.Text =
                usuario.Perfil?.Instagram;

            string caminhoPadrao =
                "pack://application:,,,/ImagemAcelera/AvatarPadrao.png";

            try
            {
                if (!string.IsNullOrWhiteSpace(usuario.Perfil?.FotoPerfil)
                    && File.Exists(usuario.Perfil.FotoPerfil))
                {
                    imgPerfil.Source =
                        new BitmapImage(
                            new Uri(usuario.Perfil.FotoPerfil));
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

        private void Obras_Button(
            object sender,
            RoutedEventArgs e)
        {
            SelecionarAba(btnObras);

            framePerfilVisual.Navigate(new ContainerObrasVisual(usuario));
        }

        private void Posts_Button(
            object sender,
            RoutedEventArgs e)
        {
            SelecionarAba(btnPosts);

            framePerfilVisual.Navigate(new ContainerPostsVisual(usuario));
        }

        private void Galeria_Button(
            object sender,
            RoutedEventArgs e)
        {
            SelecionarAba(btnGaleria);

            framePerfilVisual.Navigate(new ContainerGaleriaVisual(usuario));
        }
    }
}