using ProjetoAcelera.Views.Admin;
using ProjetoAcelera.Views.Perfil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjetoAcelera.Views.MainWindow
{
    public partial class TelaMainWindow : Window
    {
        public TelaMainWindow()
        {
            InitializeComponent();
            AtualizarNavbar();
            SwitchFrame.Navigate(new Views.Home.TelaHome());
        }
        public void AtualizarNavbar()
        {
            var usuario = App.UsuarioService.UsuarioLogado;

            if (usuario == null)
            {
                BtnLogin.Visibility = Visibility.Visible;
                BtnConta.Visibility = Visibility.Collapsed;
            }
            else
            {
                BtnLogin.Visibility = Visibility.Collapsed;
                BtnConta.Visibility = Visibility.Visible;
            }
        }


        private void Home_Click(object sender, RoutedEventArgs e)
        {
            SwitchFrame.Navigate(new Views.Home.TelaHome());
        }

        private void Programacao_Click(object sender, RoutedEventArgs e) {
            SwitchFrame.Navigate(new Views.Calendario.Calendario());
        }
        private void Cultura_Click(object sender, RoutedEventArgs e) 
        {

        }
        private void Artistas_Click(object sender, RoutedEventArgs e) 
        {
            SwitchFrame.Navigate(new Views.Artistas.TelaArtista());
        }
        private void NossaCidade_Click(object sender, RoutedEventArgs e)
        {
            SwitchFrame.Navigate(new Views.Teste.Dashboard());
        }       
        private void Conta_Click(object sender, RoutedEventArgs e)
        {
            var usuario = App.UsuarioService.UsuarioLogado;

            if (usuario.Cargo == "Admin")
            {
                SwitchFrame.Navigate(new Views.Admin.TelaAdmin());
            }
            else
            {
                SwitchFrame.Navigate(new Views.Perfil.TelaPerfil());
            }


        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            SwitchFrame.Navigate(new Views.LoginRegistro.TelaLoginRegistro());
        }

        private void Fechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
