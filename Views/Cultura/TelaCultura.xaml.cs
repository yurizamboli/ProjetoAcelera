using System.Windows;
using System.Windows.Controls;

namespace ProjetoAcelera.Views.Cultura
{
    public partial class TelaCultura : Page
    {
        public TelaCultura()
        {
            InitializeComponent();
        }

        private void BtnEquipe_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TelaEquipeProjeto());
        }
    }
}
