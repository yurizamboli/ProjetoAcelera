using System.Windows;
using ProjetoAcelera.Models;

namespace ProjetoAcelera.Views.PopUpObras
{
    public partial class ObrasPopUp : Window
    {
        private Obra obra;

        public ObrasPopUp(Obra obra)
        {
            InitializeComponent();

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
    }
}