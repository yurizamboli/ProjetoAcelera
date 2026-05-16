using ProjetoAcelera.Ferramentas;
using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using ProjetoAcelera.Views.Calendario;
using ProjetoAcelera.Views.Perfil;
using ProjetoAcelera.Views.Teste;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjetoAcelera.Views.Artistas
{
    public partial class TelaArtista : Page
    {
        private UsuarioService usuarioService;
        private List<Usuario> listaCompleta;
 

        public TelaArtista()
        {
            InitializeComponent();
            txtBusca.Text = "Buscar nome do artista...";
            txtBusca.TextChanged += TxtBusca_TextChanged;

            usuarioService = App.UsuarioService;
            listaCompleta = usuarioService.ObterTodos().Where(u => !u.Banido).OrderByDescending(u => u.Perfil != null && u.Perfil.Destaque).ThenBy(u => u.Nome).ToList();

            CarregarArtistas();
        }

        private void CarregarArtistas()
        {
            painelArtistas.Children.Clear();

            var lista = listaCompleta;
            //Adiciona os cards dos artistas no painel
            foreach (var user in lista)
            {
                painelArtistas.Children.Add(CriarCard(user));
            }
        }

        private Border CriarCard(Usuario user)
        {
            Image img = new Image
            {
                Width = 120,
                Height = 120,
                Stretch = Stretch.UniformToFill
            };
            string caminhoPadrao = "pack://application:,,,/ImagemAcelera/AvatarPadrao.png";
            try
            {
                if (!string.IsNullOrWhiteSpace(user.Perfil?.FotoPerfil) && System.IO.File.Exists(user.Perfil.FotoPerfil))
                {
                    img.Source = AuxilioImagens.CarregarImgOtimizada(user.Perfil.FotoPerfil, 90);
                }
                else
                {
                    img.Source = AuxilioImagens.CarregarImgOtimizada(caminhoPadrao, 90);
                }
            }
            catch 
            { 
                 img.Source = AuxilioImagens.CarregarImgOtimizada(caminhoPadrao, 90);
            }

            Border foto = new Border
            {
                Width = 120,
                Height = 120,
                CornerRadius = new CornerRadius(0),
                ClipToBounds = true,
                Child = img,
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D4AF37")),
                BorderThickness = new Thickness(3),
                Margin = new Thickness(0, 0, 0, 10)
            };
       
            TextBlock nome = new TextBlock
            {
                Text = user.Nome,
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center
            };

            TextBlock destaque = new TextBlock
            {
                Foreground = Brushes.Gold,
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 6, 0, 0)
            };

            if (user.Perfil != null)
            {
                if (user.Perfil.Destaque)
                {
                    destaque.Text = "⭐ ARTISTA EM DESTAQUE";
                }
                else
                {
                    destaque.Text = "";
                }
            }
            else
            {
                destaque.Text = "";
            }

            Border linha = new Border
            {
                Width = 60,
                Height = 2,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D4AF37")),
                Margin = new Thickness(0, 8, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            TextBlock totalPosts = new TextBlock
            {
                Foreground = Brushes.LightGray,
                FontSize = 11,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 8, 0, 0)
            };

            if (user.Publicacoes != null)
            {
                int quantidade = user.Publicacoes.Count;

                if (quantidade == 1)
                {
                    totalPosts.Text = "1 publicação";
                }
                else
                {
                    totalPosts.Text = quantidade + " publicações";
                }
            }
            else
            {
                totalPosts.Text = "0 publicações";
            }
            StackPanel conteudo = new StackPanel();

            conteudo.Children.Add(foto);
            conteudo.Children.Add(nome);
            conteudo.Children.Add(linha);
            conteudo.Children.Add(destaque);
            conteudo.Children.Add(totalPosts);

            Border card = new Border
            {
                Width = 190,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#163545")),
                CornerRadius = new CornerRadius(0),
                Padding = new Thickness(18),
                Margin = new Thickness(12),
                Cursor = System.Windows.Input.Cursors.Hand,
                Child = conteudo,
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#244B5A")),
                BorderThickness = new Thickness(2)
            };

            card.MouseEnter += (s, e) =>
            {
                card.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F4A5D"));
            };

            card.MouseLeave += (s, e) =>
            {
                card.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#163545"));
            };

            card.MouseDown += (s, e) =>
            {
                NavigationService.Navigate(new TelaPerfilVisual(user));
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

            var filtrados = listaCompleta.Where(u => u.Nome.ToLower().Contains(texto)).OrderByDescending(u => u.Perfil != null && u.Perfil.Destaque).ThenBy(u => u.Nome);


            foreach (var user in filtrados)
            {
                painelArtistas.Children.Add(CriarCard(user));
            }
        }

        private void BtnVerMais_Click(object sender, RoutedEventArgs e)
        {
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


    }
}