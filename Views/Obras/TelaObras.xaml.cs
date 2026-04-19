using ProjetoAcelera.Services;
using ProjetoAcelera.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace ProjetoAcelera.Views.Obras
{
    public partial class TelaObras : Window
    {
        private UsuarioService usuarioService;
        public TelaObras()
        {
            InitializeComponent();
            
        }

        private void AdicionarObra_Click(object sender, MouseButtonEventArgs e)
        {
            TelaAdicionarObra tela = new TelaAdicionarObra();
            tela.ShowDialog();
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