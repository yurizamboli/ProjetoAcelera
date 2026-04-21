using ProjetoAcelera.Services;
using ProjetoAcelera.Views.Cadastro;
using ProjetoAcelera.Views.Obras;
using ProjetoAcelera.Views.Perfil;
using ProjetoAcelera.Views.Teste;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
//Tem um erro em algum lugar ai, mas o codigo roda suave com ele então sla, n vou mexer
namespace ProjetoAcelera.Views.Login
{
    public partial class TelaLogin : Window
    {
        private UsuarioService usuarioService;
        private bool senhaVisivel = false;
        public TelaLogin()
        {
            InitializeComponent();
            usuarioService = App.UsuarioService;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string senha = txtSenha.Password;

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
        private void BtnCadastrarTela_Click(object sender, RoutedEventArgs e)
        {
            TelaCadastro tela = new TelaCadastro(usuarioService);
            tela.ShowDialog();
        }

        //SUMIR O USUARIO
        private void TxtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text == "Usuário")
            {
                txtEmail.Text = "";
                txtEmail.Foreground = Brushes.Black;
            }
        }

        private void TxtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Text = "Usuário";
                txtEmail.Foreground = Brushes.Gray;
                string email = txtEmail.Text;

                if (email == "Usuário")
                {
                    email = "";
                }
            }
        }
        //SE QUISER FAZER ALGO ENQUANTO O USUARIO DIGITA, PODE FAZER NISSO AQUI
        // Exemplo: validar o email em tempo real, mostrar mensagens de erro.
        private void txtEmail_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

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
        private void txtSenha_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!senhaVisivel)
                txtSenhaVisivel.Text = txtSenha.Password;
        }

        private void txtSenhaVisivel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (senhaVisivel)
                txtSenha.Password = txtSenhaVisivel.Text;
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
    }
}
