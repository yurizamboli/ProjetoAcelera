using ProjetoAcelera.Views.Teste;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ProjetoAcelera.Views.Artistas;

namespace ProjetoAcelera.Views.Home
{
    public partial class Home : Window
    {
        private int indiceAtual = 0;
        private List<Evento> eventos;

        public Home()
        {
            InitializeComponent();
            CarregarEventos();
            MostrarEvento();
        }

        private void CarregarEventos()
        {
            eventos = new List<Evento>
            {
                new Evento
                {
                    Titulo = "Evento Cultural",
                    Data = "25 ABR | 19H",
                    Descricao = "Evento cultural no Museu Major Novaes",
                    Detalhes = "Uma noite especial com música ao vivo, exposições e literatura.",
                    Imagem = "imagens/evento1.png"
                },
                new Evento
                {
                    Titulo = "Feira do Livro",
                    Data = "10 MAI | 14H",
                    Descricao = "Grande feira literária",
                    Detalhes = "Diversos autores, livros e atividades culturais.",
                    Imagem = "imagens/evento2.png"
                }
            };
        }

        private void MostrarEvento()
        {
            var evento = eventos[indiceAtual];

            txtTitulo.Text = evento.Titulo;
            txtData.Text = evento.Data;
            txtDescricao.Text = evento.Descricao;
            txtDetalhes.Text = evento.Detalhes;

            imgEvento.Source = new BitmapImage(new Uri(evento.Imagem, UriKind.Relative));
        }

        private void BtnProximo_Click(object sender, RoutedEventArgs e)
        {
            indiceAtual++;
            if (indiceAtual >= eventos.Count)
                indiceAtual = 0;

            MostrarEvento();
        }

        private void BtnAnterior_Click(object sender, RoutedEventArgs e)
        {
            indiceAtual--;
            if (indiceAtual < 0)
                indiceAtual = eventos.Count - 1;

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

    public class Evento
    {
        public string Titulo { get; set; }
        public string Data { get; set; }
        public string Descricao { get; set; }
        public string Detalhes { get; set; }
        public string Imagem { get; set; }
    }
}