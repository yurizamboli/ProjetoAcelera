using Microsoft.Win32;
using ProjetoAcelera.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProjetoAcelera.Views.Obras
{
    public partial class TelaAdicionarObra : Window
    {
        private ObraService obraService;

        public TelaAdicionarObra()
        {
            InitializeComponent();
            obraService = new ObraService(App.UsuarioService);
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            string titulo = txtTitulo.Text;
            string descricao = txtDescricao.Text;

            if (string.IsNullOrEmpty(caminhoImagem))
            {
                MessageBox.Show("Selecione uma imagem!");
                return;
            }
            //adicionar o bichao
            obraService.AdicionarObra(titulo, descricao, caminhoImagem);

            MessageBox.Show("Obra cadastrada!");
            this.Close();
        }

        //Isso aqui vai ter que mudar, ta salvando a imagem no caminho do projeto, vai quebrar em outro pc, vai ter que aprender a salvar a imagem
        private string caminhoImagem;
        private void BtnSelecionarImagem_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Imagens (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

            if (dialog.ShowDialog() == true)
            {
                caminhoImagem = dialog.FileName;

                imgPreview.Source = new BitmapImage(new Uri(caminhoImagem));
            }
        }
        // DESCRIÇÃO
        private void TxtDescricao_GotFocus(object sender, RoutedEventArgs e)
        {
            txtDescricaoPlaceholder.Visibility = Visibility.Hidden;
        }

        private void TxtDescricao_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                txtDescricaoPlaceholder.Visibility = Visibility.Visible;
            }
        }
        private void txtDescricao_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
        // TÍTULO
        private void TxtTitulo_GotFocus(object sender, RoutedEventArgs e)
        {
            txtTituloPlaceholder.Visibility = Visibility.Hidden;
        }

        private void TxtTitulo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                txtTituloPlaceholder.Visibility = Visibility.Visible;
            }
        }
        private void txtTitulo_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }



    }
}