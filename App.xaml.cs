using System.Windows;
using ProjetoAcelera.Services;

namespace ProjetoAcelera
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UsuarioService UsuarioService { get; private set; } = default!;
        public static EmailService EmailService { get; private set; } = default!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            UsuarioService = new UsuarioService();
            EmailService = new EmailService();

            var mainWindow = new Views.MainWindow.TelaMainWindow();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            ArquivoService arquivo = new ArquivoService();
            arquivo.SalvarUsuariosComImagens(UsuarioService.ObterTodos());
        }
    }
}