using ProjetoAcelera.Services;
using ProjetoAcelera.Views.Artistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProjetoAcelera.Views.Home
{
    public partial class TelaHome : Window
    {
        private int indiceAtual = 0;
        private EventoService eventoService;

        public TelaHome()
        {
            InitializeComponent();
            eventoService = new EventoService();
            MostrarEvento();
        }

       

        private void MostrarEvento()
        {
            var evento = eventoService.ObterEvento()[indiceAtual];

            txtTitulo.Text = evento.Titulo;
            txtData.Text = evento.Data;
            txtDescricao.Text = evento.Descricao;
            txtDetalhes.Text = evento.Detalhes;

            imgEvento.Source = new BitmapImage(new Uri(evento.Imagem, UriKind.Relative));
        }

        private void BtnProximo_Click(object sender, RoutedEventArgs e)
        {
            indiceAtual++;
            if (indiceAtual >= eventoService.ObterEvento().Count)
                indiceAtual = 0;

            MostrarEvento();
        }

        private void BtnAnterior_Click(object sender, RoutedEventArgs e)
        {
            indiceAtual--;
            if (indiceAtual < 0)
                indiceAtual = eventoService.ObterEvento().Count - 1;

            MostrarEvento();
        }

        private void Artistas_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implementar navegação para Artistas
        }

        private void NossaCidade_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implementar navegação para Nossa Cidade
        }

        private void Conta_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implementar navegação para Conta
        }
    }


}