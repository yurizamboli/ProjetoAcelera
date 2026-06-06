using Microsoft.Win32;
using ProjetoAcelera.Services;
using System.Windows;
using System.Windows.Media.Imaging;

// Tela usada para casdastrar uma nova obra no perfil do usuário
namespace ProjetoAcelera.Views.Obras
{
    public partial class TelaAdicionarObra : Window
    {
        private ObraService obraService;

        public TelaAdicionarObra()
        {
            InitializeComponent();
            obraService = new ObraService();
        }

        // BOTÃO PARA SALVAR A OBRA
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
        
        // guarda o caminho da imagem escolhida para a capa
        private string caminhoImagem;

        // BOTÃO PARA SELECIONAR A IMAGEM DA OBRA
        private void BtnSelecionarImagem_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Imagens (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

            if (dialog.ShowDialog() == true)
            {
                caminhoImagem = dialog.FileName;

                imgPreview.Source = new BitmapImage(new Uri(caminhoImagem));

                placeholderImagem.Visibility = Visibility.Collapsed;
            }
        }
        // DESCRIÇÃO
        private void TxtDescricao_GotFocus(object sender, RoutedEventArgs e)
        {
            txtDescricaoPlaceholder.Visibility = Visibility.Hidden;
        }

        // DESCRIÇÃO
        private void TxtDescricao_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                txtDescricaoPlaceholder.Visibility = Visibility.Visible;
            }
        }

        // DESCRIÇÃO
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}