using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using ProjetoAcelera.Views.EditarObras;
using ProjetoAcelera.Views.PopUpObras;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjetoAcelera.Views.Obras
{
    public partial class TelaObras : Window
    {
        private UsuarioService usuarioService;

        public TelaObras(UsuarioService service)
        {
            InitializeComponent();
            usuarioService = service;

            CarregarUsuario();
            CarregarObras();
        }
        //OBRAS
        private void CarregarObras()
        {
            var usuario = usuarioService.UsuarioLogado;

            if (usuario == null) return;

            painelObras.Children.Clear();

            painelObras.Children.Add(CriarBotaoAdicionar());

            foreach (var obra in usuario.Obras)
            {
                painelObras.Children.Add(CriarCardObra(obra));
            }
        }
        //NOMEZINHO LA NA CONTA
        private void CarregarUsuario()
        {
            var usuario = usuarioService.UsuarioLogado;

            if (usuario == null) return;

            txtNome.Text = usuario.Nome;
        }

        private Border CriarBotaoAdicionar()
        {
            Border border = new Border
            {
                Width = 120,
                Height = 160,
                Margin = new Thickness(5),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EAEAEA")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(10)
            };

            border.MouseDown += AdicionarObra_Click;

            border.Child = new TextBlock
            {
                Text = "+",
                FontSize = 40,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            return border;
        }

        private Border CriarCardObra(Obra obra)
        {
            StackPanel container = new StackPanel();

            // IMAGEM
            Image img = new Image
            {
                Height = 120,
                Stretch = Stretch.UniformToFill
            };

            try
            {
                img.Source = new BitmapImage(new Uri(obra.Capa));
            }
            catch { }

            // TÍTULO
            TextBlock titulo = new TextBlock
            {
                Text = obra.Titulo,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 12
            };

            // BOTÃO FAVORITO, ARRUMAR ISSO AQUI DEPOIS, AS ESTRELAS TÃO MUITO FEIAS, MAS FUNCIONA
            Button favBtn = new Button
            {
                Content = obra.Favorito ? "⭐" : "☆",
                Width = 30,
                Height = 25,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            favBtn.Click += (s, e) =>
            {
                var service = new ObraService(App.UsuarioService);
                service.FavoritarObra(obra.Titulo);

                CarregarObras();
            };

            // BOTÃO DE REMOVER ACHAR UM EMOJI BONITINHO E DEIXAR IGUAL O DE FAVORITAR
            Button delBtn = new Button
            {
                Content = "Deletar",
                Width = 70,
                Height = 30,
                Background = Brushes.LightCoral,
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                Cursor = System.Windows.Input.Cursors.Hand,
                FontSize = 12
            };

            delBtn.Click += (s, e) =>
            {
                var service = new ObraService(App.UsuarioService);
                service.RemoverObra(obra.Titulo);

                CarregarObras();
            };

            // BOTÕES (FAVORITO + DELETE)
            StackPanel actions = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            actions.Children.Add(favBtn);
            actions.Children.Add(delBtn);

            // MONTAGEM FINAL
            container.Children.Add(img);
            container.Children.Add(titulo);
            container.Children.Add(actions);

            Border border = new Border
            {
                Width = 120,
                Height = 160,
                Margin = new Thickness(5),
                CornerRadius = new CornerRadius(10),
                BorderBrush = Brushes.DarkBlue,
                BorderThickness = new Thickness(2),
                Child = container
            };

            border.MouseDown += (s, e) => AbrirDetalhesObra(obra);

            return border;
        }

        private void AdicionarObra_Click(object sender, MouseButtonEventArgs e)
        {
            var tela = new TelaAdicionarObra();
            tela.ShowDialog();

            CarregarObras();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Salvo Sim");
        }

        private void AbrirDetalhesObra(Obra obra)
        {
            var tela = new ObrasPopUp(obra);
            tela.ShowDialog();

            CarregarObras();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void txtNome_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }
}