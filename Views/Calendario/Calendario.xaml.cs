using ProjetoAcelera.Models;
using ProjetoAcelera.Views.Admin;
using ProjetoAcelera.Views.Artistas;
using ProjetoAcelera.Views.Home;
using ProjetoAcelera.Views.Perfil;
using ProjetoAcelera.Views.Teste;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace ProjetoAcelera.Views.Calendario
{
    public partial class Calendario : Page
    {
        private int _anoAtual;
        private int _mesAtual;
        private DateTime? _dataSelecionada = null;

        public Calendario()
        {
            InitializeComponent();

            Loaded += Calendario_Loaded;

        }

        private void Calendario_Loaded(object sender, RoutedEventArgs e)
        {
            var agora = DateTime.Now;
            _anoAtual = agora.Year;
            _mesAtual = agora.Month;

            GerarCalendario(_anoAtual, _mesAtual);
        }

        private void MostrarEventosDoDia(DateTime data)
        {
            if (_dataSelecionada.HasValue && _dataSelecionada.Value.Date == data.Date)
            {
                _dataSelecionada = null;
                listaEventosDoDia.ItemsSource = null;
                txtDataSelecionada.Text = "Selecione um dia no calendário";
                GerarCalendario(_anoAtual, _mesAtual);
                return;
            }

            _dataSelecionada = data.Date;
            var eventosDoDia = App.EventoService.ObterEventosPorData(data);
            txtDataSelecionada.Text = data.ToString("dddd, d 'de' MMMM 'de' yyyy", CultureInfo.CurrentCulture);
            if (eventosDoDia.Any())
            {
                listaEventosDoDia.ItemsSource = eventosDoDia;
            }
            else
            {
                listaEventosDoDia.ItemsSource = null;
                txtDataSelecionada.Text += " - nenhum evento cadastrado";
            }

            GerarCalendario(_anoAtual, _mesAtual);
        }

        private void GerarCalendario(int ano, int mes)
        {
            gridDias.Children.Clear();

            string[] meses = { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                      "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };

            txtMes.Text = meses[mes - 1] + " " + ano;

            DateTime primeiroDia = new DateTime(ano, mes, 1);
            int diasNoMes = DateTime.DaysInMonth(ano, mes);

            int diaSemana = ((int)primeiroDia.DayOfWeek + 6) % 7;

            for (int i = 0; i < diaSemana; i++)
            {
                var placeholder = new Border
                {
                    Background = Brushes.Transparent,
                    Margin = new Thickness(3)
                };

                gridDias.Children.Add(placeholder);
            }

            DateTime hoje = DateTime.Now.Date;

            for (int dia = 1; dia <= diasNoMes; dia++)
            {
                DateTime data = new DateTime(ano, mes, dia);

                var eventosDoDia = App.EventoService.ObterEventosPorData(data);
                bool temEvento = eventosDoDia.Any();

                var texto = new TextBlock
                {
                    Text = dia.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(4),
                    FontSize = 16,
                    FontWeight = FontWeights.SemiBold
                };

                var borda = new Border
                {
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(6),
                    Margin = new Thickness(3),
                    Tag = data
                };
                bool diaSelecionado =_dataSelecionada.HasValue && _dataSelecionada.Value.Date == data.Date;
                if (temEvento)
                {
                    borda.Background = new SolidColorBrush(Color.FromRgb(31, 58, 95)); // azul #1F3A5F
                    borda.BorderBrush = new SolidColorBrush(Color.FromRgb(184, 134, 11)); // dourado #B8860B
                    borda.BorderThickness = new Thickness(diaSelecionado ? 3 : 2);
                    texto.Foreground = Brushes.White;
                }
                else if (data == hoje)
                {
                    borda.Background = new SolidColorBrush(Color.FromRgb(184, 134, 11)); // dourado
                    borda.BorderBrush = Brushes.Transparent;
                    borda.BorderThickness = new Thickness(0);
                    texto.Foreground = Brushes.White;
                }
                else
                {
                    borda.Background = Brushes.Transparent;
                    borda.BorderBrush = Brushes.Transparent;
                    borda.BorderThickness = new Thickness(0);
                    texto.Foreground = Brushes.Black;
                }

                borda.ToolTip = null;
                borda.Cursor = System.Windows.Input.Cursors.Hand;

                borda.MouseLeftButtonDown += (s, e) =>
                {
                    MostrarEventosDoDia(data);
                };

                borda.Child = texto;

                gridDias.Children.Add(borda);
            }

            while (gridDias.Children.Count < 42)
            {
                gridDias.Children.Add(new Border
                {
                    Background = Brushes.Transparent,
                    Margin = new Thickness(3)
                });
            }
        }

        private void BtnAnterior_Click(object sender, RoutedEventArgs e)
        {
            _mesAtual--;
            if (_mesAtual < 1)
            {
                _mesAtual = 12;
                _anoAtual--;
            }
            GerarCalendario(_anoAtual, _mesAtual);
        }

        private void BtnProximo_Click(object sender, RoutedEventArgs e)
        {
            _mesAtual++;
            if (_mesAtual > 12)
            {
                _mesAtual = 1;
                _anoAtual++;
            }
            GerarCalendario(_anoAtual, _mesAtual);
        }
        private void AbrirImagemEvento_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var imagem = sender as Image;
            if (imagem == null)
            {
                return;
            }
            var evento = imagem.DataContext as Evento;
            if (evento == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(evento.Imagem))
            {
                return;
            }
            JanelaImagemFull janela = new JanelaImagemFull(evento.Imagem);
            janela.ShowDialog();
        }

    }
}
