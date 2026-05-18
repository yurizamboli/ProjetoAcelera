using ProjetoAcelera.Ferramentas;
using ProjetoAcelera.Models;
using System;
using System.Windows;

namespace ProjetoAcelera.Views.PopUpObras
{
    public partial class ObrasVisualPopUp : Window
    {
        private Obra obra;

        public ObrasVisualPopUp(Obra obraRecebida)
        {
            InitializeComponent();

            obra = obraRecebida;

            CarregarDados();
        }

        private void CarregarDados()
        {
            txtTitulo.Text = obra.Titulo;
            txtDescricao.Text = obra.Descricao;

            try
            {
                if (!string.IsNullOrWhiteSpace(obra.Capa))
                {
                    imgCapa.Source =
                        AuxilioImagens.CarregarImgOtimizada(obra.Capa,700);
                }
            }
            catch
            {
                // vazio
            }

            if (obra.Favorito)
            {
                estrelaFavorita.Visibility = Visibility.Visible;
            }
            else
            {
                estrelaFavorita.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}