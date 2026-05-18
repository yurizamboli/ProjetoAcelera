using Microsoft.Win32;
using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProjetoAcelera.Views.EditarObras
{
    public partial class TelaEditarObra : Window
    {
        private ObraService obraService;
        private Obra obraAtual;
        private string caminhoImagem = string.Empty;

        public TelaEditarObra(Obra obra)
        {
            InitializeComponent();

            obraService = new ObraService();
            obraAtual = obra;

            PreencherCampos();
        }

        private void PreencherCampos()
        {
            txtTitulo.Text = obraAtual.Titulo;
            txtDescricao.Text = obraAtual.Descricao;
            caminhoImagem = obraAtual.Capa;

            //placeholder do titulo
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                txtTituloPlaceholder.Visibility = Visibility.Visible;
            }
            else
            {
                txtTituloPlaceholder.Visibility = Visibility.Hidden;
            }
            try
            {
                imgPreview.Source = new BitmapImage(new Uri(caminhoImagem));
            }
            catch 
            {
                
            }

            // placeholder da descrição
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                txtDescricaoPlaceholder.Visibility = Visibility.Visible;
            }
            else
            {
                txtDescricaoPlaceholder.Visibility = Visibility.Hidden;
            }

            try
            {
                imgPreview.Source = new BitmapImage(new Uri(caminhoImagem));

                placeholderImagem.Visibility = Visibility.Hidden;
            }
            catch
            {

            }

        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            obraAtual.Titulo = txtTitulo.Text;
            obraAtual.Descricao = txtDescricao.Text;
            obraAtual.Capa = caminhoImagem;

            obraService.AtualizarObra(obraAtual);

            MessageBox.Show("Obra atualizada!");
            this.Close();
        }

        private void BtnSelecionarImagem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Imagens (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

            if (dialog.ShowDialog() == true)
            {
                caminhoImagem = dialog.FileName;
                imgPreview.Source = new BitmapImage(new Uri(caminhoImagem));
                placeholderImagem.Visibility = Visibility.Hidden;
            }
        }

        // placeholders (igual teu adicionar)
        private void TxtDescricao_GotFocus(object sender, RoutedEventArgs e)
        {
            txtDescricaoPlaceholder.Visibility = Visibility.Hidden;
        }

        private void TxtDescricao_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
                txtDescricaoPlaceholder.Visibility = Visibility.Visible;
        }

        private void TxtTitulo_GotFocus(object sender, RoutedEventArgs e)
        {
            txtTituloPlaceholder.Visibility = Visibility.Hidden;
        }

        private void TxtTitulo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
                txtTituloPlaceholder.Visibility = Visibility.Visible;
        }

        private void txtDescricao_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void txtTitulo_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}