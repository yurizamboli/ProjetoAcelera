using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using ProjetoAcelera.Views.Teste;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ProjetoAcelera.Views.Calendario;
using System.Windows.Media.Imaging;

namespace ProjetoAcelera.Views.Artistas
{
    public partial class TelaArtista : Window
    {
        private UsuarioService usuarioService;
        private List<Usuario> listaCompleta;
        //Vai aparecer 10 artistas por vez, e quando clicar no botão "Ver mais" vai aparecer mais 10 pra não ferrar com a performance
        private int quantidadeAtual = 10;

        public TelaArtista()
        {
            InitializeComponent();
            txtBusca.Text = "Buscar nome do artista...";
            txtBusca.TextChanged += TxtBusca_TextChanged;

            usuarioService = App.UsuarioService;
            listaCompleta = usuarioService.ObterTodos();

            CarregarArtistas();
        }

        private void CarregarArtistas()
        {
            painelArtistas.Children.Clear();

            var lista = listaCompleta.Take(quantidadeAtual);
            //Adiciona os cards dos artistas no painel
            foreach (var user in lista)
            {
                painelArtistas.Children.Add(CriarCard(user));
            }
        }

        private Border CriarCard(Usuario user)
        {
            StackPanel container = new StackPanel
            {
                Margin = new Thickness(10),
                Width = 100
            };

            Image img = new Image
            {
                Width = 90,
                Height = 90,
                Stretch = Stretch.UniformToFill
            };

            try
            {
                img.Source = new BitmapImage(new System.Uri(user.Perfil.FotoPerfil));
            }
            catch 
            { 
            
            }

            Border foto = new Border
            {
                Width = 90,
                Height = 90,
                //Se quiser deixar as fotinhos redondas só mudar o CornerRadius pra 45
                CornerRadius = new CornerRadius(0),
                ClipToBounds = true,
                Child = img,
                Background = Brushes.LightGray
            };

            TextBlock nome = new TextBlock
            {
                Text = user.Nome,
                Foreground = Brushes.White,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 12
            };

            container.Children.Add(foto);
            container.Children.Add(nome);

            Border card = new Border
            {
                Child = container,
                Cursor = System.Windows.Input.Cursors.Hand
            };

            card.MouseDown += (s, e) =>
            {
                var tela = new Views.Perfil.TelaPerfilVisual(user);
                tela.ShowDialog();
            };

            return card;
        }

        private void TxtBusca_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (painelArtistas == null || listaCompleta == null) return;
            
            //antes tava só "string texto = txtBusca.Text.ToLower();". isso tava fazendo o placeholder ser usado na hora da busca
            string texto = txtBusca.Text;

            if (texto == "Buscar nome do artista...")
                texto = "";

            texto = texto.ToLower();
            painelArtistas.Children.Clear();

            var filtrados = listaCompleta
                .Where(u => u.Nome.ToLower().Contains(texto))
                .Take(quantidadeAtual);

            foreach (var user in filtrados)
            {
                painelArtistas.Children.Add(CriarCard(user));
            }
        }

        private void BtnVerMais_Click(object sender, RoutedEventArgs e)
        {
            quantidadeAtual += 10;
            CarregarArtistas();
        }
        private void TxtBusca_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBusca.Text == "Buscar nome do artista...")
            {
                txtBusca.Text = "";
                txtBusca.Foreground = Brushes.Black;
            }
        }
        private void TxtBusca_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBusca.Text))
            {
                txtBusca.Text = "Buscar nome do artista...";
                txtBusca.Foreground = Brushes.Gray;
            }
        }

        private void Conta_Click(object sender, RoutedEventArgs e)
        {
            var tela = new Views.Perfil.TelaPerfil(App.UsuarioService);
            tela.Show();
            this.Close();
        }

        private void NossaCidade_Click(object sender, RoutedEventArgs e)
        {
            var tela = new Dashboard();
            tela.Show();
            this.Close();
        }

        private void Programacao_Click(object sender, RoutedEventArgs e)
        {
            var tela = new Views.Calendario.Calendario();
            tela.Show();
            this.Close();
        }
    }
}