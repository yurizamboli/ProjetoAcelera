using System;
using ProjetoAcelera.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProjetoAcelera.Ferramentas
{
    public static class PublicacaoComponentesVisual
    {
        public static Border CriarLinhaAzul(Thickness margin)
        {
            return new Border
            {
                Height = 1,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                Margin = margin
            };
        }
        public static Border CriarCardPublicacao()
        {
            return new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF7E1")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C9B27D")),
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(16),
                Margin = new Thickness(0, 0, 0, 18),
                Padding = new Thickness(15)
            };
        }

        public static Border CriarMolduraAvatar()
        {
            return new Border
            {
                Width = 45,
                Height = 45,
                CornerRadius = new CornerRadius(0),
                ClipToBounds = true,
                Background = Brushes.LightGray,
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                BorderThickness = new Thickness(1)
            };
        }

        public static TextBlock CriarTextoAutor(string nome)
        {
            return new TextBlock
            {
                Text = nome,
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F"))
            };
        }

        public static TextBlock CriarTextoData(string data)
        {
            return new TextBlock
            {
                Text = data,
                FontSize = 11,
                Foreground = Brushes.Gray
            };
        }

        public static TextBlock CriarTextoConteudo(string texto)
        {
            return new TextBlock
            {
                Text = texto,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Margin = new Thickness(0, 5, 0, 12)
            };
        }

        public static Border CriarMolduraImagem()
        {
            return new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E1CF")),
                CornerRadius = new CornerRadius(12),
                ClipToBounds = true,
                Margin = new Thickness(0, 0, 0, 10)
            };
        }

        public static TextBlock CriarTextoCurtidas(int curtidas)
        {
            return new TextBlock
            {
                Text = $"❤️ {curtidas} curtidas",
                FontWeight = FontWeights.Bold,
                FontSize = 13,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                VerticalAlignment = VerticalAlignment.Center
            };
        }
        public static Grid CriarAreaStats()
        {
            Grid areaStats = new Grid
            {
                Margin = new Thickness(0, 4, 0, 10)
            };

            areaStats.ColumnDefinitions.Add(
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            );

            areaStats.ColumnDefinitions.Add(
                new ColumnDefinition { Width = GridLength.Auto }
            );

            return areaStats;
        }

        public static StackPanel CriarStatsEsquerda()
        {
            return new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        public static TextBlock CriarTextoComentariosBloqueados()
        {
            return new TextBlock
            {
                Text = "🚫 Comentários desativados",
                FontWeight = FontWeights.Bold,
                FontSize = 13,
                Margin = new Thickness(24, 0, 0, 0),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        public static Button CriarBotaoExibirComentarios(int totalComentarios)
        {
            return new Button
            {
                Content = $"▼ Exibir comentários ({totalComentarios})",
                Height = 24,
                Margin = new Thickness(24, 0, 0, 0),
                Padding = new Thickness(0),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                FontWeight = FontWeights.Bold,
                FontSize = 13,
                Cursor = Cursors.Hand,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        public static Border CriarCardComentario()
        {
            return new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EFE4C8")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C9B27D")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 8, 0, 0)
            };
        }

        public static TextBlock CriarNomeComentario(string nome)
        {
            return new TextBlock
            {
                Text = nome,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F"))
            };
        }

        public static TextBlock CriarTextoComentario(string texto)
        {
            return new TextBlock
            {
                Text = texto,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 3, 0, 5),
                Foreground = Brushes.Black
            };
        }

        public static TextBlock CriarStatusComentario(string status)
        {
            return new TextBlock
            {
                Text = "Status: " + status,
                FontSize = 11,
                FontWeight = FontWeights.Bold,
                Foreground = status == "Aprovado" ? Brushes.Green : Brushes.DarkOrange,
                Margin = new Thickness(0, 0, 0, 6)
            };
        }


        ///////////////////////////COMENTARIOS/////////////////////////
        public static void CriarAreaComentarios(Publicacao pub,StackPanel areaStats,StackPanel stack,bool mostrarTodosComentarios, bool permitirAprovacao,Action<Guid> aprovarComentario,Action<Guid> reprovarComentario)
        {
            var comentariosParaExibir = pub.Comentarios ?? new List<Comentario>();

            if (!mostrarTodosComentarios)
            {
                comentariosParaExibir = comentariosParaExibir.Where(c => c.Status == "Aprovado").ToList();
            }

            int totalComentarios = comentariosParaExibir.Count;

            if (!pub.ComentariosPermitidos)
            {
                TextBlock comentariosBloqueados = CriarTextoComentariosBloqueados();

                areaStats.Children.Add(comentariosBloqueados);
                return;
            }

            StackPanel comentariosContainer = new StackPanel
            {
                Margin = new Thickness(0, 5, 0, 0)
            };

            ScrollViewer scrollComentarios = new ScrollViewer
            {
                MaxHeight = 220,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Content = comentariosContainer,
                Visibility = Visibility.Collapsed,
                Margin = new Thickness(0, 8, 0, 0)
            };

            Button btnExibirComentarios = CriarBotaoExibirComentarios(totalComentarios);

            btnExibirComentarios.Click += (s, e) =>
            {
                if (scrollComentarios.Visibility == Visibility.Visible)
                {
                    scrollComentarios.Visibility = Visibility.Collapsed;
                    btnExibirComentarios.Content = $"▼ Exibir comentários ({totalComentarios})";
                }
                else
                {
                    scrollComentarios.Visibility = Visibility.Visible;
                    btnExibirComentarios.Content = $"▲ Ocultar comentários ({totalComentarios})";
                }
            };

            if (totalComentarios == 0)
            {
                Border semComentariosCard = CriarCardComentario();

                TextBlock semComentariosTexto = new TextBlock
                {
                    Text = "Nenhum comentário ainda.",
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F"))
                };

                semComentariosCard.Child = semComentariosTexto;
                comentariosContainer.Children.Add(semComentariosCard);
            }
            else
            {
                foreach (var comentario in comentariosParaExibir)   
                {
                    if (!mostrarTodosComentarios && comentario.Status != "Aprovado")
                    {
                        continue;
                    }

                    Border comentarioCard = CriarCardComentario();
                    StackPanel comentarioStack = new StackPanel();
                    TextBlock nomeComentario = CriarNomeComentario(comentario.NomeAutor);
                    TextBlock textoComentario = CriarTextoComentario(comentario.Conteudo);
                    comentarioStack.Children.Add(nomeComentario);
                    comentarioStack.Children.Add(textoComentario);

                    if (mostrarTodosComentarios)
                    {
                        TextBlock statusComentario = CriarStatusComentario(comentario.Status);
                        comentarioStack.Children.Add(statusComentario);
                    }

                    if (permitirAprovacao && comentario.Status == "Aguardando aprovação")
                    {
                        StackPanel botoesComentario = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            HorizontalAlignment = HorizontalAlignment.Right
                        };

                        Button btnAprovarComentario = new Button
                        {
                            Content = "Aprovar",
                            Width = 90,
                            Height = 28,
                            Margin = new Thickness(0, 0, 8, 0),
                            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                            Foreground = Brushes.White,
                            FontWeight = FontWeights.Bold,
                            BorderThickness = new Thickness(0),
                            Cursor = Cursors.Hand
                        };

                        btnAprovarComentario.Click += (s, e) =>
                        {
                            aprovarComentario(comentario.Id);
                        };

                        Button btnReprovarComentario = new Button
                        {
                            Content = "Reprovar",
                            Width = 90,
                            Height = 28,
                            Background = Brushes.DarkRed,
                            Foreground = Brushes.White,
                            FontWeight = FontWeights.Bold,
                            BorderThickness = new Thickness(0),
                            Cursor = Cursors.Hand
                        };

                        btnReprovarComentario.Click += (s, e) =>
                        {
                            reprovarComentario(comentario.Id);
                        };

                        botoesComentario.Children.Add(btnAprovarComentario);
                        botoesComentario.Children.Add(btnReprovarComentario);
                        comentarioStack.Children.Add(botoesComentario);
                    }
                    comentarioCard.Child = comentarioStack;
                    comentariosContainer.Children.Add(comentarioCard);
                }
            }

            areaStats.Children.Add(btnExibirComentarios);
            stack.Children.Add(scrollComentarios);
        }
        public static StackPanel CriarCampoComentario(Publicacao pub, Action<string> enviarComentario)
        {
            StackPanel areaComentario = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 12, 0, 0),
                Visibility = pub.ComentariosPermitidos ? Visibility.Visible : Visibility.Collapsed
            };

            Border campoBorder = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EFE4C8")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C9B27D")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(18),
                Height = 38,
                Width = 560,
                Padding = new Thickness(12, 0, 12, 0)
            };

            Grid gridCampo = new Grid();

            TextBox txtComentario = new TextBox
            {
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                FontSize = 13,
                Padding = new Thickness(0),
                Foreground = Brushes.Black,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            TextBlock placeholder = new TextBlock
            {
                Text = "Escreva um comentário...",
                Foreground = Brushes.Gray,
                FontSize = 13,
                VerticalAlignment = VerticalAlignment.Center,
                IsHitTestVisible = false
            };

            txtComentario.TextChanged += (s, e) =>
            {
                placeholder.Visibility = string.IsNullOrWhiteSpace(txtComentario.Text)? Visibility.Visible: Visibility.Collapsed;
            };

            gridCampo.Children.Add(txtComentario);
            gridCampo.Children.Add(placeholder);

            campoBorder.Child = gridCampo;

            Button btnComentar = new Button
            {
                Content = "Comentar",
                Width = 100,
                Height = 34,
                Margin = new Thickness(10, 2, 0, 0),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F3A5F")),
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand
            };

            btnComentar.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtComentario.Text))
                {
                    return;
                }

                enviarComentario(txtComentario.Text.Trim());

                txtComentario.Clear();
                placeholder.Visibility = Visibility.Visible;
            };

            areaComentario.Children.Add(campoBorder);
            areaComentario.Children.Add(btnComentar);

            return areaComentario;
        }
    }
}