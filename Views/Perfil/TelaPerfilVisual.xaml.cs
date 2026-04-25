using ProjetoAcelera.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System;

namespace ProjetoAcelera.Views.Perfil
{
    public partial class TelaPerfilVisual : Window
    {
        private Usuario usuario;

        public TelaPerfilVisual(Usuario user)
        {
            InitializeComponent();

            usuario = user;

            CarregarPerfil();
            CarregarObras();
        }

        private void CarregarPerfil()
        {
            txtNome.Text = usuario.Nome;
            txtBio.Text = usuario.Perfil?.Bio;
            txtFacebook.Text = usuario.Perfil?.Facebook;
            txtInstagram.Text = usuario.Perfil?.Instagram;

            try
            {
                imgPerfil.Source = new BitmapImage(new Uri(usuario.Perfil.FotoPerfil));
            }
            catch 
            {
                
            }
        }

        private void CarregarObras()
        {
            painelObras.Children.Clear();

            if (usuario.Obras == null || usuario.Obras.Count == 0)
                return;

            foreach (var obra in usuario.Obras)
            {
                painelObras.Children.Add(CriarCardObra(obra));
            }
        }

        private Border CriarCardObra(Obra obra)
        {
            StackPanel container = new StackPanel();

            Image img = new Image
            {
                Height = 100,
                Stretch = Stretch.UniformToFill
            };

            try
            {
                img.Source = new BitmapImage(new Uri(obra.Capa));
            }

            catch 
            {
                
            }
            TextBlock titulo = new TextBlock
            {
                Text = obra.Titulo,
                TextAlignment = TextAlignment.Center,
                FontSize = 12
            };

            container.Children.Add(img);
            container.Children.Add(titulo);

            return new Border
            {
                Width = 100,
                Height = 130,
                Margin = new Thickness(5),
                BorderBrush = Brushes.DarkBlue,
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(10),
                Child = container
            };
        }
    }
}