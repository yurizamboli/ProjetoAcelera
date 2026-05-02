using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using ProjetoAcelera.Views.Home;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProjetoAcelera.Views.Admin
{
    public partial class TelaAdmin : Window
    {
        private UsuarioService usuarioService;
        private AdminService adminService;

        public TelaAdmin()
        {
            InitializeComponent();

            usuarioService = App.UsuarioService;
            adminService = new AdminService(usuarioService);

            CarregarDados();
        }

        private void CarregarDados()
        {
            var user = usuarioService.UsuarioLogado;

            if (user == null)
            {
                MessageBox.Show("Nenhum usuário logado.");
                this.Close();
                return;
            }
            txtNome.Text = user.Nome;

            if (user.Perfil != null)
            {
                
                txtNome.Text = user.Nome;
            }
            else
            {
                txtBio.Text = "";
                
            }


            // CARREGAR LISTA DE USUÁRIOS PARA COMBOBOX, EXCLUINDO O USUÁRIO LOGADO
            var lista = usuarioService.ObterTodos()
            .Where(u => u.Email != user.Email).ToList();

            comboUsuarios.ItemsSource = lista;
            comboUsuarios.DisplayMemberPath = "Nome";

            comboPromover.ItemsSource = lista;
            comboPromover.DisplayMemberPath = "Nome";

            comboRemover.ItemsSource = lista;
            comboRemover.DisplayMemberPath = "Nome";

            // foto
            try
            {
                if (user.Perfil != null && !string.IsNullOrEmpty(user.Perfil.FotoPerfil))
                {
                    imgPerfil.Source = new BitmapImage(new Uri(user.Perfil.FotoPerfil));
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

            //Nao ta ligado com evento
            MessageBox.Show("Evento criado)");
        }

        private void Destaque_Click(object sender, RoutedEventArgs e)
        {
            //ainda não tem tela de destaque, então só vai mostrar a mensagem
            var user = comboUsuarios.SelectedItem as Usuario;

            if (user != null)
            {
                adminService.TornarDestaque(user.Nome);
                MessageBox.Show("Usuário em destaque!");
            }
        }

        private void Promover_Click(object sender, RoutedEventArgs e)
        {
            var user = comboPromover.SelectedItem as Usuario;

            if (user != null)
            {
                adminService.PromoverParaAdmin(user.Nome);
                MessageBox.Show("Promovido!");
            }
        }

        private void Voltar_Click(object sender, RoutedEventArgs e)
        {
            new TelaHome().Show();
            this.Close();
        }

        private void Remover_Click(object sender, RoutedEventArgs e)
        {
            var user = comboRemover.SelectedItem as Usuario;

            if (user == null)
            {
                MessageBox.Show("Selecione um usuário.");
                return;
            }

            string mensagem = string.Format("Deseja remover {0}?", user.Nome);

            var confirm = MessageBox.Show(
                mensagem,
                "Confirmar",
                MessageBoxButton.YesNo
            );

            if (confirm == MessageBoxResult.Yes)
            {
                adminService.RemoverUsuario(user.Nome);
                MessageBox.Show("Usuário removido!");

                CarregarDados();
            }
        }
        }
    }