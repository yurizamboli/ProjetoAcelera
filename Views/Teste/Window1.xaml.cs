using ProjetoAcelera.Views.Admin;
using ProjetoAcelera.Views.Artistas;
using ProjetoAcelera.Views.Calendario;
using ProjetoAcelera.Views.Perfil;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjetoAcelera.Views.Teste
{
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Conta_Click(object sender, RoutedEventArgs e)
        {
            var usuario = App.UsuarioService.UsuarioLogado;

            if (usuario.Cargo == "Admin")
            {
                new TelaAdmin().Show();
            }
            else
            {
                new TelaPerfil().Show();
            }

            this.Close();
        }

        private void Artistas_Click(object sender, RoutedEventArgs e)
        {
            TelaArtista tela = new TelaArtista();
            tela.Show();
            this.Close();
        }

        private void Programacao_Click(object sender, RoutedEventArgs e)
        {
            var tela = new Views.Calendario.Calendario();
            tela.Show();
            this.Close();
        }
    }
}