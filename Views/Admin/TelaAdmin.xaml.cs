using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using ProjetoAcelera.Views.Home;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace ProjetoAcelera.Views.Admin
{
    public partial class TelaAdmin : Page
    {
        private UsuarioService usuarioService;
        private AdminService adminService;
        private PostagemService postagemService;

        public TelaAdmin()
        {
            InitializeComponent();

            usuarioService = App.UsuarioService;
            adminService = new AdminService(usuarioService);
            postagemService = App.PostagemService;

            CarregarDados();
            CarregarPostagensPendentes();
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

        private void CarregarPostagensPendentes()
        {
            listaPostagensPendentes.ItemsSource = null;
            listaPostagensPendentes.ItemsSource = postagemService.ObterPendentes();
        }

        private void AprovarPostagem_Click(object sender, RoutedEventArgs e)
        {
            var botao = sender as System.Windows.Controls.Button;
            var postagem = botao?.Tag as Postagem;
            if (postagem == null) return;

            postagemService.AprovarPostagem(postagem.Id);
            CarregarPostagensPendentes();
            MessageBox.Show("Postagem aprovada com sucesso.");
        }

        private void ReprovarPostagem_Click(object sender, RoutedEventArgs e)
        {
            var botao = sender as System.Windows.Controls.Button;
            var postagem = botao?.Tag as Postagem;
            if (postagem == null) return;

            postagemService.ReprovarPostagem(postagem.Id);
            CarregarPostagensPendentes();
            MessageBox.Show("Postagem rejeitada.");
        }

    }
}