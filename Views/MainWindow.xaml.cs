using ProjetoAcelera.Services;
using System.Windows;

namespace ProjetoAcelera.Views
{
    public partial class MainWindow : Window
    {
        private UsuarioService usuarioService;

        public MainWindow()
        {
            InitializeComponent();
            usuarioService = new UsuarioService();
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
            Cadastro tela = new Cadastro(usuarioService);
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