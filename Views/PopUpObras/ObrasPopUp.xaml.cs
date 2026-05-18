using ProjetoAcelera.Ferramentas;
using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using System.Windows;
using System.Windows.Media;

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

            CarregarDadosObra();
            AtualizarEstrela();
        }

        private void CarregarDadosObra()
        {
            txtTitulo.Text = obra.Titulo;
            txtDescricao.Text = obra.Descricao;

            try
            {
                if (!string.IsNullOrWhiteSpace(obra.Capa))
                {
                    imgCapa.Source =
                        AuxilioImagens.CarregarImgOtimizada(obra.Capa, 600);
                }
            }
            catch
            {
                // vazio
            }
        }

        private void AtualizarEstrela()
        {
            if (obra.Favorito)
            {
                txtEstrela.Text = "★";
                txtEstrela.Foreground = Brushes.Gold;
            }
            else
            {
                txtEstrela.Text = "☆";
                txtEstrela.Foreground = Brushes.White;
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var tela =
                new ProjetoAcelera.Views.EditarObras.TelaEditarObra(obra);

            tela.ShowDialog();

            CarregarDadosObra();
            AtualizarEstrela();
        }

        private void BtnExcluir_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Deseja realmente excluir esta obra?",
                "Excluir Obra",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            var service = new ObraService();

            service.RemoverObra(obra.Titulo);

            this.Close();
        }

        private void BtnFavoritar_Click(object sender, RoutedEventArgs e)
        {
            obra.Favorito = !obra.Favorito;

            var service = new ObraService();

            service.AtualizarObra(obra);

            AtualizarEstrela();
        }
    }
}