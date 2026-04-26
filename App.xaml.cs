using System.Windows;
using ProjetoAcelera.Services;

namespace ProjetoAcelera
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UsuarioService UsuarioService;
        public static EmailService EmailService { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            UsuarioService = new UsuarioService();
            EmailService = new EmailService();

            var login = new Views.LoginRegistro.TelaLoginRegistro();
            login.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            ArquivoService arquivo = new ArquivoService();
            arquivo.SalvarUsuariosComImagens(UsuarioService.ObterTodos());
        }
    }
}