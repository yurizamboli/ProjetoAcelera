using System.Windows;
using System.Windows.Input;

namespace ProjetoAcelera.Views
{
    public partial class TelaObras : Window
    {
        public TelaObras()
        {
            InitializeComponent();
        }

        private void AdicionarObra_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Aparecer um pop up para cadastrar uma obra");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Salvo Sim");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Editando...");
        }

    }
}