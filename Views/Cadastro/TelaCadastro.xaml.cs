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
using ProjetoAcelera.Services;


namespace ProjetoAcelera.Views.Cadastro
{
    public partial class TelaCadastro : Window
    {
        private UsuarioService usuarioService;
        private bool senhaVisivel = false;
        public TelaCadastro(UsuarioService service)
        {
            InitializeComponent();
            this.usuarioService = service;

            // Mantém o placeholder do PasswordBox atualizado quando o usuário digita
            txtSenha.PasswordChanged += TxtSenha_PasswordChanged;
        }
        private string LimparPlaceholder(string texto, string placeholder)
        {
            return texto == placeholder ? "" : texto;
        }

        //Botão de cadastro
        private void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            string nome = LimparPlaceholder(txtNome.Text, "Nome");
            string email = LimparPlaceholder(txtEmail.Text, "Email");
            string senha = txtSenha.Password;

            bool sucesso = usuarioService.Cadastrar(nome, senha, email);

            if (sucesso)
            {
                MessageBox.Show("Cadastro feito!");
                this.Close();
            }

        }
        //Botão de voltar
        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //SUMIR O NOME
        private void TxtNome_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtNome.Text == "Nome")
            {
                txtNome.Text = "";
                txtNome.Foreground = Brushes.Black;
            }
        }
       
        private void TxtNome_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                txtNome.Text = "Nome";
                txtNome.Foreground = Brushes.Gray;
            }
        }

        //SUMIR O EMAIL
        private void TxtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text == "Email")
            {
                txtEmail.Text = "";
                txtEmail.Foreground = Brushes.Black;
            }
        }

        private void TxtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Text = "Email";
                txtEmail.Foreground = Brushes.Gray;
            }
        }

        //SUMIR A SENHA
        private void TxtSenha_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSenhaPlaceholder.Visibility = Visibility.Hidden;
        }

        private void TxtSenha_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSenha.Password))
            {
                txtSenhaPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void TxtSenha_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtSenhaPlaceholder.Visibility = string.IsNullOrEmpty(txtSenha.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        //mudar quando o usurio digitar no campo de texto nome
        private void txtNome_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        //mudar quando o usurio digitar no campo de texto email
        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void BtnToggleSenha_Click(object sender, RoutedEventArgs e)
        {
            senhaVisivel = !senhaVisivel;

            if (senhaVisivel)
            {
                txtSenhaVisivel.Text = txtSenha.Password;

                txtSenha.Visibility = Visibility.Collapsed;
                txtSenhaVisivel.Visibility = Visibility.Visible;
            }
            else
            {
                txtSenha.Password = txtSenhaVisivel.Text;

                txtSenhaVisivel.Visibility = Visibility.Collapsed;
                txtSenha.Visibility = Visibility.Visible;
            }
        }

        private void txtSenha_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!senhaVisivel)
                txtSenhaVisivel.Text = txtSenha.Password;

            txtSenhaPlaceholder.Visibility =
                string.IsNullOrEmpty(txtSenha.Password)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void txtSenhaVisivel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (senhaVisivel)
                txtSenha.Password = txtSenhaVisivel.Text;

            txtSenhaPlaceholder.Visibility =
                string.IsNullOrEmpty(txtSenhaVisivel.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}