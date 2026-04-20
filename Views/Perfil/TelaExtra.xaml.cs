using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ProjetoAcelera.Services;
using ProjetoAcelera.Views.Obras;

namespace ProjetoAcelera.Views.Perfil
{
    public partial class TelaExtra : Window
    {
        public TelaExtra()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Mudei pra isso pro programa rodar
            TelaObras tela = new TelaObras(App.UsuarioService);
            tela.Show();
            MessageBox.Show("AAAAAAAAAA");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Lonely Is The Word");
        }
    }
}