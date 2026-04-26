using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using ProjetoAcelera.Views.LoginRegistro;
using System.Windows;

namespace ProjetoAcelera.Views.PopUpObras
{
    public partial class ObrasPopUp : Window
    {
        private UsuarioService usuarioService;
        private Obra obra;
        public ObrasPopUp(Obra obra)
        {
            InitializeComponent();
            this.usuarioService = App.UsuarioService;
            this.obra = obra;

            txtTitulo.Text = obra.Titulo;
            txtDescricao.Text = obra.Descricao;

            try
            {
                imgCapa.Source = new System.Windows.Media.Imaging.BitmapImage(new System.Uri(obra.Capa));
            }
            catch 
            { 
            
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var tela = new ProjetoAcelera.Views.EditarObras.TelaEditarObra(obra);
            tela.ShowDialog();

            // atualiza popup depois de editar
            txtTitulo.Text = obra.Titulo;
            txtDescricao.Text = obra.Descricao;

            try
            {
                imgCapa.Source = new System.Windows.Media.Imaging.BitmapImage(new System.Uri(obra.Capa));
            }
            catch 
            { 
            
            }
        }

        private void BtnExcluir_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
            "Deseja realmente Excluir?",
            "Excluir Obra",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) { return; }
            var service = new ObraService();
            service.RemoverObra(obra.Titulo);
            this.Close();

        }

        private void BtnFavoritar_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
            "Deseja Favoritar a obra?",
            "Favoritar Obra",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) { return; }
            var service = new ObraService();
            service.FavoritarObra(obra.Titulo);
            }
        }
    }
