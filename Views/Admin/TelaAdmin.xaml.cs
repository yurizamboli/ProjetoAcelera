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
    public partial class TelaAdmin : Page
    {
        private UsuarioService usuarioService;
        private AdminService adminService;
        private PublicacaoService publicacaoService;
        public TelaAdmin()
        {
            InitializeComponent();

            usuarioService = App.UsuarioService;
            adminService = new AdminService(usuarioService);
            publicacaoService = new PublicacaoService();

            CarregarDados();
            CarregarPublicacoesPendentes();
        }

        private void CarregarDados()
        {
            var user = usuarioService.UsuarioLogado;

            if (user == null)
            {
                MessageBox.Show("Nenhum usuário logado.");
                return;
            }

            txtNome.Text = user.Nome;

            if (user.Perfil != null)
            {
                txtBio.Text = user.Perfil.Bio;
            }
            else
            {
                txtBio.Text = "";
            }

            // LISTA DE USUÁRIOS (SEM O ADMIN LOGADO)
            var lista = usuarioService.ObterTodos()
                .Where(u => u.Email != user.Email)
                .ToList();

            comboUsuarios.ItemsSource = lista;
            comboUsuarios.DisplayMemberPath = "NomeCompleto";

            comboPromover.ItemsSource = lista;
            comboPromover.DisplayMemberPath = "NomeCompleto";

            comboBanir.ItemsSource = lista;
            comboBanir.DisplayMemberPath = "NomeCompleto";

            // FOTO
            try
            {
                if (user.Perfil != null &&
                    !string.IsNullOrEmpty(user.Perfil.FotoPerfil))
                {
                    imgPerfil.Source =
                        new BitmapImage(new Uri(user.Perfil.FotoPerfil));
                }
            }
            catch
            {

            }
        }

        private void AdicionarEvento_Click(object sender, RoutedEventArgs e)
        {
            string titulo = txtTituloEvento.Text;
            string descricao = txtDescEvento.Text;

            if (dateEvento.SelectedDate == null)
            {
                MessageBox.Show("Selecione uma data.");
                return;
            }

            // ainda não ligado com EventoService
            MessageBox.Show("Evento criado!");
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

            MessageBox.Show("Promovido!");
        }

        private void Banir_Click(object sender, RoutedEventArgs e)
        {
            var user = comboBanir.SelectedItem as Usuario;

            if (user == null)
            {
                MessageBox.Show("Selecione um usuário.");
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

        private void Voltar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TelaHome());
        }

        private void CarregarPublicacoesPendentes()
        {
            listaPostagensPendentes.ItemsSource = null;
            listaPostagensPendentes.ItemsSource = publicacaoService.ObterPendentes();
        }

        private void AprovarPostagem_Click(object sender, RoutedEventArgs e)
        {
            var botao = sender as Button;
            var postagem = botao?.Tag as Publicacao;
            if (postagem == null) return;

            publicacaoService.AprovarPublicacao(postagem.Id);
            CarregarPublicacoesPendentes();
            MessageBox.Show("Postagem aprovada com sucesso.");
        }

        private void ReprovarPostagem_Click(object sender, RoutedEventArgs e)
        {
            var botao = sender as Button;
            var postagem = botao?.Tag as Publicacao;
            if (postagem == null) return;

            publicacaoService.ReprovarPublicacao(postagem.Id);
            CarregarPublicacoesPendentes();
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

            JanelaImagemFull janela = new JanelaImagemFull(publicacao.ImagemUrl);
            janela.ShowDialog();
        }


    }
}