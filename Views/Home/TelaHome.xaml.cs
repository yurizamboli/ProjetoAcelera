using ProjetoAcelera.Services;
using ProjetoAcelera.Views.Admin;
using ProjetoAcelera.Views.Artistas;
using ProjetoAcelera.Views.Perfil;
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

        private void Artistas_Click(object sender, RoutedEventArgs e)
        {
            TelaArtista tela = new TelaArtista();
            tela.Show();
            this.Close();
        }

        private void NossaCidade_Click(object sender, RoutedEventArgs e)
        {
            var tela = new Views.Teste.Dashboard();
            tela.Show();
            this.Close();
        }
    }


}