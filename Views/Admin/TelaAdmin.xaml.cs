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
    public partial class TelaAdmin : Page
    {
        private UsuarioService usuarioService;
        private AdminService adminService;
        private PublicacaoService publicacaoService;
        private EventoService eventoService;
        private string caminhoImagemEvento;

        public TelaAdmin()
        {
            InitializeComponent();

            usuarioService = App.UsuarioService;
            adminService = new AdminService(usuarioService);
            publicacaoService = new PublicacaoService();
            eventoService = new EventoService();

            CarregarDados();
            CarregarEventos();
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

            // LISTA DE USUÁRIOS
            var lista = usuarioService.ObterTodos().Where(u => u.Email != user.Email && !u.Banido).ToList();

            comboUsuarios.ItemsSource = null;
            comboUsuarios.ItemsSource = lista;
            comboUsuarios.DisplayMemberPath = "NomeCompleto";

            comboPromover.ItemsSource = null;
            comboPromover.ItemsSource = lista;
            comboPromover.DisplayMemberPath = "NomeCompleto";

            comboBanir.ItemsSource = null;
            comboBanir.ItemsSource = lista;
            comboBanir.DisplayMemberPath = "NomeCompleto";

            // LISTA DE DESTAQUES
            listaDestaques.ItemsSource = null;

            listaDestaques.ItemsSource = usuarioService.ObterTodos().Where(u => u.Perfil != null && u.Perfil.Destaque && !u.Banido).ToList();

            //Lista Banidos
            listaBanidos.ItemsSource = null;

            listaBanidos.ItemsSource = usuarioService.ObterTodos().Where(u => u.Banido).ToList();
            string caminhoPadrao = "pack://application:,,,/ImagemAcelera/AvatarPadrao.png";
            // FOTO
            try
            {
                if (user.Perfil != null && !string.IsNullOrEmpty(user.Perfil.FotoPerfil))
                {
                    imgPerfil.Source =AuxilioImagens.CarregarImgOtimizada(user.Perfil.FotoPerfil,250);
                }        
                else
            {
                imgPerfil.Source = AuxilioImagens.CarregarImgOtimizada(caminhoPadrao,250);
            }
        }
            catch
            {
                imgPerfil.Source = AuxilioImagens.CarregarImgOtimizada(caminhoPadrao,250);
            }
            
        }

        private void CarregarEventos()
        {
            listaEventos.ItemsSource = null;
            listaEventos.ItemsSource = eventoService.ObterEvento();

            comboEventosProgramacao.ItemsSource = null;
            comboEventosProgramacao.ItemsSource = eventoService.ObterEvento();
            comboEventosProgramacao.DisplayMemberPath = "Titulo";
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

            if (string.IsNullOrWhiteSpace(titulo))
            {
                MessageBox.Show("Digite o título do evento.");
                return;
            }

            if (dateEvento.SelectedDate == null)
            {
                MessageBox.Show("Selecione uma data.");
                return;
            }

            if (string.IsNullOrWhiteSpace(caminhoImagemEvento))
            {
                MessageBox.Show("Selecione uma imagem para o evento.");
                return;
            }

            string data = dateEvento.SelectedDate.Value.ToString("dd/MM/yyyy");

            eventoService.AdicionarEventos(
                titulo,
                data,
                descricao,
                "",
                caminhoImagemEvento);

            MessageBox.Show("Evento criado com sucesso!");

            txtTituloEvento.Clear();
            txtDescEvento.Clear();
            dateEvento.SelectedDate = null;
            caminhoImagemEvento = null;
            txtCaminhoImagem.Text = "Selecione uma imagem...";

            CarregarEventos();
        }

        private void EditarEvento_Click(object sender, RoutedEventArgs e)
        {
            var evento = listaEventos.SelectedItem as Evento;

            if (evento == null)
            {
                MessageBox.Show("Selecione um evento para editar.");
                return;
            }

            txtTituloEvento.Text = evento.Titulo;
            txtDescEvento.Text = evento.Descricao;
            caminhoImagemEvento = evento.Imagem;

            txtCaminhoImagem.Text =
                System.IO.Path.GetFileName(evento.Imagem);

            try
            {
                dateEvento.SelectedDate =
                    DateTime.ParseExact(
                        evento.Data,
                        "dd/MM/yyyy",
                        null);
            }
            catch
            {

            }
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
                MessageBoxButton.YesNo);

            if (confirm == MessageBoxResult.Yes)
            {
                var eventos = eventoService.ObterEvento();

                eventos.Remove(evento);

                MessageBox.Show("Evento deletado com sucesso!");

                CarregarEventos();
            }
        }

        private void AdicionarProgramacao_Click(object sender, RoutedEventArgs e)
        {
            var evento = comboEventosProgramacao.SelectedItem as Evento;

            if (evento == null)
            {
                MessageBox.Show("Selecione um evento.");
                return;
            }

            if (dateProgramacao.SelectedDate == null)
            {
                MessageBox.Show("Selecione uma data.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNomeProgramacao.Text))
            {
                MessageBox.Show("Digite o nome da atividade.");
                return;
            }

            MessageBox.Show(
                $"Programação '{txtNomeProgramacao.Text}' adicionada ao evento '{evento.Titulo}'");

            txtNomeProgramacao.Clear();
            txtDescProgramacao.Clear();
            dateProgramacao.SelectedDate = null;
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
        private void Desbanir_Click(object sender, RoutedEventArgs e)
        {
            Button botao = (Button)sender;

            Usuario usuario = (Usuario)botao.Tag;

            if (usuario == null)
            {
                return;
            }

            usuario.Banido = false;

            MessageBox.Show("Usuário desbanido!");

            CarregarDados();
        }
    }
}