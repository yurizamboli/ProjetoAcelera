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

namespace ProjetoAcelera.Views.Home
{
    public partial class TelaHome : Window
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

            // Tenta carregar a imagem; se falhar, deixa sem imagem (não trava o app)
            try
            {
                imgEvento.Source = new BitmapImage(new Uri(evento.Imagem, UriKind.Relative));
            }
            catch
            {
                imgEvento.Source = null;
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

        //  Navegação entre telas 
        // Padrão: abre a nova tela, depois fecha a atual

        private void Programacao_Click(object sender, RoutedEventArgs e)
        {
            new Views.Calendario.Calendario().Show();
            this.Close();
        }

        private void Cultura_Click(object sender, RoutedEventArgs e)
        {
            
            
        }

        private void Artistas_Click(object sender, RoutedEventArgs e)
        {
            new TelaArtista().Show();
            this.Close();
        }

        private void NossaCidade_Click(object sender, RoutedEventArgs e)
        {
            new Views.Teste.Dashboard().Show();
            this.Close();
        }

        private void Conta_Click(object sender, RoutedEventArgs e)
        {
            var usuario = App.UsuarioService.UsuarioLogado;

            if (usuario.Cargo == "Admin")
            {
                new TelaAdmin().Show();
            }
            else
            {
                new TelaPerfil().Show();
            }

            this.Close();
        }
    }
}
