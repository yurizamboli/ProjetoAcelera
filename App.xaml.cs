using System.Windows;
using ProjetoAcelera.Services;

namespace ProjetoAcelera
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ArquivoService ArquivoService { get; private set; } = default!;
        public static UsuarioService UsuarioService { get; private set; } = default!;
        public static EmailService EmailService { get; private set; } = default!;
        public static EventoService EventoService { get; private set; } = default!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ArquivoService = new ArquivoService();
            UsuarioService = new UsuarioService();
            EmailService = new EmailService();
            EventoService = new EventoService(ArquivoService);

            var mainWindow = new Views.MainWindow.TelaMainWindow();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {

            ArquivoService.SalvarUsuariosComImagens(UsuarioService.ObterTodos());
            EventoService.SalvarEventos();
            base.OnExit(e);
        }
    }
}