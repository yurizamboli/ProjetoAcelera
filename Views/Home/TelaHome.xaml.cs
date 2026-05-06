using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using ProjetoAcelera.Views.Admin;
using ProjetoAcelera.Views.Artistas;
using ProjetoAcelera.Views.Teste;
using ProjetoAcelera.Views.Perfil;
using ProjetoAcelera.Views.Calendario;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace ProjetoAcelera.Views.Home
{
    public partial class TelaHome : Page
    {
        // Índice do evento atual no carrossel
        private int indiceAtual = 0;

        // Serviço que busca os eventos (de um banco, arquivo, etc.)
        private EventoService eventoService;

        // Lista de eventos carregada uma única vez
        private List<Evento> listaEventos;

        public TelaHome()
        {
            InitializeComponent();
           

            eventoService = new EventoService();

            // Carrega todos os eventos de uma vez só (mais eficiente)
            listaEventos = eventoService.ObterEvento();

            // Verificação de segurança: se não houver eventos, avisa e para
            if (listaEventos == null || listaEventos.Count == 0)
            {
                MessageBox.Show("Nenhum evento encontrado.",
                                "Aviso",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            // Mostra o primeiro evento
            MostrarEvento();
        }


        private void MostrarEvento()
        {
            var evento = listaEventos[indiceAtual];

            txtTitulo.Text = evento.Titulo;
            txtData.Text = evento.Data;
            txtDescricao.Text = evento.Descricao;
            txtDetalhes.Text = evento.Detalhes;

            // Carrega a imagem do evento
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(evento.Imagem, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                imgEvento.Source = bitmap;
            }
            catch
            {
                // Se falhar, tenta carregar imagem padrão
                try
                {
                    imgEvento.Source = new BitmapImage(new Uri("/ImagemAcelera/evento1.png", UriKind.Relative));
                }
                catch { }
            }
        }

        // Navegação do carrossel//

        private void BtnProximo_Click(object sender, RoutedEventArgs e)
        {
            indiceAtual++;

            // Volta ao início quando passar do último//
            if (indiceAtual >= listaEventos.Count)
                indiceAtual = 0;

            MostrarEvento();
        }

        private void BtnAnterior_Click(object sender, RoutedEventArgs e)
        {
            indiceAtual--;

            // Vai ao último quando estiver no primeiro e clicar em "anterior"
            if (indiceAtual < 0)
                indiceAtual = listaEventos.Count - 1;

            MostrarEvento();
        }


        private void Programacao_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Views.Calendario.Calendario());
        }

        private void Artistas_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Views.Artistas.TelaArtista());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NossaCidade_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Views.Teste.Dashboard());
        }
    }
}
