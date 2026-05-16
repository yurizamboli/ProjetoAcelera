using Microsoft.Win32;
using ProjetoAcelera.Ferramentas;
using ProjetoAcelera.Services;
using ProjetoAcelera.Views.Perfil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProjetoAcelera.Views.Perfil.EditarPerfil
{
    public partial class TelaEditarPerfil : Window
    {
        private string caminhoImagemSelecionada;
        public TelaEditarPerfil()
        {
            InitializeComponent();
            CarregarDadosPerfil();
        }

        private void CarregarDadosPerfil()
        {
            var usuario = App.UsuarioService.UsuarioLogado;
            if (usuario == null) 
            {
                return;
            }

            txtNome.Text = usuario.Nome ?? "";
            txtBio.Text = usuario.Perfil?.Bio ?? "";
            txtFacebook.Text = usuario.Perfil?.Facebook ?? "";
            txtInstagram.Text = usuario.Perfil?.Instagram ?? "";
            string foto = usuario.Perfil?.FotoPerfil ?? "";
            string fotoPadrao = "pack://application:,,,/ImagemAcelera/AvatarPadrao.png";
            try
            {
                if (!string.IsNullOrWhiteSpace(foto) && File.Exists(foto))
                {
                    imgPreview.Source = AuxilioImagens.CarregarImgOtimizada(foto,250);
                }
                else
                {
                    imgPreview.Source = AuxilioImagens.CarregarImgOtimizada(fotoPadrao,250);
                }
            }
            catch
            {
                imgPreview.Source = AuxilioImagens.CarregarImgOtimizada(fotoPadrao,250);
            }
        }
        


        private void EscolherFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (dialog.ShowDialog() == true)
            {
                caminhoImagemSelecionada = dialog.FileName;
                imgPreview.Source = AuxilioImagens.CarregarImgOtimizada(caminhoImagemSelecionada,250);

                BitmapImage bitmapOriginal = new BitmapImage(new Uri(caminhoImagemSelecionada, UriKind.Absolute));
                int largura = bitmapOriginal.PixelWidth;
                int altura = bitmapOriginal.PixelHeight;

                if (largura != altura)
                {
                    MessageBox.Show(
                        "ℹ️ Dica: use uma imagem quadrada (ex: 180x180, 360x360, 540x540) para evitar distorções.",
                        "Verificação da Foto",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
             }
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            try {
                var perfilService = new PerfilService(App.UsuarioService);
                var usuario = App.UsuarioService.UsuarioLogado;

                string fotoFinal = usuario.Perfil.FotoPerfil;
                if (!string.IsNullOrEmpty(caminhoImagemSelecionada))
                {
                    fotoFinal = caminhoImagemSelecionada;
                }
                perfilService.AtualizarPerfil(
                    txtNome.Text,
                    txtBio.Text,
                    txtFacebook.Text,
                    txtInstagram.Text,
                    fotoFinal);
                DialogResult = true;
                this.Close();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
        
    }
}