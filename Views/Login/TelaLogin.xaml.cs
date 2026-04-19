using ProjetoAcelera.Services;
using ProjetoAcelera.Views.Cadastro;
using ProjetoAcelera.Views.Obras;
using ProjetoAcelera.Views.Perfil;
using ProjetoAcelera.Views.Teste;
using System.Windows;

namespace ProjetoAcelera.Views.Login
{
    public partial class TelaLogin : Window
    {
        private UsuarioService usuarioService;

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
        //LEMBRAR DE TIRAR ESSA PARADA AQ
        private void BtnTesteObras_Click(object sender, RoutedEventArgs e)
        {
            TelaObras tela = new TelaObras();
            tela.Show();
        }

        private void BtnTesteFormatacao_Click(object sender, RoutedEventArgs e)
        {
            TelaExtra tela = new TelaExtra();
            tela.Show();
        }
    }
}