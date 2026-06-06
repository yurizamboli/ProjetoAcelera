using Microsoft.Win32;
using ProjetoAcelera.Ferramentas;
using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using ProjetoAcelera.Views.Home;
using ProjetoAcelera.Views.LoginRegistro;
using ProjetoAcelera.Views.MainWindow;
using ProjetoAcelera.Views.Perfil;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ProjetoAcelera.Views.Admin
{
    public partial class ContainerPainelAdmin : Page
    {
        private UsuarioService usuarioService;
        private AdminService adminService;
        private PublicacaoService publicacaoService;
        private EventoService eventoService;
        private string caminhoImagemEvento;

        public ContainerPainelAdmin()
        {
            InitializeComponent();

            usuarioService = App.UsuarioService;
            adminService = new AdminService(usuarioService);
            publicacaoService = new PublicacaoService();
            eventoService =App.EventoService;

            CarregarDados();
            CarregarEventos();
            CarregarPublicacoesPendentes();
            CarregarPublicacoesPublicadas();
        }

        private void CarregarDados()
        {
            var user = usuarioService.UsuarioLogado;

            if (user == null)
            {
                MessageBox.Show("Nenhum usuário logado.");
                return;
            }

            // LISTA DE USUÁRIOS
            var listaUsuarios = usuarioService.ObterTodos().Where(u => u.Email != user.Email && !u.Banido).ToList();
            var listaParaPromover = usuarioService.ObterTodos().Where(u => u.Email != user.Email && u.Cargo != "Admin" && !u.Banido).ToList();
            var listaParaBanir = usuarioService.ObterTodos().Where(u => u.Email != user.Email && !u.Banido && !u.AdminPrincipal).ToList();

            comboUsuarios.ItemsSource = null;
            comboUsuarios.ItemsSource = listaUsuarios;
            comboUsuarios.DisplayMemberPath = "NomeCompleto";

            comboPromover.ItemsSource = null;
            comboPromover.ItemsSource = listaParaPromover;
            comboPromover.DisplayMemberPath = "NomeCompleto";

            comboBanir.ItemsSource = null;
            comboBanir.ItemsSource = listaParaBanir;
            comboBanir.DisplayMemberPath = "NomeCompleto";

            // LISTA DE DESTAQUES
            listaDestaques.ItemsSource = null;
            listaDestaques.ItemsSource = usuarioService.ObterTodos().Where(u => u.Perfil != null && u.Perfil.Destaque && !u.Banido).ToList();

            // LISTA DE ADMINS PROMOVIDOS
            listaAdmins.ItemsSource = null;
            var admins = usuarioService.ObterTodos().Where(u => u.Cargo == "Admin" && !u.AdminPrincipal && !u.Banido).ToList();
            listaAdmins.ItemsSource = admins;

            // LISTA DE USUÁRIOS BANIDOS
            listaBanidos.ItemsSource = null;
            var banidos = usuarioService.ObterTodos().Where(u => u.Banido && !u.AdminPrincipal).ToList();
            listaBanidos.ItemsSource = banidos;
        }

        private void CarregarEventos()
        {
            listaEventos.ItemsSource = null;
            listaEventos.ItemsSource = eventoService.ObterEvento();
        }

        private void SelecionarImagemEvento_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Imagens|*.jpg;*.jpeg;*.png;*.gif"
            };

            if (dialog.ShowDialog() == true)
            {
                caminhoImagemEvento = dialog.FileName;
                txtCaminhoImagem.Text = System.IO.Path.GetFileName(caminhoImagemEvento);
            }
        }

        private void AdicionarEvento_Click(object sender, RoutedEventArgs e)
        {
            string titulo = txtTituloEvento.Text;
            string descricao = txtDescEvento.Text;
            string programacao = txtProgramacaoEvento.Text;
            string local = txtLocalEvento.Text;
            bool destaque = checkEventoDestaque.IsChecked == true;

            // VALIDAÇÕES
            if (string.IsNullOrWhiteSpace(titulo))
            {
                MessageBox.Show("Digite o título do evento.");
                return;
            }

            if (string.IsNullOrWhiteSpace(descricao))
            {
                MessageBox.Show("Digite a descrição do evento.");
                return;
            }

            if (string.IsNullOrWhiteSpace(programacao))
            {
                MessageBox.Show("Digite a programação ou os detalhes do evento.");
                return;
            }

            if (string.IsNullOrWhiteSpace(local))
            {
                MessageBox.Show("Digite o local do evento.");
                return;
            }

            if (dateEventoInicio.SelectedDate == null)
            {
                MessageBox.Show("Selecione a data de início.");
                return;
            }

            if (dateEventoFim.SelectedDate != null && dateEventoFim.SelectedDate.Value.Date < dateEventoInicio.SelectedDate.Value.Date)
            {
                MessageBox.Show("A data de fim não pode ser anterior à data de início.");
                return;
            }

            if (string.IsNullOrWhiteSpace(caminhoImagemEvento))
            {
                MessageBox.Show("Selecione uma imagem para o evento.");
                return;
            }

            eventoService.AdicionarEvento( titulo, dateEventoInicio.SelectedDate.Value, dateEventoFim.SelectedDate, descricao, programacao, local, caminhoImagemEvento, destaque);

            MessageBox.Show("Evento criado com sucesso!");

            txtTituloEvento.Clear();
            txtDescEvento.Clear();
            txtProgramacaoEvento.Clear();
            txtLocalEvento.Clear();

            dateEventoInicio.SelectedDate = null;
            dateEventoFim.SelectedDate = null;

            checkEventoDestaque.IsChecked = true;
            caminhoImagemEvento = null;
            txtCaminhoImagem.Text = "Selecione uma imagem...";

            CarregarEventos();
        }

        private void DeletarEvento_Click(object sender, RoutedEventArgs e)
        {
            var evento = listaEventos.SelectedItem as Evento;

            if (evento == null)
            {
                MessageBox.Show("Selecione um evento para deletar.");
                return;
            }

            var confirm = MessageBox.Show(
                $"Deseja deletar o evento '{evento.Titulo}'?",
                "Confirmar",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes)
            {
                return;
            }

            eventoService.RemoverEvento(evento.Id);

            MessageBox.Show("Evento deletado com sucesso!");

            CarregarEventos();
        }

        private void Destaque_Click(object sender, RoutedEventArgs e)
        {
            var user = comboUsuarios.SelectedItem as Usuario;

            if (user == null)
            {
                MessageBox.Show("Selecione um usuário.");
                return;
            }

            adminService.TornarDestaque(user.Email);

            MessageBox.Show("Usuário em destaque!");

            CarregarDados();
        }

        private void Promover_Click(object sender, RoutedEventArgs e)
        {
            var user = comboPromover.SelectedItem as Usuario;

            if (user == null)
            {
                MessageBox.Show("Selecione um usuário.");
                return;
            }

            adminService.PromoverParaAdmin(user.Email);
            user.AdminPrincipal = false;
            MessageBox.Show("Promovido!");
            CarregarDados();
        }

        private void Banir_Click(object sender, RoutedEventArgs e)
        {
            var user = comboBanir.SelectedItem as Usuario;

            if (user == null)
            {
                MessageBox.Show("Selecione um usuário.");
                return;
            }
            if (user.AdminPrincipal)
            {
                MessageBox.Show("O administrador principal não pode ser banido.");
                return;
            }
            string mensagem =
                "Deseja banir o usuário " + user.Nome + "?";

            var confirm = MessageBox.Show(
                mensagem,
                "Confirmar",
                MessageBoxButton.YesNo
            );

            if (confirm == MessageBoxResult.Yes)
            {
                adminService.BanirUsuario(user.Email);

                MessageBox.Show("Usuário banido!");

                CarregarDados();
            }
        }
        private void RemoverBanimento_Click(object sender, RoutedEventArgs e)
        {
            Button botao = (Button)sender;

            Usuario usuario = (Usuario)botao.Tag;

            if (usuario == null)
            {
                return;
            }

            var confirm = MessageBox.Show(
                "Deseja retirar o banimento de " + usuario.Nome + "?",
                "Confirmar",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes)
            {
                return;
            }

            usuario.Banido = false;

            MessageBox.Show("Banimento removido.");

            CarregarDados();
        }
        private void Voltar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TelaHome());
        }

        private void CarregarPublicacoesPendentes()
        {
            listaPostagensPendentes.ItemsSource = null;
            listaPostagensPendentes.ItemsSource =
                publicacaoService.ObterPendentes();
        }

        private void AprovarPostagem_Click(object sender, RoutedEventArgs e)
        {
            var botao = sender as Button;
            var postagem = botao?.Tag as Publicacao;

            if (postagem == null)
                return;

            publicacaoService.AprovarPublicacao(postagem.Id);

            CarregarPublicacoesPendentes();
            CarregarPublicacoesPublicadas();

            MessageBox.Show("Postagem aprovada com sucesso.");
        }

        private void ReprovarPostagem_Click(object sender, RoutedEventArgs e)
        {
            var botao = sender as Button;
            var postagem = botao?.Tag as Publicacao;

            if (postagem == null)
                return;

            publicacaoService.ReprovarPublicacao(postagem.Id);

            CarregarPublicacoesPendentes();
            CarregarPublicacoesPublicadas();

            MessageBox.Show("Postagem rejeitada.");
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Deseja realmente sair?",
                "Logout",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            App.UsuarioService.Logout();

            var main =
                Application.Current.MainWindow
                as TelaMainWindow;

            main?.AtualizarNavbar();

            NavigationService.Navigate(
                new TelaLoginRegistro());
        }

        private void AbrirImagem_Click(object sender, MouseButtonEventArgs e)
        {
            var imagem = sender as Image;

            if (imagem == null)
            {
                return;
            }

            var publicacao = imagem.DataContext as Publicacao;

            if (publicacao == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(publicacao.ImagemUrl))
            {
                return;
            }

            JanelaImagemFull janela =
                new JanelaImagemFull(publicacao.ImagemUrl);

            janela.ShowDialog();
        }

        private void RemoverDestaque_Click(object sender, RoutedEventArgs e)
        {
            Button botao = (Button)sender;

            Usuario usuario = (Usuario)botao.Tag;

            if (usuario == null)
            {
                return;
            }

            if (usuario.Perfil != null)
            {
                usuario.Perfil.Destaque = false;
            }

            MessageBox.Show("Usuário removido dos destaques.");

            CarregarDados();
        }
        private void RemoverAdmin_Click(object sender, RoutedEventArgs e)
        {
            Button botao = (Button)sender;
            Usuario usuario = (Usuario)botao.Tag;

            if (usuario == null)
            {
                return;
            }
            if (usuario.AdminPrincipal)
            {
                MessageBox.Show("O administrador principal não pode ser removido.");
                return;
            }

            var confirm = MessageBox.Show(
                "Deseja remover o acesso de administrador de " + usuario.Nome + "?",
                "Confirmar",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes)
            {
                return;
            }

            usuario.Cargo = "Usuario";
            usuario.AdminPrincipal = false;
            MessageBox.Show("Usuário voltou a ser usuário comum.");
            CarregarDados();
        }
        private void CarregarPublicacoesPublicadas()
        {
            listaPostagensPublicadas.ItemsSource = null;
            listaPostagensPublicadas.ItemsSource =
            publicacaoService.ObterPublicadasNaHome();
        }
        private void RemoverDaHome_Click(object sender, RoutedEventArgs e)
        {
            var botao = sender as Button;
            var postagem = botao?.Tag as Publicacao;

            if (postagem == null)
                return;

            var confirm = MessageBox.Show(
                "Deseja remover esta publicação da Home?",
                "Confirmar remoção",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes)
                return;

            publicacaoService.RemoverDaHome(postagem.Id);

            CarregarPublicacoesPendentes();
            CarregarPublicacoesPublicadas();

            MessageBox.Show("Publicação removida da Home.");
        }
        private void ListaEventosItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListBoxItem;
            if (item == null)
            {
                return;
            }
            if (item.IsSelected)
            {
                listaEventos.SelectedItem = null;
                e.Handled = true;
            }
        }
    }
}