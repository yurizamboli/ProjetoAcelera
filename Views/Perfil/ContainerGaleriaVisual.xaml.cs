using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using ProjetoAcelera.Ferramentas;

namespace ProjetoAcelera.Views.Perfil
{
    public partial class ContainerGaleriaVisual : Page
    {
        private Usuario usuario;
        private int quantidadeImagensExibidas = 12;
        private const int quantidadeCarregarMais = 12;
        public ContainerGaleriaVisual(Usuario user)
        {
            InitializeComponent();

            usuario = user;

            CarregarGaleria();
        }
        private void CarregarGaleria()
        {
            painelGaleria.Children.Clear();

            if (usuario.Publicacoes == null)
            { 
                return; 
            }


            var todosPosts = usuario.Publicacoes.Where(p => !string.IsNullOrWhiteSpace(p.ImagemUrl)).OrderByDescending(p => p.DataPublicacao).ToList();

            var posts = todosPosts.Take(quantidadeImagensExibidas).ToList();

            foreach (var post in posts)
            {
                painelGaleria.Children.Add(CriarImagem(post.ImagemUrl));
            }
            btnCarregarMais.Visibility = quantidadeImagensExibidas < todosPosts.Count? Visibility.Visible : Visibility.Collapsed;
        }
        

        private Border CriarImagem(string caminho)
        {
            Image img = new Image
            {
                Width = 180,
                Height = 180,
                Stretch = Stretch.UniformToFill
            };

            try
            {
                img.Source = AuxilioImagens.CarregarImgOtimizada(caminho, 400);
            }
            catch
            {

            }        
            Border border = new Border
            {
                Width = 180,
                Height = 180,
                Margin = new Thickness(10),
                CornerRadius = new CornerRadius(12),
                ClipToBounds = true,
                BorderBrush = Brushes.DarkGoldenrod,
                BorderThickness = new Thickness(2),
                Child = img
            };
            border.MouseDown += (s, e) =>
            { 
                var janela = new JanelaImagemFull(caminho); janela.ShowDialog(); 
            };
            return border;
        }

        private void BtnCarregarMais_Click(object sender, RoutedEventArgs e)
        {
            quantidadeImagensExibidas += quantidadeCarregarMais;
            CarregarGaleria();
        }
    }
}