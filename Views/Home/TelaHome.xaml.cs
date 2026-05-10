using ProjetoAcelera.Models;
using ProjetoAcelera.Services;
using ProjetoAcelera.Views.Admin;
using ProjetoAcelera.Views.Artistas;
using ProjetoAcelera.Views.Teste;
using ProjetoAcelera.Views.Perfil;
using ProjetoAcelera.Views.Calendario;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace ProjetoAcelera.Views.Home
{
    public partial class TelaHome : Page
    {
        // Índice do evento atual no carrossel
        private int indiceAtual = 0;
        private string? caminhoImagemPostagem;
        private string? caminhoVideoPostagem;

        // Serviço que busca os eventos (de um banco, arquivo, etc.)
        private EventoService eventoService;

        // Lista de eventos carregada uma única vez
        private List<Evento> listaEventos = new List<Evento>();

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
            InicializarPostagem();
        }

        private void InicializarPostagem()
        {
            var usuario = App.UsuarioService.UsuarioLogado;

            txtStatusPostagem.Text = string.Empty;
            txtAnexoStatus.Text = string.Empty;
            caminhoImagemPostagem = null;
            caminhoVideoPostagem = null;

            if (usuario != null)
            {
                txtUsuarioPostagem.Text = $"Olá, {usuario.Nome}";
            }
            else
            {
                txtUsuarioPostagem.Text = "Faça login para publicar uma postagem.";
            }
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

        private void AdicionarFoto_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Imagens|*.jpg;*.jpeg;*.png;*.gif"
            };

            if (dialog.ShowDialog() == true)
            {
                caminhoImagemPostagem = dialog.FileName;
                caminhoVideoPostagem = null;
                txtAnexoStatus.Text = $"Imagem selecionada: {Path.GetFileName(caminhoImagemPostagem)}";
                txtStatusPostagem.Text = string.Empty;
            }
        }

        private void AdicionarVideo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Vídeos|*.mp4;*.mov;*.avi;*.wmv"
            };

            if (dialog.ShowDialog() == true)
            {
                caminhoVideoPostagem = dialog.FileName;
                caminhoImagemPostagem = null;
                txtAnexoStatus.Text = $"Vídeo selecionado: {Path.GetFileName(caminhoVideoPostagem)}";
                txtStatusPostagem.Text = string.Empty;
            }
        }

        private void Postar_Click(object sender, RoutedEventArgs e)
        {
            var usuario = App.UsuarioService.UsuarioLogado;

            if (usuario == null)
            {
                MessageBox.Show("Faça login para publicar uma postagem.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPostTexto.Text))
            {
                MessageBox.Show("Escreva algo antes de postar.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var postagem = new Postagem
            {
                AutorEmail = usuario.Email,
                AutorNome = usuario.Nome,
                Texto = txtPostTexto.Text.Trim(),
                CaminhoImagem = caminhoImagemPostagem,
                CaminhoVideo = caminhoVideoPostagem,
                Status = "Aguardando aprovação",
                DataCriacao = DateTime.Now
            };

            App.PostagemService.AdicionarPostagem(postagem);

            txtPostTexto.Text = string.Empty;
            txtAnexoStatus.Text = string.Empty;
            txtStatusPostagem.Text = "Sua postagem está sendo analisada pelo administrador.";
            caminhoImagemPostagem = null;
            caminhoVideoPostagem = null;
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
