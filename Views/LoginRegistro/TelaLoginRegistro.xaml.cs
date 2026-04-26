using ProjetoAcelera.Services;
using ProjetoAcelera.Views.Teste;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjetoAcelera.Views.LoginRegistro
{
    public partial class TelaLoginRegistro : Window
    {
        private UsuarioService usuarioService;
        private EmailService emailService;
        private bool senhaVisivelRegistro = false;
        private bool senhaVisivelLogin = false;
        public TelaLoginRegistro()
        {
            InitializeComponent();
           this.usuarioService = App.UsuarioService;
            this.emailService = App.EmailService;
            // Mantém o placeholder do PasswordBox atualizado quando o usuário digita
            txtSenhaRegistro.PasswordChanged += TxtSenhaRegistro_PasswordChanged;
        }
        private string LimparPlaceholder(string texto, string placeholder)
        {
            return texto == placeholder ? "" : texto;
        }

        //Botão de cadastro
        private void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            string nome = LimparPlaceholder(txtNomeRegistro.Text, "Nome");
            string email = LimparPlaceholder(txtEmailRegistro.Text, "Email");
            string senha = txtSenhaRegistro.Password;

            bool sucesso = usuarioService.Cadastrar(nome, senha, email);

            if (sucesso)
            {
                MessageBox.Show("Cadastro feito!");
                txtNomeRegistro.Text = "Nome";
                txtNomeRegistro.Foreground = Brushes.Gray;

                txtEmailRegistro.Text = "Email";
                txtEmailRegistro.Foreground = Brushes.Gray;

                txtSenhaRegistro.Password = "";
                txtSenhaVisivelRegistro.Text = "";

                txtSenhaPlaceholderRegistro.Visibility = Visibility.Visible;

                txtSenhaRegistro.Visibility = Visibility.Visible;
                txtSenhaVisivelRegistro.Visibility = Visibility.Collapsed;

                senhaVisivelRegistro = false;
            }

        }      
        //SUMIR O NOME
        private void TxtNomeRegistro_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtNomeRegistro.Text == "Nome")
            {
                txtNomeRegistro.Text = "";
                txtNomeRegistro.Foreground = Brushes.Black;
            }
        }

        private void TxtNomeRegistro_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNomeRegistro.Text))
            {
                txtNomeRegistro.Text = "Nome";
                txtNomeRegistro.Foreground = Brushes.Gray;
            }
        }

        //SUMIR O EMAIL
        private void TxtEmailRegistro_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmailRegistro.Text == "Email")
            {
                txtEmailRegistro.Text = "";
                txtEmailRegistro.Foreground = Brushes.Black;
            }
        }

        private void TxtEmailRegistro_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmailRegistro.Text))
            {
                txtEmailRegistro.Text = "Email";
                txtEmailRegistro.Foreground = Brushes.Gray;
            }
        }

        //SUMIR A SENHA
        private void TxtSenhaRegistro_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSenhaPlaceholderRegistro.Visibility = Visibility.Hidden;
        }

        private void TxtSenhaRegistro_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSenhaRegistro.Password))
            {
                txtSenhaPlaceholderRegistro.Visibility = Visibility.Visible;
            }
        }

        private void TxtSenhaRegistro_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtSenhaPlaceholderRegistro.Visibility = string.IsNullOrEmpty(txtSenhaRegistro.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        //mudar quando o usurio digitar no campo de texto nome
        private void txtNomeRegistro_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        //mudar quando o usurio digitar no campo de texto email
        private void txtEmailRegistro_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void BtnToggleSenha_ClickRegistro(object sender, RoutedEventArgs e)
        {
            senhaVisivelRegistro = !senhaVisivelRegistro;

            if (senhaVisivelRegistro)
            {
                txtSenhaVisivelRegistro.Text = txtSenhaRegistro.Password;

                txtSenhaRegistro.Visibility = Visibility.Collapsed;
                txtSenhaVisivelRegistro.Visibility = Visibility.Visible;
            }
            else
            {
                txtSenhaRegistro.Password = txtSenhaVisivelRegistro.Text;

                txtSenhaVisivelRegistro.Visibility = Visibility.Collapsed;
                txtSenhaRegistro.Visibility = Visibility.Visible;
            }
        }

        private void txtSenhaRegistro_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!senhaVisivelRegistro)
                txtSenhaVisivelRegistro.Text = txtSenhaRegistro.Password;

            txtSenhaPlaceholderRegistro.Visibility =
                string.IsNullOrEmpty(txtSenhaRegistro.Password)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void txtSenhaVisivelRegistro_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (senhaVisivelRegistro)
                txtSenhaRegistro.Password = txtSenhaVisivelRegistro.Text;

            txtSenhaPlaceholderRegistro.Visibility =
                string.IsNullOrEmpty(txtSenhaVisivelRegistro.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }



        //PARTE DO LOGIN//  
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmailLogin.Text;
            string senha = txtSenhaLogin.Password;

            bool sucesso = usuarioService.Login(email, senha);

            if (sucesso)
            {
                Dashboard tela = new Dashboard();
                tela.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Email ou senha inválidos");
            }
        }

        //SUMIR O USUARIO
        private void TxtEmailLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmailLogin.Text == "Usuário")
            {
                txtEmailLogin.Text = "";
                txtEmailLogin.Foreground = Brushes.Black;
            }
        }

        private void TxtEmailLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmailLogin.Text))
            {
                txtEmailLogin.Text = "Usuário";
                txtEmailLogin.Foreground = Brushes.Gray;
                string email = txtEmailLogin.Text;

                if (email == "Usuário")
                {
                    email = "";
                }
            }
        }
        //SE QUISER FAZER ALGO ENQUANTO O USUARIO DIGITA, PODE FAZER NISSO AQUI
        // Exemplo: validar o email em tempo real, mostrar mensagens de erro.
        private void txtEmailLogin_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        //SUMIR A SENHA
        private void TxtSenhaLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSenhaPlaceholderLogin.Visibility = Visibility.Hidden;
        }

        private void TxtSenhaLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSenhaLogin.Password))
            {
                txtSenhaPlaceholderLogin.Visibility = Visibility.Visible;
            }
        }
        private void txtSenhaLogin_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!senhaVisivelLogin)
                txtSenhaVisivelLogin.Text = txtSenhaLogin.Password;
        }

        private void txtSenhaVisivelLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (senhaVisivelLogin)
                txtSenhaLogin.Password = txtSenhaVisivelLogin.Text;
        }
        private void BtnToggleSenha_ClickLogin(object sender, RoutedEventArgs e)
        {
            senhaVisivelLogin = !senhaVisivelLogin;

            if (senhaVisivelLogin)
            {
                txtSenhaVisivelLogin.Text = txtSenhaLogin.Password;

                txtSenhaLogin.Visibility = Visibility.Collapsed;
                txtSenhaVisivelLogin.Visibility = Visibility.Visible;
            }
            else
            {
                txtSenhaLogin.Password = txtSenhaVisivelLogin.Text;

                txtSenhaVisivelLogin.Visibility = Visibility.Collapsed;
                txtSenhaLogin.Visibility = Visibility.Visible;
            }
        }

        private void BtnEsqueceuSenha_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmailLogin.Text;

            var token = usuarioService.GerarTokenRecup(email);

            if (token == null)
            {
                MessageBox.Show("Usuário não encontrado");
                return;
            }
            bool emailEnviado = emailService.EnviarTokenPorEmail(email, token);

            if (!emailEnviado)
            {
                MessageBox.Show("Erro ao enviar email de recuperação.");
                return;
            }

            MessageBox.Show("Token enviado para seu email!");

            var tela = new RedefinirSenha();
            tela.ShowDialog();
        }
    }
}
