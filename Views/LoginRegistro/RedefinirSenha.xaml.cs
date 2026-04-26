using ProjetoAcelera.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjetoAcelera.Views.LoginRegistro
{
    partial class RedefinirSenha : Window
    {
        private UsuarioService usuarioService;

        public RedefinirSenha()
        {
            InitializeComponent();
            this.usuarioService = App.UsuarioService;
        }
            
        private void BtnRedefinir_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string token = txtToken.Text;
            string novaSenha = txtNovaSenha.Password;
            string confirmar = txtConfirmarSenha.Password;

            if (novaSenha != confirmar)
            {
                MessageBox.Show("As senhas não coincidem");
                return;
            }

            bool sucesso = usuarioService.RedefinirSenha(email, token, novaSenha);

            if (sucesso)
            {
                MessageBox.Show("Senha redefinida com sucesso!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Token inválido ou expirado");
            }
        }
    }
}
