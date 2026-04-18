using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ProjetoAcelera.Services;


namespace ProjetoAcelera.Views
{
    public partial class TelaExtra : Window
    {
        public TelaExtra()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TelaObras tela = new TelaObras();
            tela.Show();
            MessageBox.Show("AAAAAAAAAA");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Lonely Is The Word");
        }
    }
}