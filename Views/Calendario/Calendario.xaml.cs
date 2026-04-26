using ProjetoAcelera.Views.Teste;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using ProjetoAcelera.Views.Artistas;
using ProjetoAcelera.Views.Teste;

namespace ProjetoAcelera.Views.Calendario
{
    public partial class Calendario : Window
    {
        private int _anoAtual;
        private int _mesAtual;
        private readonly DispatcherTimer _timer;

        public Calendario()
        {
            InitializeComponent();

            Loaded += Calendario_Loaded;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1)
            };
            _timer.Tick += Timer_Tick;
        }

        private void Calendario_Loaded(object sender, RoutedEventArgs e)
        {
            var agora = DateTime.Now;
            _anoAtual = agora.Year;
            _mesAtual = agora.Month;

            GerarCalendario(_anoAtual, _mesAtual);
            _timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            var agora = DateTime.Now;
            if (agora.Year != _anoAtual || agora.Month != _mesAtual)
            {
                _anoAtual = agora.Year;
                _mesAtual = agora.Month;
                GerarCalendario(_anoAtual, _mesAtual);
                return;
            }

            AtualizarDestaqueDoDia(agora.Date);
        }

        private void AtualizarDestaqueDoDia(DateTime hoje)
        {
            foreach (var child in gridDias.Children)
            {
                if (child is Border b && b.Tag is DateTime data)
                {
                    if (data.Date == hoje)
                    {
                        b.Background = new SolidColorBrush(Color.FromRgb(184, 134, 11));
                        if (b.Child is TextBlock t) t.Foreground = Brushes.White;
                    }
                    else
                    {
                        b.Background = Brushes.Transparent;
                        if (b.Child is TextBlock t) t.Foreground = Brushes.Black;
                    }
                }
            }
        }

        private void GerarCalendario(int ano, int mes)
        {
            gridDias.Children.Clear();

            // Nome do mês em português
            string[] meses = { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                              "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };
            txtMes.Text = meses[mes - 1] + " " + ano;

            DateTime primeiroDia = new DateTime(ano, mes, 1);
            int diasNoMes = DateTime.DaysInMonth(ano, mes);

            // Ajusta para começar na segunda-feira
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

                if (data == hoje)
                {
                    borda.Background = new SolidColorBrush(Color.FromRgb(184, 134, 11));
                    texto.Foreground = Brushes.White;
                }
                else
                {
                    borda.Background = Brushes.Transparent;
                    texto.Foreground = Brushes.Black;
                }

                borda.Child = texto;
                borda.ToolTip = data.ToString("dddd, d 'de' MMMM", CultureInfo.CurrentCulture);

                gridDias.Children.Add(borda);
            }

            while (gridDias.Children.Count < 42)
            {
                gridDias.Children.Add(new Border { Background = Brushes.Transparent, Margin = new Thickness(3) });
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

        private void NossaCidade_Click(object sender, RoutedEventArgs e)
        {
            var tela = new Dashboard();
            tela.Show();
            this.Close();
        }

        private void Conta_Click(object sender, RoutedEventArgs e)
        {
            var tela = new Views.Perfil.TelaPerfil();
            tela.Show();
            this.Close();
        }

        private void Artistas_Click(object sender, RoutedEventArgs e)
        {
            TelaArtista tela = new TelaArtista();
            tela.Show();
            this.Close();
        }
    }
}
