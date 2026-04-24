using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

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
            // Se mudou o mês/ano, reconstruir todo o calendário
            if (agora.Year != _anoAtual || agora.Month != _mesAtual)
            {
                _anoAtual = agora.Year;
                _mesAtual = agora.Month;
                GerarCalendario(_anoAtual, _mesAtual);
                return;
            }

            // Caso contrário, apenas atualiza o destaque do dia atual
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
                        b.Background = new SolidColorBrush(Color.FromRgb(184, 134, 11)); // dourado
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

            // Mostra o nome do mês e o ano
            txtMes.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mes) + " " + ano.ToString();

            DateTime primeiroDia = new DateTime(ano, mes, 1);
            int diasNoMes = DateTime.DaysInMonth(ano, mes);

            // Ajusta a semana, começando na segunda-feira
            int diaSemana = ((int)primeiroDia.DayOfWeek + 6) % 7;

            // Espaços vazios antes do dia 1
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

            // Dias do mês
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

                // Destaque para o dia de hoje
                if (data == hoje)
                {
                    borda.Background = new SolidColorBrush(Color.FromRgb(184, 134, 11)); // dourado
                    texto.Foreground = Brushes.White;
                }
                else
                {
                    borda.Background = Brushes.Transparent;
                    texto.Foreground = Brushes.Black;
                }

                borda.Child = texto;

                // Tooltip para mostrar data completa
                borda.ToolTip = data.ToString("dddd, d 'de' MMMM", CultureInfo.CurrentCulture);

                gridDias.Children.Add(borda);
            }

            // Preencher células restantes para manter 6 linhas (6*7=42)
            while (gridDias.Children.Count < 42)
            {
                gridDias.Children.Add(new Border { Background = Brushes.Transparent, Margin = new Thickness(3) });
            }
        }
    }
}
