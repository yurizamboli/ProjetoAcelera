using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ProjetoAcelera.Services;
using ProjetoAcelera.Views.Perfil;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using System.Linq.Expressions;

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
            if (usuario == null) return;

            txtNome.Text = usuario.Nome ?? "";
            txtBio.Text = usuario.Perfil?.Bio ?? "";
            txtFacebook.Text = usuario.Perfil?.Facebook ?? "";
            txtInstagram.Text = usuario.Perfil?.Instagram ?? "";
            string foto = usuario.Perfil?.FotoPerfil ?? "";
            if (string.IsNullOrWhiteSpace(foto))
            {
                foto = "pack://application:,,,/ImagemAcelera/AvatarPadrao.png";
            }

            imgPreview.Source = new BitmapImage(new Uri(foto));
        }


        private void EscolherFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (dialog.ShowDialog() == true)
            {
                caminhoImagemSelecionada = dialog.FileName;
                var bitmap = new BitmapImage(new Uri(caminhoImagemSelecionada));

                imgPreview.Source = bitmap;

                int largura = bitmap.PixelWidth;
                int altura = bitmap.PixelHeight;


                if (largura == altura)
                {
                    if (largura < 180)
                        MessageBox.Show("⚠️ A imagem está abaixo do mínimo recomendado (180x180). Pode perder qualidade.");
                }
                else
                {
                    MessageBox.Show("ℹ️ Dica: use uma imagem quadrada (ex: 180x180, 360x360, 540x540) para evitar distorções.", "Verificação da Foto", MessageBoxButton.OK, MessageBoxImage.Information);
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