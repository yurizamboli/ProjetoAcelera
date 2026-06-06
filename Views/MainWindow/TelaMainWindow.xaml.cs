using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ProjetoAcelera.Views.MainWindow
{
    public partial class TelaMainWindow : Window
    {
        public TelaMainWindow()
        {
            InitializeComponent();
            AtualizarNavbar();
            Navegar(new Views.Home.TelaHome());
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
            Navegar(new Views.Home.TelaHome());
        }

        private void Programacao_Click(object sender, RoutedEventArgs e) {
            Navegar(new Views.Calendario.Calendario());
        }
        private void Cultura_Click(object sender, RoutedEventArgs e) 
        {
            Navegar(new Views.Cultura.TelaCultura());
        }
        private void Artistas_Click(object sender, RoutedEventArgs e) 
        {
            Navegar(new Views.Artistas.TelaArtista());
        }
        private void NossaCidade_Click(object sender, RoutedEventArgs e)
        {
            Navegar(new Views.Teste.Dashboard());
        }       
        private void Conta_Click(object sender, RoutedEventArgs e)
        {
                Navegar(new Views.Perfil.TelaPerfil());
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Navegar(new Views.LoginRegistro.TelaLoginRegistro());
        }

        private void Fechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
       
        private void SwitchFrame_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (FecharComboBoxAberto())
            {
                e.Handled = true;
                return;
            }
            double velocidadeScroll = 3.0;
            ScrollPrincipal.ScrollToVerticalOffset(ScrollPrincipal.VerticalOffset - (e.Delta / velocidadeScroll));
            e.Handled = true;
        }
        private bool FecharComboBoxAberto()
        {
            ComboBox comboAberto = ProcurarComboBoxAberto(SwitchFrame);

            if (comboAberto != null)
            {
                comboAberto.IsDropDownOpen = false;
                return true;
            }

            return false;
        }

        private ComboBox ProcurarComboBoxAberto(DependencyObject elemento)
        {
            if (elemento == null)
            {
                return null;
            }

            if (elemento is ComboBox combo && combo.IsDropDownOpen)
            {
                return combo;
            }

            int totalFilhos = 0;

            try
            {
                totalFilhos = VisualTreeHelper.GetChildrenCount(elemento);
            }
            catch
            {
                return null;
            }

            for (int i = 0; i < totalFilhos; i++)
            {
                DependencyObject filho = VisualTreeHelper.GetChild(elemento, i);

                ComboBox resultado = ProcurarComboBoxAberto(filho);

                if (resultado != null)
                {
                    return resultado;
                }
            }

            return null;
        }
        private void Navegar(Page pagina)
        {
            SwitchFrame.Navigate(pagina);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                ScrollPrincipal.ScrollToVerticalOffset(0);
                ScrollPrincipal.ScrollToTop();

            }), DispatcherPriority.ContextIdle);
        }
    }
}
